using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>A request coordinator which retries failed requests with a delay between each attempt.</summary>
	internal class ZoomRetryCoordinator : IRequestCoordinator
	{
		private readonly IRequestCoordinator _defaultRetryCoordinator;
		private readonly ITokenHandler _tokenHandler;
		private (string RefreshToken, string AccessToken, DateTime TokenExpiration, int TokenIndex) _previousTokenInfo = (null, null, DateTime.MinValue, 0);

		public ITokenHandler TokenHandler { get { return _tokenHandler; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomRetryCoordinator" /> class.
		/// </summary>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ZoomRetryCoordinator(ITokenHandler tokenHandler)
		{
			_tokenHandler = tokenHandler;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomRetryCoordinator" /> class.
		/// </summary>
		/// <param name="config">The retry configuration.</param>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ZoomRetryCoordinator(IRetryConfig config, ITokenHandler tokenHandler)
			: this(tokenHandler)
		{
			_defaultRetryCoordinator = new RetryCoordinator(config);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomRetryCoordinator" /> class.
		/// </summary>
		/// <param name="config">The retry configuration.</param>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ZoomRetryCoordinator(IEnumerable<IRetryConfig> config, ITokenHandler tokenHandler)
			: this(tokenHandler)
		{
			_defaultRetryCoordinator = new RetryCoordinator(config);
		}

		/// <summary>Dispatch an HTTP request.</summary>
		/// <param name="request">The response message to validate.</param>
		/// <param name="dispatcher">A method which executes the request.</param>
		/// <returns>The final HTTP response.</returns>
		public async Task<HttpResponseMessage> ExecuteAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
		{
			var requestUri = request.Message.RequestUri;
			HttpResponseMessage response;

			// The two scenarios where we need basic authentication are:
			//  - The Zoom API endpoint requires it (as of this writing the only such end point that I'm aware of is oauth/data/compliance which happens to be obsolete).
			//  - the token handler is null (for instance, when unit testing)
			var isBasicAuthentication = string.Equals(request.Message.Headers.Authorization?.Scheme, "Basic", StringComparison.OrdinalIgnoreCase);
			isBasicAuthentication |= _tokenHandler == null;

			// Basic authentication does not need any special token management logic while OAuth requires us to manage the token, refresh it when it expires, etc.
			if (isBasicAuthentication) response = await ExecuteWithBasicAuthenticationAsync(request, dispatcher).ConfigureAwait(false);
			else response = await ExecuteWithOAuthAuthenticationAsync(request, dispatcher).ConfigureAwait(false);

			// Check if the request was redirected and includes an authorization header.
			var finalRequestUri = response.RequestMessage.RequestUri;
			var requestRedirected = finalRequestUri != requestUri;
			var requestIncludesAuthorization = request.Message.Headers.Authorization != null;

			if (requestRedirected && requestIncludesAuthorization)
			{
				// When a redirect occurs, Microsoft's HTTP client strips out the 'Authorization'
				// header which causes the Zoom API to reject the request. The solution is to
				// re-issue the original request (including all necessary headers) at the new url.
				// See https://github.com/Jericho/ZoomNet/issues/257 for details.
				request.Message.RequestUri = finalRequestUri;
				response = await _defaultRetryCoordinator.ExecuteAsync(request, dispatcher);
			}

			return response;
		}

		private Task<HttpResponseMessage> ExecuteWithBasicAuthenticationAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
		{
			// Simply dispatch the request.
			return _defaultRetryCoordinator.ExecuteAsync(request, dispatcher);
		}

		/// <summary>Dispatch an HTTP request.</summary>
		/// <param name="request">The response message to validate.</param>
		/// <param name="dispatcher">A method which executes the request.</param>
		/// <returns>The final HTTP response.</returns>
		private async Task<HttpResponseMessage> ExecuteWithOAuthAuthenticationAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
		{
			var tokenInfo = await _tokenHandler.GetTokenInfoAsync(false, _previousTokenInfo, request.CancellationToken).ConfigureAwait(false);
			request = request.WithBearerAuthentication(tokenInfo.AccessToken);

			// Dispatch the request
			var response = await _defaultRetryCoordinator.ExecuteAsync(request, dispatcher).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
			{
				_previousTokenInfo = tokenInfo;
				return response;
			}

			// Check if the token needs to be refreshed
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				var responseContent = await response.Content.ReadAsStringAsync(null).ConfigureAwait(false);
				var jsonResponse = JsonDocument.Parse(responseContent).RootElement;
				var message = jsonResponse.GetPropertyValue("message", string.Empty);

				// We want to refresh the token and retry the request when the Zoom returns "access token is expired".
				var shouldRetry = message.StartsWith("access token is expired", StringComparison.OrdinalIgnoreCase);

				// Zoom returns "Invalid access token" in two scenarios:
				//  - when the developer provides an invalid token. We don't want to retry the request in this situation.
				//  - when the token has been invalidated because another process has requested a new one. We want to retry the request after retrieving the refreshed token.
				// Unfortunately, I can't think of a way to distinguish between the two scenarios bcause they share the exact same error message.
				shouldRetry |= message.StartsWith("Invalid access token", StringComparison.OrdinalIgnoreCase);

				if (shouldRetry)
				{
					var refreshedTokenInfo = await _tokenHandler.GetTokenInfoAsync(true, tokenInfo, request.CancellationToken).ConfigureAwait(false);
					response = await _defaultRetryCoordinator.ExecuteAsync(request.WithBearerAuthentication(refreshedTokenInfo.AccessToken), dispatcher);
					_previousTokenInfo = refreshedTokenInfo;
					return response;
				}
			}

			return response;
		}
	}
}
