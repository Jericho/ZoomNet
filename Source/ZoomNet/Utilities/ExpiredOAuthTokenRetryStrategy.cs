using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client.Retry;
using System;
using System.Net;
using System.Net.Http;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Implements IRetryConfig to renew an oauth token when it has expired.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Retry.IRetryConfig" />
	internal class ExpiredOAuthTokenRetryStrategy : IRetryConfig
	{
		#region FIELDS

		private readonly ITokenHandler _tokenHandler;

		#endregion

		#region PROPERTIES

		/// <summary>Gets the maximum number of times to retry a request before failing.</summary>
		public int MaxRetries { get; }

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpiredOAuthTokenRetryStrategy" /> class.
		/// </summary>
		/// <param name="tokenHandler">The handler that takes care of renewing expired tokens.</param>
		public ExpiredOAuthTokenRetryStrategy(ITokenHandler tokenHandler)
		{
			_tokenHandler = tokenHandler;
		}

		#endregion

		#region PUBLIC METHODS

		/// <summary>
		/// Checks if we should retry an operation.
		/// </summary>
		/// <param name="response">The Http response of the previous request.</param>
		/// <returns>
		///   <c>true</c> if another attempt should be made; otherwise, <c>false</c>.
		/// </returns>
		public bool ShouldRetry(HttpResponseMessage response)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized && _tokenHandler != null)
			{
				var responseContent = response.Content.ReadAsStringAsync(null).GetAwaiter().GetResult();
				var jObject = JObject.Parse(responseContent);
				var message = jObject.GetPropertyValue("message", string.Empty);
				if (message.StartsWith("access token is expired", StringComparison.OrdinalIgnoreCase))
				{
					_tokenHandler.RefreshTokenIfNecessary(false);

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets a TimeSpan value which defines how long to wait before trying again after an unsuccessful attempt.
		/// </summary>
		/// <param name="attempt">The number of attempts carried out so far. That is, after the first attempt (for
		/// the first retry), attempt will be set to 1, after the second attempt it is set to 2, and so on.</param>
		/// <param name="response">The Http response of the previous request.</param>
		/// <returns>
		/// A TimeSpan value which defines how long to wait before the next attempt.
		/// </returns>
		public TimeSpan GetDelay(int attempt, HttpResponseMessage response)
		{
			// We renewed the OAuth token therefore we don't want to wait.
			// We want to reissue the request immediately
			return TimeSpan.FromMilliseconds(0);
		}

		#endregion
	}
}
