using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests
{
	public class ZoomClientTests
	{
		private const string CLIENT_ID = "my_client_id";
		private const string CLIENT_SECRET = "my_client_secret";
		private const string ACCOUNT_ID = "my_account_id";
		private const string ACCESS_TOKEN = null;

		private readonly ITestOutputHelper _outputHelper;

		public ZoomClientTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public void Version_is_not_empty()
		{
			// Arrange

			// Act
			var result = ZoomClient.Version;

			// Assert
			result.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void Dispose()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null, logger: logger);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}

		[Fact]
		public void Dispose_CanBeCalledMultipleTimes()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null, logger: logger);

			// Act
			client.Dispose();
			client.Dispose(); // Should not throw

			// Assert
			// Nothing to assert. We just want to confirm that multiple 'Dispose' calls did not throw any exception
		}

		[Fact]
		public void Throws_if_connectioninfo_is_null()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = (IConnectionInfo)null;

			// Act
			Should.Throw<ArgumentNullException>(() => new ZoomClient(connectionInfo, logger: logger));
		}

		[Fact]
		public void Throws_if_httpclient_is_null()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var httpClient = (HttpClient)null;

			// Act
			Should.Throw<ArgumentNullException>(() => new ZoomClient(connectionInfo, httpClient, logger: logger));
		}

		[Fact]
		public void Throws_if_unknown_connection_type()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = new MockConnectionInfo();

			// Act
			Should.Throw<ZoomException>(() => new ZoomClient(connectionInfo, logger: logger));
		}

		[Fact]
		public void Constructor_WithConnectionInfo_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);

			// Act
			using var client = new ZoomClient(connectionInfo, logger: logger);

			// Assert
			client.ShouldNotBeNull();
			client.Accounts.ShouldNotBeNull();
			client.Billing.ShouldNotBeNull();
			client.CallLogs.ShouldNotBeNull();
			client.Chat.ShouldNotBeNull();
			client.Chatbot.ShouldNotBeNull();
			client.CloudRecordings.ShouldNotBeNull();
			client.Contacts.ShouldNotBeNull();
			client.Dashboards.ShouldNotBeNull();
			client.Events.ShouldNotBeNull();
			client.ExternalContacts.ShouldNotBeNull();
			client.Groups.ShouldNotBeNull();
			client.Meetings.ShouldNotBeNull();
			client.PastMeetings.ShouldNotBeNull();
			client.PastWebinars.ShouldNotBeNull();
			client.Phone.ShouldNotBeNull();
			client.Reports.ShouldNotBeNull();
			client.Roles.ShouldNotBeNull();
			client.Rooms.ShouldNotBeNull();
			client.Sms.ShouldNotBeNull();
			client.TrackingFields.ShouldNotBeNull();
			client.Users.ShouldNotBeNull();
			client.Webinars.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithProxy_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var proxy = new WebProxy("http://proxy.example.com:8080");

			// Act
			using var client = new ZoomClient(connectionInfo, proxy, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullProxy_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);

			// Act
			using var client = new ZoomClient(connectionInfo, (IWebProxy)null, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithHttpMessageHandler_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var handler = new MockHttpMessageHandler();

			// Act
			using var client = new ZoomClient(connectionInfo, handler, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithHttpClient_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var httpClient = new HttpClient();

			// Act
			using var client = new ZoomClient(connectionInfo, httpClient, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithOptions_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = new Uri("https://api.zoom.us/v2")
			};

			// Act
			using var client = new ZoomClient(connectionInfo, options, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullOptions_UsesDefaultOptions()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);

			// Act
			using var client = new ZoomClient(connectionInfo, (ZoomClientOptions)null, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullLogger_CreatesClient()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);

			// Act
			using var client = new ZoomClient(connectionInfo, logger: null);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_ThrowsIfApiBaseUrlIsNull()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = null
			};

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => new ZoomClient(connectionInfo, options, logger));
		}

		[Fact]
		public void HasPermissions_WithMatchingScopes_ReturnsTrue()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read recording:read"
			});

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			using var client = new ZoomClient(connectionInfo, mockTokenHttp.ToHttpClient(), logger: logger);

			// Act
			var result = client.HasPermissions(new[] { "meeting:write", "user:read" });

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void HasPermissions_WithMissingScopes_ReturnsFalse()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read"
			});

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			using var client = new ZoomClient(connectionInfo, mockTokenHttp.ToHttpClient(), logger: logger);

			// Act
			var result = client.HasPermissions(new[] { "meeting:write", "user:read", "dashboard:read" });

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void HasPermissions_WithAllMatchingScopes_ReturnsTrue()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read recording:read dashboard:read"
			});

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			using var client = new ZoomClient(connectionInfo, mockTokenHttp.ToHttpClient(), logger: logger);

			// Act
			var result = client.HasPermissions(new[] { "meeting:write", "user:read", "recording:read", "dashboard:read" });

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void HasPermissions_WithEmptyScopes_ReturnsTrue()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read"
			});

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			using var client = new ZoomClient(connectionInfo, mockTokenHttp.ToHttpClient(), logger: logger);

			// Act
			var result = client.HasPermissions(Enumerable.Empty<string>());

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void Constructor_WithCustomBaseUrl_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = new Uri("https://custom-api.zoom.us/v2")
			};

			// Act
			using var client = new ZoomClient(connectionInfo, options, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithProxyAndOptions_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var proxy = new WebProxy("http://proxy.example.com:8080");
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = new Uri("https://api.zoom.us/v2")
			};

			// Act
			using var client = new ZoomClient(connectionInfo, proxy, options, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithHttpMessageHandlerAndOptions_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var handler = new MockHttpMessageHandler();
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = new Uri("https://api.zoom.us/v2")
			};

			// Act
			using var client = new ZoomClient(connectionInfo, handler, options, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithHttpClientAndOptions_CreatesClient()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var httpClient = new HttpClient();
			var options = new ZoomClientOptions
			{
				ApiBaseUrl = new Uri("https://api.zoom.us/v2")
			};

			// Act
			using var client = new ZoomClient(connectionInfo, httpClient, options, logger: logger);

			// Assert
			client.ShouldNotBeNull();
		}

		[Fact]
		public void AllResourceProperties_AreInitialized()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);

			// Act
			using var client = new ZoomClient(connectionInfo, logger: logger);

			// Assert
			client.Accounts.ShouldNotBeNull();
			client.Billing.ShouldNotBeNull();
			client.CallLogs.ShouldNotBeNull();
			client.Chat.ShouldNotBeNull();
			client.Chatbot.ShouldNotBeNull();
			client.CloudRecordings.ShouldNotBeNull();
			client.Contacts.ShouldNotBeNull();
			client.Dashboards.ShouldNotBeNull();
			client.Events.ShouldNotBeNull();
			client.ExternalContacts.ShouldNotBeNull();
			client.Groups.ShouldNotBeNull();
			client.Meetings.ShouldNotBeNull();
			client.PastMeetings.ShouldNotBeNull();
			client.PastWebinars.ShouldNotBeNull();
			client.Phone.ShouldNotBeNull();
			client.Reports.ShouldNotBeNull();
			client.Roles.ShouldNotBeNull();
			client.Rooms.ShouldNotBeNull();
			client.Sms.ShouldNotBeNull();
			client.TrackingFields.ShouldNotBeNull();
			client.Users.ShouldNotBeNull();
			client.Webinars.ShouldNotBeNull();
		}

		[Fact]
		public void Version_IsConsistent()
		{
			// Arrange
			var version1 = ZoomClient.Version;

			// Act
			var version2 = ZoomClient.Version;

			// Assert
			version1.ShouldBe(version2);
		}
	}
}
