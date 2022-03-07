using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>A request coordinator which retries failed requests with a delay between each attempt.</summary>
	internal class ZoomRetryCoordinator : IRequestCoordinator
	{
		private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		private readonly IRequestCoordinator _defaultRetryCoordinator;
		private readonly ITokenHandler _tokenHandler;
		private DateTime _tokenLastRefreshed;

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomRetryCoordinator" /> class.
		/// </summary>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ZoomRetryCoordinator(ITokenHandler tokenHandler)
		{
			_tokenHandler = tokenHandler;
			_tokenLastRefreshed = DateTime.MinValue.ToUniversalTime();
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
			// Dispatch the request
			var response = await _defaultRetryCoordinator.ExecuteAsync(request, dispatcher).ConfigureAwait(false);

			// Check if the token needs to be refreshed
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				var responseContent = await response.Content.ReadAsStringAsync(null).ConfigureAwait(false);
				var jsonResponse = JsonDocument.Parse(responseContent).RootElement;
				var message = jsonResponse.GetPropertyValue("message", string.Empty);
				if (message.StartsWith("access token is expired", StringComparison.OrdinalIgnoreCase))
				{
					var refreshedToken = RefreshToken();
					response = await _defaultRetryCoordinator.ExecuteAsync(request.WithBearerAuthentication(refreshedToken), dispatcher);
				}
			}

			return response;
		}

		private string RefreshToken()
		{
			try
			{
				_lock.EnterUpgradeableReadLock();

				var lastRefreshed = _tokenLastRefreshed;

				try
				{
					_lock.EnterWriteLock();

					var forceRefresh = lastRefreshed == _tokenLastRefreshed;
					var refreshedToken = _tokenHandler.RefreshTokenIfNecessary(forceRefresh);
					_tokenLastRefreshed = DateTime.UtcNow;

					return refreshedToken;
				}
				finally
				{
					if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
				}
			}
			finally
			{
				if (_lock.IsUpgradeableReadLockHeld) _lock.ExitUpgradeableReadLock();
			}
		}
	}
}
