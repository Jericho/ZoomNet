using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class Http429RetryStrategyTests
	{
		private const HttpStatusCode TOO_MANY_REQUESTS = (HttpStatusCode)429;

		#region Constructor Tests

		[Fact]
		public void Constructor_Default_SetsMaxRetriesToDefault()
		{
			// Arrange & Act
			var strategy = new Http429RetryStrategy();

			// Assert
			strategy.MaxRetries.ShouldBe(4);
		}

		[Fact]
		public void Constructor_WithMaxAttempts_SetsMaxRetries()
		{
			// Arrange
			var maxAttempts = 10;

			// Act
			var strategy = new Http429RetryStrategy(maxAttempts);

			// Assert
			strategy.MaxRetries.ShouldBe(maxAttempts);
		}

		[Fact]
		public void Constructor_WithZeroMaxAttempts_SetsMaxRetriesToZero()
		{
			// Arrange
			var maxAttempts = 0;

			// Act
			var strategy = new Http429RetryStrategy(maxAttempts);

			// Assert
			strategy.MaxRetries.ShouldBe(0);
		}

		[Fact]
		public void Constructor_WithNegativeMaxAttempts_SetsMaxRetriesToNegative()
		{
			// Arrange
			var maxAttempts = -1;

			// Act
			var strategy = new Http429RetryStrategy(maxAttempts);

			// Assert
			strategy.MaxRetries.ShouldBe(-1);
		}

		[Fact]
		public void Constructor_WithCustomMaxAttemptsAndMockClock_WorksCorrectly()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var maxAttempts = 7;

			// Act
			var strategy = new Http429RetryStrategy(maxAttempts, mockClock);

			// Assert
			strategy.MaxRetries.ShouldBe(7);
		}

		#endregion

		#region ShouldRetry Tests

		[Fact]
		public void ShouldRetry_WithNullResponse_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();

			// Act
			var result = strategy.ShouldRetry(null);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_WithNon429StatusCode_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(HttpStatusCode.OK);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With404StatusCode_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(HttpStatusCode.NotFound);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With500StatusCode_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndRetryAfterWithin15Seconds_ReturnsTrue()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(10).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void ShouldRetry_With429AndRetryAfterExactly15Seconds_ReturnsFalse()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(15).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndRetryAfterMoreThan15Seconds_ReturnsFalse()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(30).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndQPSType_ReturnsTrue()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "QPS");

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void ShouldRetry_With429AndQpsTypeLowercase_ReturnsTrue()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "qps");

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void ShouldRetry_With429AndNoHeaders_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndInvalidRetryAfterHeader_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.TryAddWithoutValidation("Retry-After", "invalid-date");

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndDailyRateLimitType_ReturnsFalse()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "Daily");

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void ShouldRetry_With429AndRetryAfterInPast_ReturnsTrue()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(-10).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void ShouldRetry_WithAllRateLimitHeaders_ProcessesCorrectly()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Category", "ApiCall");
			response.Headers.Add("X-RateLimit-Type", "QPS");
			response.Headers.Add("X-RateLimit-Limit", "10");
			response.Headers.Add("X-RateLimit-Remaining", "0");
			response.Headers.Add("Retry-After", mockClock.UtcNow.AddSeconds(5).ToString("R"));

			// Act
			var result = strategy.ShouldRetry(response);

			// Assert
			result.ShouldBeTrue();
		}

		#endregion

		#region GetDelay Tests

		[Fact]
		public void GetDelay_WithNullResponse_ReturnsDefaultDelay()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();

			// Act
			var delay = strategy.GetDelay(1, null);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void GetDelay_WithValidRetryAfterHeader_ReturnsCalculatedDelay()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(3).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(3));
		}

		[Fact]
		public void GetDelay_WithQPSRateLimitType_ReturnsDelayBetween1And2Seconds()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "QPS");

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1000);
			delay.TotalMilliseconds.ShouldBeLessThanOrEqualTo(2000);
		}

		[Fact]
		public void GetDelay_WithQPSRateLimitTypeLowercase_ReturnsDelayBetween1And2Seconds()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "qps");

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1000);
			delay.TotalMilliseconds.ShouldBeLessThanOrEqualTo(2000);
		}

		[Fact]
		public void GetDelay_WithInvalidRetryAfterHeader_ReturnsDefaultDelay()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.TryAddWithoutValidation("Retry-After", "not-a-date");

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void GetDelay_WithNegativeCalculatedDelay_ReturnsDefaultDelay()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(-5).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void GetDelay_WithDelayGreaterThan5Seconds_Returns5Seconds()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(10).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(5));
		}

		[Fact]
		public void GetDelay_WithDelayExactly5Seconds_Returns5Seconds()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(5).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(5));
		}

		[Fact]
		public void GetDelay_WithNoHeaders_ReturnsDefaultDelay()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void GetDelay_WithDifferentAttemptNumbers_ReturnsConsistentDelay()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(2).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay1 = strategy.GetDelay(1, response);
			var delay2 = strategy.GetDelay(2, response);
			var delay3 = strategy.GetDelay(3, response);

			// Assert
			delay1.ShouldBe(TimeSpan.FromSeconds(2));
			delay2.ShouldBe(TimeSpan.FromSeconds(2));
			delay3.ShouldBe(TimeSpan.FromSeconds(2));
		}

		[Fact]
		public void GetDelay_WithSmallRetryAfterDelay_ReturnsCalculatedDelay()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			var retryAfter = mockClock.UtcNow.AddSeconds(5).ToString("R");
			response.Headers.Add("Retry-After", retryAfter);

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.ShouldBe(TimeSpan.FromSeconds(5));
		}

		[Fact]
		public void GetDelay_WithAllRateLimitHeaders_UsesRetryAfter()
		{
			// Arrange
			var mockClock = new MockSystemClock(2023, 12, 1, 10, 0, 0, 0);
			var strategy = new Http429RetryStrategy(4, mockClock);
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Category", "ApiCall");
			response.Headers.Add("X-RateLimit-Type", "QPS");
			response.Headers.Add("X-RateLimit-Limit", "10");
			response.Headers.Add("X-RateLimit-Remaining", "0");
			response.Headers.Add("Retry-After", mockClock.UtcNow.AddSeconds(4).ToString("R"));

			// Act
			var delay = strategy.GetDelay(1, response);

			// Assert
			delay.TotalSeconds.ShouldBeGreaterThanOrEqualTo(3.9);
			delay.TotalSeconds.ShouldBeLessThanOrEqualTo(4.1);
		}

		[Fact]
		public void GetDelay_WithOnlyQPSTypeHeader_ReturnsRandomizedDelay()
		{
			// Arrange
			var strategy = new Http429RetryStrategy();
			var response = new HttpResponseMessage(TOO_MANY_REQUESTS);
			response.Headers.Add("X-RateLimit-Type", "QPS");

			// Act - Test multiple times to ensure randomness
			var delay1 = strategy.GetDelay(1, response);
			var delay2 = strategy.GetDelay(1, response);
			var delay3 = strategy.GetDelay(1, response);

			// Assert - All should be between 1 and 2 seconds
			delay1.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1000);
			delay1.TotalMilliseconds.ShouldBeLessThanOrEqualTo(2000);
			delay2.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1000);
			delay2.TotalMilliseconds.ShouldBeLessThanOrEqualTo(2000);
			delay3.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1000);
			delay3.TotalMilliseconds.ShouldBeLessThanOrEqualTo(2000);
		}

		#endregion
	}
}
