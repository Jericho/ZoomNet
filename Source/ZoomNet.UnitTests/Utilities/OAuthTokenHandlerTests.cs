using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class OAuthTokenHandlerTests
	{
		/// <summary>
		/// This unit test demonstrates that an exception thrown in the OAuthTokenHandler
		/// when attempting to refresh tokens does not prevent us from retrying. 
		/// </summary>
		/// <remarks>
		/// This unit test was used to reproduce the problem described in this
		/// <a href="https://github.com/Jericho/ZoomNet/issues/109">Github issue</a>
		/// and to subsequently demonstrate that the bug was fixed.
		/// </remarks>
		[Fact]
		public void Attempt_to_refresh_token_multiple_times_despite_exception()
		{
			// Arrange
			var clientId = "abc123";
			var clientSecret = "xyz789";
			var authorizationCode = "INVALID_AUTH_CODE";
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode,
				(newRefreshToken, newAccessToken) =>
				{
					// Intentionally left blank
				},
				null);
			var apiResponse = "{ \"reason\":\"Invalid authorization code " + authorizationCode + "\",\"error\":\"invalid_request\"}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, $"https://api.zoom.us/oauth/token")
				.Respond(HttpStatusCode.BadRequest, "application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			Should.Throw<ZoomException>(() => handler.RefreshTokenIfNecessary(true));
			Should.Throw<ZoomException>(() => handler.RefreshTokenIfNecessary(true));

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public void Constructor_WithNullConnectionInfo_ThrowsArgumentNullException()
		{
			// Arrange
			var httpClient = new HttpClient();

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => new OAuthTokenHandler(null, httpClient, null));
		}

		[Fact]
		public void Constructor_WithValidParameters_CreatesHandler()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var httpClient = new HttpClient();

			// Act
			var handler = new OAuthTokenHandler(connectionInfo, httpClient, null);

			// Assert
			handler.ShouldNotBeNull();
			handler.ConnectionInfo.ShouldBe(connectionInfo);
		}

		[Fact]
		public void Constructor_WithCustomClockSkew_CreatesHandler()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var httpClient = new HttpClient();
			var clockSkew = TimeSpan.FromMinutes(10);

			// Act
			var handler = new OAuthTokenHandler(connectionInfo, httpClient, clockSkew);

			// Assert
			handler.ShouldNotBeNull();
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithServerToServerCredentials_RefreshesToken()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			var token = handler.RefreshTokenIfNecessary(true);

			// Assert
			token.ShouldBe(accessToken);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithAuthorizationCode_RefreshesToken()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var authorizationCode = "auth_code_123";
			var accessToken = "new_access_token_12345";
			var refreshToken = "new_refresh_token_67890";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(
				clientId,
				clientSecret,
				authorizationCode,
				(newRefreshToken, newAccessToken) => { },
				null);

			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				refresh_token = refreshToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			var token = handler.RefreshTokenIfNecessary(true);

			// Assert
			token.ShouldBe(accessToken);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			connectionInfo.RefreshToken.ShouldBe(refreshToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithRefreshToken_RefreshesToken()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var oldRefreshToken = "old_refresh_token";
			var accessToken = "new_access_token_12345";
			var newRefreshToken = "new_refresh_token_67890";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.WithRefreshToken(
				clientId,
				clientSecret,
				oldRefreshToken,
				(refreshToken, newAccessToken) => { });

			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				refresh_token = newRefreshToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			var token = handler.RefreshTokenIfNecessary(true);

			// Assert
			token.ShouldBe(accessToken);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			connectionInfo.RefreshToken.ShouldBe(newRefreshToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithEmptyResponse_ThrowsException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond(HttpStatusCode.OK, "application/json", string.Empty);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act & Assert
			Should.Throw<Exception>(() => handler.RefreshTokenIfNecessary(true));
		}

		[Fact]
		public void RefreshTokenIfNecessary_WhenAlreadyRefreshed_DoesNotRefreshAgain()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act - First refresh
			handler.RefreshTokenIfNecessary(true);

			// Act - Second call without force refresh and token not expired
			var token = handler.RefreshTokenIfNecessary(false);

			// Assert - Only one request should have been made
			token.ShouldBe(accessToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void Token_Property_RefreshesTokenIfNecessary()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			var token = handler.Token;

			// Assert
			token.ShouldBe(accessToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void RefreshTokenIfNecessary_UpdatesScopes()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 3600;
			var scopes = "meeting:write user:read recording:read";

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = scopes
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			handler.RefreshTokenIfNecessary(true);

			// Assert
			connectionInfo.Scopes.ShouldNotBeNull();
			connectionInfo.Scopes.Count.ShouldBe(3);
			connectionInfo.Scopes.ShouldContain("meeting:write");
			connectionInfo.Scopes.ShouldContain("user:read");
			connectionInfo.Scopes.ShouldContain("recording:read");
		}

		[Fact]
		public void RefreshTokenIfNecessary_InvokesCallback()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var authorizationCode = "auth_code_123";
			var accessToken = "new_access_token_12345";
			var refreshToken = "new_refresh_token_67890";
			var expiresIn = 3600;

			string callbackRefreshToken = null;
			string callbackAccessToken = null;

			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(
				clientId,
				clientSecret,
				authorizationCode,
				(newRefreshToken, newAccessToken) =>
				{
					callbackRefreshToken = newRefreshToken;
					callbackAccessToken = newAccessToken;
				},
				null);

			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				refresh_token = refreshToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			handler.RefreshTokenIfNecessary(true);

			// Assert
			callbackRefreshToken.ShouldBe(refreshToken);
			callbackAccessToken.ShouldBe(accessToken);
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithAuthorizationCodeAndRedirectUri_IncludesRedirectUri()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var authorizationCode = "auth_code_123";
			var redirectUri = "https://example.com/callback";
			var accessToken = "new_access_token_12345";
			var refreshToken = "new_refresh_token_67890";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(
				clientId,
				clientSecret,
				authorizationCode,
				(newRefreshToken, newAccessToken) => { },
				redirectUri);

			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				refresh_token = refreshToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.WithFormData("redirect_uri", redirectUri)
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			handler.RefreshTokenIfNecessary(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
		}

		[Fact]
		public void RefreshTokenIfNecessary_ServerToServerDoesNotChangeGrantType()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);
			var originalGrantType = connectionInfo.GrantType;

			// Act
			handler.RefreshTokenIfNecessary(true);

			// Assert
			connectionInfo.GrantType.ShouldBe(originalGrantType);
		}

		[Fact]
		public void RefreshTokenIfNecessary_AuthorizationCodeChangesToRefreshToken()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var authorizationCode = "auth_code_123";
			var accessToken = "new_access_token_12345";
			var refreshToken = "new_refresh_token_67890";
			var expiresIn = 3600;

			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(
				clientId,
				clientSecret,
				authorizationCode,
				(newRefreshToken, newAccessToken) => { },
				null);

			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				refresh_token = refreshToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			handler.RefreshTokenIfNecessary(true);

			// Assert
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.RefreshToken);
		}

		[Fact]
		public void RefreshTokenIfNecessary_WithClockSkew_ConsidersClockSkewForExpiration()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_secret";
			var accountId = "test_account";
			var accessToken = "new_access_token_12345";
			var expiresIn = 300; // 5 minutes
			var clockSkew = TimeSpan.FromMinutes(10); // 10 minute clock skew

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
			var apiResponse = JsonSerializer.Serialize(new
			{
				access_token = accessToken,
				token_type = "bearer",
				expires_in = expiresIn,
				scope = "scope1 scope2"
			});

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), clockSkew);

			// Act
			handler.RefreshTokenIfNecessary(true);

			// The token should be considered expired immediately because clockSkew (10 min) > expiresIn (5 min)
			// So accessing Token property should trigger another refresh
			mockHttp
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", apiResponse);

			var token = handler.Token;

			// Assert
			token.ShouldBe(accessToken);
			mockHttp.VerifyNoOutstandingExpectation();
		}
	}
}
