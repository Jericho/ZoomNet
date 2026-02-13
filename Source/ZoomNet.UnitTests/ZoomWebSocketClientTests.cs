using Shouldly;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using WebhookEvent = ZoomNet.Models.Webhooks.Event;

namespace ZoomNet.UnitTests
{
	public class ZoomWebSocketClientTests
	{
		private const string CLIENT_ID = "test_client_id";
		private const string CLIENT_SECRET = "test_client_secret";
		private const string ACCOUNT_ID = "test_account_id";
		private const string SUBSCRIPTION_ID = "test_subscription_id";

		private readonly ITestOutputHelper _outputHelper;

		public ZoomWebSocketClientTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region Constructor Tests

		[Fact]
		public void Constructor_WithValidParameters_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullConnectionInfo_ThrowsArgumentNullException()
		{
			// Arrange
			IConnectionInfo connectionInfo = null;
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			var exception = Should.Throw<ArgumentNullException>(() => new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor));
			exception.ParamName.ShouldBe("connectionInfo");
		}

		[Fact]
		public void Constructor_WithNullSubscriptionId_ThrowsArgumentNullException()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			string subscriptionId = null;
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			var exception = Should.Throw<ArgumentNullException>(() => new ZoomWebSocketClient(connectionInfo, subscriptionId, eventProcessor));
			exception.ParamName.ShouldBe("subscriptionId");
		}

		[Fact]
		public void Constructor_WithNullEventProcessor_ThrowsArgumentNullException()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			Func<WebhookEvent, CancellationToken, Task> eventProcessor = null;

			// Act & Assert
			var exception = Should.Throw<ArgumentNullException>(() => new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor));
			exception.ParamName.ShouldBe("eventProcessor");
		}

		[Fact]
		public void Constructor_WithNonServerToServerOAuth_ThrowsArgumentException()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken("client_id", "client_secret", "refresh_token", "access_token", null);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			var exception = Should.Throw<ArgumentException>(() => new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor));
			exception.Message.ShouldContain("Server-to-Server OAuth");
		}

		[Fact]
		public void Constructor_WithProxy_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var proxy = new WebProxy("http://proxy.example.com:8080");
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, proxy);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithLogger_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var logger = _outputHelper.ToLogger<ZoomWebSocketClient>();
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, null, logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithThrowWhenUnknownEventType_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, throwWhenUnknownEventType: true);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithAllParameters_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var proxy = new WebProxy("http://proxy.example.com:8080");
			var logger = _outputHelper.ToLogger<ZoomWebSocketClient>();
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, throwWhenUnknownEventType: true, proxy, logger);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region Dispose Tests

		[Fact]
		public void Dispose_CalledOnce_DisposesSuccessfully()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;
			var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Act
			client.Dispose();

			// Assert
			// No exception should be thrown
		}

		[Fact]
		public void Dispose_CalledTwice_DoesNotThrow()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;
			var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Act
			client.Dispose();
			client.Dispose(); // Second call should not throw

			// Assert
			// No exception should be thrown
		}

		[Fact]
		public void Dispose_AfterUsingStatement_DisposesAutomatically()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			using (var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor))
			{
				client.ShouldNotBeNull();
			}

			// No exception should be thrown after using block
		}

		#endregion

		#region Parameter Validation Tests

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void Constructor_WithEmptySubscriptionId_ThrowsArgumentNullException(string subscriptionId)
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			Should.Throw<ArgumentException>(() => new ZoomWebSocketClient(connectionInfo, subscriptionId, eventProcessor));
		}

		[Fact]
		public void Constructor_WithValidSubscriptionIdFormat_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var subscriptionId = "sub_12345678901234567890";
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, subscriptionId, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region OAuthConnectionInfo Validation Tests

		[Fact]
		public void Constructor_WithAuthorizationCodeGrantType_ThrowsArgumentException()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken("client_id", "client_secret", "refresh_token", "access_token", null);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act & Assert
			var exception = Should.Throw<ArgumentException>(() => new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor));
			exception.Message.ShouldContain("Server-to-Server OAuth");
		}

		#endregion

		#region Proxy Configuration Tests

		[Fact]
		public void Constructor_WithNullProxy_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, proxy: null);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithProxyCredentials_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var proxy = new WebProxy("http://proxy.example.com:8080")
			{
				Credentials = new NetworkCredential("proxyUser", "proxyPass")
			};
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, proxy);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region Logger Configuration Tests

		[Fact]
		public void Constructor_WithNullLogger_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, logger: null);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithCustomLogger_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var logger = _outputHelper.ToLogger<ZoomWebSocketClient>();
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region Event Processor Edge Cases

		[Fact]
		public void Constructor_WithEventProcessorThatThrows_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => throw new InvalidOperationException("Test exception");

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
			// Note: The exception would only be thrown when an event is actually processed
		}

		[Fact]
		public void Constructor_WithAsyncEventProcessor_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static async Task eventProcessor(WebhookEvent evt, CancellationToken ct)
			{
				await Task.Delay(100, ct);
				// Process event
			}

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region ThrowWhenUnknownEventType Tests

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Constructor_WithThrowWhenUnknownEventTypeFlag_CreatesInstance(bool throwWhenUnknownEventType)
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, throwWhenUnknownEventType: throwWhenUnknownEventType);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region Multiple Instance Tests

		[Fact]
		public void Constructor_CreateMultipleInstances_AllSucceed()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client1 = new ZoomWebSocketClient(connectionInfo, "subscription1", eventProcessor);
			using var client2 = new ZoomWebSocketClient(connectionInfo, "subscription2", eventProcessor);
			using var client3 = new ZoomWebSocketClient(connectionInfo, "subscription3", eventProcessor);

			// Assert
			client1.ShouldNotBeNull();
			client2.ShouldNotBeNull();
			client3.ShouldNotBeNull();
		}

		#endregion

		#region Configuration Combination Tests

		[Fact]
		public void Constructor_WithAllOptionalParametersNull_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, false, null, null);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithAllOptionalParametersSet_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID);
			var proxy = new WebProxy("http://proxy.example.com:8080");
			var logger = _outputHelper.ToLogger<ZoomWebSocketClient>();
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor, true, proxy, logger);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion

		#region Access Token Tests

		[Fact]
		public void Constructor_WithAccessToken_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, "existing_access_token");
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullAccessToken_CreatesInstance()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, null);
			static Task eventProcessor(WebhookEvent evt, CancellationToken ct) => Task.CompletedTask;

			// Act
			using var client = new ZoomWebSocketClient(connectionInfo, SUBSCRIPTION_ID, eventProcessor);

			// Assert
			client.ShouldNotBeNull();
		}

		#endregion
	}
}


