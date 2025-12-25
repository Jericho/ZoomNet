using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models.Webhooks;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class DataComplianceTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public DataComplianceTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region NotifyAsync Tests

		[Fact]
		public async Task NotifyAsync_WithValidParameters_Succeeds()
		{
			// Arrange
			var userId = "user123";
			var accountId = 123456789L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "signature123",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "client123"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithBasicAuthenticationUsed()
		{
			// Arrange
			var userId = "user456";
			var accountId = 987654321L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "sig456",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "client456"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.With(request =>
				{
					// Verify that Basic authentication header is present
					return request.Headers.Authorization != null &&
						   request.Headers.Authorization.Scheme == "Basic";
				})
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithDifferentAccountId()
		{
			// Arrange
			var userId = "differentUser";
			var accountId = 111222333L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "different_sig",
				DeauthorizationTime = new DateTime(2023, 12, 1, 10, 0, 0, DateTimeKind.Utc),
				ClientId = "different_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithComplexUserId()
		{
			// Arrange
			var userId = "complex_user_id_with_special_chars_123";
			var accountId = 999888777L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "complex_signature",
				DeauthorizationTime = DateTime.UtcNow.AddDays(-1),
				ClientId = "complex_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithRecentDeauthorizationTime()
		{
			// Arrange
			var userId = "recentUser";
			var accountId = 555666777L;
			var recentTime = DateTime.UtcNow.AddMinutes(-5);
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "recent_sig",
				DeauthorizationTime = recentTime,
				ClientId = "recent_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_VerifiesComplianceCompletedIsTrue()
		{
			// Arrange
			var userId = "complianceUser";
			var accountId = 444555666L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "compliance_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "compliance_client"
			};

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp // Issue a new token
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond(HttpStatusCode.OK, "application/json", "{\"refresh_token\":\"new refresh token\",\"access_token\":\"new access token\"}");

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.With(request =>
				{
					// Verify the request contains compliance_completed: true
					var content = request.Content.ReadAsStringAsync().Result;
					return content.Contains("\"compliance_completed\":\"true\"") ||
						   content.Contains("\"compliance_completed\": \"true\"");
				})
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, mockTokenHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_VerifiesRequestPayloadStructure()
		{
			// Arrange
			var userId = "payloadUser";
			var accountId = 777888999L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "payload_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "payload_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.With(request =>
				{
					// Verify the request payload contains expected fields
					var content = request.Content.ReadAsStringAsync().Result;
					return content.Contains("client_id") &&
						   content.Contains("user_id") &&
						   content.Contains("account_id") &&
						   content.Contains("deauthorization_event_received") &&
						   content.Contains("compliance_completed");
				})
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithMinimalEventData()
		{
			// Arrange
			var userId = "minimalUser";
			var accountId = 123L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "min",
				DeauthorizationTime = DateTime.MinValue,
				ClientId = "min"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithLargeAccountId()
		{
			// Arrange
			var userId = "largeAccountUser";
			var accountId = 999999999999999L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "large_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "large_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithOAuthConnectionInfo()
		{
			// Arrange
			var userId = "oauthUser";
			var accountId = 333444555L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "oauth_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "oauth_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Edge Cases and Error Scenarios

		[Fact]
		public async Task NotifyAsync_WithEmptyUserId()
		{
			// Arrange
			var userId = "";
			var accountId = 123456L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "empty_user_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "empty_user_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithZeroAccountId()
		{
			// Arrange
			var userId = "zeroAccountUser";
			var accountId = 0L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "zero_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "zero_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task NotifyAsync_WithSpecialCharactersInUserId()
		{
			// Arrange
			var userId = "user@example.com+special_chars_123";
			var accountId = 654321L;
			var deauthorizationEvent = new AppDeauthorizedEvent
			{
				AccountId = accountId.ToString(),
				UserId = userId,
				Signature = "special_sig",
				DeauthorizationTime = DateTime.UtcNow,
				ClientId = "special_client"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("oauth", "data", "compliance"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
#pragma warning disable CS0618 // Type or member is obsolete
			var dataCompliance = new DataCompliance(client);
#pragma warning restore CS0618 // Type or member is obsolete

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await dataCompliance.NotifyAsync(userId, accountId, deauthorizationEvent, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
