using Pathoschild.Http.Client.Retry;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Implements IRetryConfig with back off based on a wait time derived from the
	/// "Retry-After" response header. The value in this header contains the date
	/// and time when the next attempt can take place.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Retry.IRetryConfig" />
	internal class Http429RetryStrategy : IRetryConfig
	{
		#region FIELDS

		private const int DEFAULT_MAX_RETRIES = 4;
		private const HttpStatusCode TOO_MANY_REQUESTS = (HttpStatusCode)429;
		private static readonly TimeSpan DEFAULT_DELAY = TimeSpan.FromSeconds(1);

		private readonly ISystemClock _systemClock;

		#endregion

		#region PROPERTIES

		/// <summary>Gets the maximum number of times to retry a request before failing.</summary>
		public int MaxRetries { get; }

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="Http429RetryStrategy" /> class.
		/// </summary>
		public Http429RetryStrategy()
			: this(DEFAULT_MAX_RETRIES, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Http429RetryStrategy" /> class.
		/// </summary>
		/// <param name="maxAttempts">The maximum attempts.</param>
		public Http429RetryStrategy(int maxAttempts)
			: this(maxAttempts, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Http429RetryStrategy" /> class.
		/// </summary>
		/// <param name="maxAttempts">The maximum attempts.</param>
		/// <param name="systemClock">The system clock. This is for unit testing only.</param>
		internal Http429RetryStrategy(int maxAttempts, ISystemClock systemClock)
		{
			MaxRetries = maxAttempts;
			_systemClock = systemClock ?? SystemClock.Instance;
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
			if (response == null) return false;
			if (response.StatusCode != TOO_MANY_REQUESTS) return false;

			var rateLimitInfo = GetRateLimitInformation(response?.Headers);

			// There's no need to retry when the reset time is too far in the future.
			// I arbitrarily decided that 15 seconds is the cutoff.
			var delay = CalculateDelay(_systemClock, rateLimitInfo.RetryAfter, DEFAULT_DELAY);
			if (delay.TotalSeconds > 15) return false;

			return true;
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
			// Default value in case the HTTP headers don't provide enough information
			var waitTime = DEFAULT_DELAY;

			// Figure out how long to wait based on the presence of Zoom specific headers as discussed here:
			// https://marketplace.zoom.us/docs/api-reference/rate-limits#best-practices-for-handling-errors
			var rateLimitInfo = GetRateLimitInformation(response?.Headers);

			if (!string.IsNullOrEmpty(rateLimitInfo.RetryAfter))
			{
				waitTime = CalculateDelay(_systemClock, rateLimitInfo.RetryAfter, DEFAULT_DELAY);
			}
			else if ((rateLimitInfo.Type ?? string.Empty).Equals("QPS", StringComparison.OrdinalIgnoreCase))
			{
				// QPS stands for "Query Per Second".
				// It means that we have exceeded the number of API calls per second.
				// Therefore we must wait one second before retrying.
				var waitMilliseconds = 1000;

				// Edit January 2021: I introduced randomness in the wait time to avoid retrying too quickly
				// and to avoid requests issued in a tight loop to all be retried at the same time.
				// Up to one extra second: 250 milliseconds * 4.
				for (int i = 0; i < 4; i++)
				{
					waitMilliseconds += RandomGenerator.RollDice(250);
				}

				waitTime = TimeSpan.FromMilliseconds(waitMilliseconds);
			}

			// Make sure the wait time is valid
			if (waitTime.TotalMilliseconds < 0) waitTime = DEFAULT_DELAY;

			// Totally arbitrary. Make sure we don't wait more than a 'reasonable' amount of time
			if (waitTime.TotalSeconds > 5) waitTime = TimeSpan.FromSeconds(5);

			return waitTime;
		}

		#endregion

		#region PRIVATE METHODS

		private static (string Category, string Type, string Limit, string Remaining, string RetryAfter) GetRateLimitInformation(HttpResponseHeaders headers)
		{
			var category = headers?.GetValue("X-RateLimit-Category");
			var type = headers?.GetValue("X-RateLimit-Type");
			var limit = headers?.GetValue("X-RateLimit-Limit");
			var remaining = headers?.GetValue("X-RateLimit-Remaining");
			var retryAfter = headers?.GetValue("Retry-After");

			return (category, type, limit, remaining, retryAfter);
		}

		private static TimeSpan CalculateDelay(ISystemClock clock, string retryAfter, TimeSpan defaultDelay)
		{
			if (DateTime.TryParse(retryAfter, out DateTime nextRetryDateTime))
			{
				return nextRetryDateTime.Subtract(clock.UtcNow);
			}

			return defaultDelay;
		}

		#endregion
	}
}
