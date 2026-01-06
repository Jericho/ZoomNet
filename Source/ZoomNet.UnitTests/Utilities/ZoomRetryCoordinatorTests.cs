using Pathoschild.Http.Client.Retry;
using Shouldly;
using System;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class ZoomRetryCoordinatorTests
	{
		[Fact]
		public void Constructor_WithTokenHandler_SetsUpCoordinator()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);

			// Act
			var coordinator = new ZoomRetryCoordinator(tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithRetryStrategyAndTokenHandler_SetsUpCoordinator()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategy = new Http429RetryStrategy();

			// Act
			var coordinator = new ZoomRetryCoordinator(retryStrategy, tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithMultipleRetryStrategiesAndTokenHandler_SetsUpCoordinator()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategy1 = new Http429RetryStrategy(maxAttempts: 3);
			var retryStrategy2 = new Http429RetryStrategy(maxAttempts: 5);
			var retryStrategies = new IRetryConfig[] { retryStrategy1, retryStrategy2 };

			// Act
			var coordinator = new ZoomRetryCoordinator(retryStrategies, tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullTokenHandler_ThrowsArgumentNullException()
		{
			// Act & Assert
			Should.Throw<ArgumentNullException>(() => new ZoomRetryCoordinator(tokenHandler: null));
		}

		[Fact]
		public void Constructor_WithRetryStrategyAndNullTokenHandler_ThrowsArgumentNullException()
		{
			// Arrange
			var retryStrategy = new Http429RetryStrategy();

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => new ZoomRetryCoordinator(retryStrategy, null));
		}

		[Fact]
		public void Constructor_WithMultipleRetryStrategiesAndNullTokenHandler_ThrowsArgumentNullException()
		{
			// Arrange
			var retryStrategy1 = new Http429RetryStrategy(maxAttempts: 3);
			var retryStrategy2 = new Http429RetryStrategy(maxAttempts: 5);
			var retryStrategies = new IRetryConfig[] { retryStrategy1, retryStrategy2 };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => new ZoomRetryCoordinator(retryStrategies, null));
		}

		[Fact]
		public void Constructor_WithDifferentMaxAttempts_CreatesCoordinatorSuccessfully()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategy = new Http429RetryStrategy(maxAttempts: 10);

			// Act
			var coordinator = new ZoomRetryCoordinator(retryStrategy, tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithZeroMaxAttempts_CreatesCoordinatorSuccessfully()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategy = new Http429RetryStrategy(maxAttempts: 0);

			// Act
			var coordinator = new ZoomRetryCoordinator(retryStrategy, tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithSingleRetryStrategyInArray_CreatesCoordinatorSuccessfully()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategies = new IRetryConfig[] { new Http429RetryStrategy() };

			// Act
			var coordinator = new ZoomRetryCoordinator(retryStrategies, tokenHandler);

			// Assert
			coordinator.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithEmptyRetryStrategiesArray_ThrowsArgumentException()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("test_client_id", "test_secret", "test_account");
			var tokenHandler = new OAuthTokenHandler(connectionInfo, new System.Net.Http.HttpClient(), null);
			var retryStrategies = new IRetryConfig[0];

			// Act
			Should.Throw<ArgumentException>(() => new ZoomRetryCoordinator(retryStrategies, tokenHandler));
		}
	}
}
