using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Internal;
using Pathoschild.Http.Client.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>A request coordinator which retries failed requests with a delay between each attempt.</summary>
	internal class ZoomRetryCoordinator : IRequestCoordinator
	{
		private readonly ITokenHandler _tokenHandler;
		private readonly List<IRetryConfig> _retryConfigs = new List<IRetryConfig>();
		private readonly HttpStatusCode _timeoutStatusCode = (HttpStatusCode)589;

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
			if (config != null) _retryConfigs.Add(config);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomRetryCoordinator" /> class.
		/// </summary>
		/// <param name="config">The retry configuration.</param>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ZoomRetryCoordinator(IEnumerable<IRetryConfig> config, ITokenHandler tokenHandler)
			: this(tokenHandler)
		{
			if (config != null) _retryConfigs.AddRange(config.Where(c => c != null));
		}

		/// <summary>Dispatch an HTTP request.</summary>
		/// <param name="request">The response message to validate.</param>
		/// <param name="dispatcher">A method which executes the request.</param>
		/// <returns>The final HTTP response.</returns>
		public async Task<HttpResponseMessage> ExecuteAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
		{
			int attempt = 0;
			while (true)
			{
				// dispatch request
				attempt++;
				var response = await DispatchRequest(request, dispatcher).ConfigureAwait(false);

				// check if the token needs to be refreshed
				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					var responseContent = await response.Content.ReadAsStringAsync(null).ConfigureAwait(false);
					var jObject = JObject.Parse(responseContent);
					var message = jObject.GetPropertyValue<string>("message");
					if (message.StartsWith("access token is expired", StringComparison.OrdinalIgnoreCase))
					{
						var refreshedToken = _tokenHandler.RefreshTokenIfNecessary(true);
						request = request.WithBearerAuthentication(refreshedToken);
						response = await DispatchRequest(request, dispatcher);
					}
				}

				// find the applicable retry configuration, if any
				IRetryConfig retryConfig = RetryConfig.None();
				bool shouldRetry = false;
				foreach (var config in _retryConfigs)
				{
					if (config.ShouldRetry(response))
					{
						retryConfig = config;
						shouldRetry = true;
						break;
					}
				}

				// exit if there is no need to retry the request
				if (!shouldRetry)
					return response;

				int maxAttempt = 1 + retryConfig?.MaxRetries ?? 0;

				// throw an exception if we have reached the maximum number of retries
				if (attempt > maxAttempt)
					throw new ApiException(new Response(response, request.Formatters), $"The HTTP request failed, and the retry coordinator gave up after the maximum {maxAttempt} retries");

				// set up retry
				TimeSpan delay = retryConfig.GetDelay(attempt, response);
				if (delay.TotalMilliseconds > 0)
					await Task.Delay(delay).ConfigureAwait(false);
			}
		}

		private async Task<HttpResponseMessage> DispatchRequest(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher)
		{
			try
			{
				return await dispatcher(request).ConfigureAwait(false);
			}
			catch (TaskCanceledException) when (!request.CancellationToken.IsCancellationRequested)
			{
				return request.Message.CreateResponse(_timeoutStatusCode);
			}
		}
	}
}
