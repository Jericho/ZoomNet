using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;

namespace ZoomNet.UnitTests
{
	public class OAuthConnectionInfoTests
	{
		#region WithClientCredentials Tests

		[Fact]
		public void WithClientCredentials_MinimalParameters_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";

			// Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.ClientCredentials);
			connectionInfo.AccessToken.ShouldBeNull();
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
			connectionInfo.OnTokenRefreshed.ShouldBeNull();
		}

		[Fact]
		public void WithClientCredentials_WithCallback_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.ClientCredentials);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void WithClientCredentials_WithAccessToken_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accessToken = "test_access_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret, accessToken, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MaxValue);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.ClientCredentials);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void WithClientCredentials_NullClientId_ThrowsArgumentNullException()
		{
			// Arrange
			string clientId = null;
			var clientSecret = "test_client_secret";

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret));
		}

		[Fact]
		public void WithClientCredentials_EmptyClientId_ThrowsArgumentException()
		{
			// Arrange
			var clientId = string.Empty;
			var clientSecret = "test_client_secret";

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret));
		}

		[Fact]
		public void WithClientCredentials_NullClientSecret_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			string clientSecret = null;

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret));
		}

		[Fact]
		public void WithClientCredentials_EmptyClientSecret_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = string.Empty;

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret));
		}

		#endregion

		#region WithAuthorizationCode Tests

		[Fact]
		public void WithAuthorizationCode_MinimalParameters_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AuthorizationCode.ShouldBe(authorizationCode);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AuthorizationCode);
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
			connectionInfo.RedirectUri.ShouldBeNull();
			connectionInfo.CodeVerifier.ShouldBeNull();
		}

		[Fact]
		public void WithAuthorizationCode_WithRedirectUri_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";
			var redirectUri = "https://example.com/callback";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback, redirectUri);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AuthorizationCode.ShouldBe(authorizationCode);
			connectionInfo.RedirectUri.ShouldBe(redirectUri);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AuthorizationCode);
		}

		[Fact]
		public void WithAuthorizationCode_WithCodeVerifier_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";
			var redirectUri = "https://example.com/callback";
			var codeVerifier = "test_code_verifier";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback, redirectUri, codeVerifier);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AuthorizationCode.ShouldBe(authorizationCode);
			connectionInfo.RedirectUri.ShouldBe(redirectUri);
			connectionInfo.CodeVerifier.ShouldBe(codeVerifier);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AuthorizationCode);
		}

		[Fact]
		public void WithAuthorizationCode_NullClientId_ThrowsArgumentNullException()
		{
			// Arrange
			string clientId = null;
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		[Fact]
		public void WithAuthorizationCode_EmptyClientId_ThrowsArgumentException()
		{
			// Arrange
			var clientId = string.Empty;
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		[Fact]
		public void WithAuthorizationCode_NullClientSecret_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			string clientSecret = null;
			var authorizationCode = "test_auth_code";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		[Fact]
		public void WithAuthorizationCode_EmptyClientSecret_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = string.Empty;
			var authorizationCode = "test_auth_code";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		[Fact]
		public void WithAuthorizationCode_NullAuthorizationCode_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			string authorizationCode = null;
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		[Fact]
		public void WithAuthorizationCode_EmptyAuthorizationCode_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var authorizationCode = string.Empty;
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, callback));
		}

		#endregion

		#region WithRefreshToken Tests

		[Fact]
		public void WithRefreshToken_MinimalParameters_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.RefreshToken.ShouldBe(refreshToken);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.RefreshToken);
			connectionInfo.AccessToken.ShouldBeNull();
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void WithRefreshToken_WithAccessToken_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";
			var accessToken = "test_access_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, accessToken, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.RefreshToken.ShouldBe(refreshToken);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MaxValue);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.RefreshToken);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void WithRefreshToken_NullClientId_ThrowsArgumentNullException()
		{
			// Arrange
			string clientId = null;
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		[Fact]
		public void WithRefreshToken_EmptyClientId_ThrowsArgumentException()
		{
			// Arrange
			var clientId = string.Empty;
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		[Fact]
		public void WithRefreshToken_NullClientSecret_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			string clientSecret = null;
			var refreshToken = "test_refresh_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		[Fact]
		public void WithRefreshToken_EmptyClientSecret_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = string.Empty;
			var refreshToken = "test_refresh_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		[Fact]
		public void WithRefreshToken_NullRefreshToken_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			string refreshToken = null;
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		[Fact]
		public void WithRefreshToken_EmptyRefreshToken_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var refreshToken = string.Empty;
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, callback));
		}

		#endregion

		#region ForServerToServer Tests

		[Fact]
		public void ForServerToServer_MinimalParameters_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";

			// Act
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AccountId.ShouldBe(accountId);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AccountCredentials);
			connectionInfo.AccessToken.ShouldBeNull();
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
			connectionInfo.OnTokenRefreshed.ShouldBeNull();
		}

		[Fact]
		public void ForServerToServer_WithCallback_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AccountId.ShouldBe(accountId);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AccountCredentials);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void ForServerToServer_WithAccessToken_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";
			var accessToken = "test_access_token";
			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) => { };

			// Act
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, accessToken, callback);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.ClientId.ShouldBe(clientId);
			connectionInfo.ClientSecret.ShouldBe(clientSecret);
			connectionInfo.AccountId.ShouldBe(accountId);
			connectionInfo.AccessToken.ShouldBe(accessToken);
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MaxValue);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.AccountCredentials);
			connectionInfo.OnTokenRefreshed.ShouldBe(callback);
		}

		[Fact]
		public void ForServerToServer_NullClientId_ThrowsArgumentNullException()
		{
			// Arrange
			string clientId = null;
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		[Fact]
		public void ForServerToServer_EmptyClientId_ThrowsArgumentException()
		{
			// Arrange
			var clientId = string.Empty;
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		[Fact]
		public void ForServerToServer_NullClientSecret_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			string clientSecret = null;
			var accountId = "test_account_id";

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		[Fact]
		public void ForServerToServer_EmptyClientSecret_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = string.Empty;
			var accountId = "test_account_id";

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		[Fact]
		public void ForServerToServer_NullAccountId_ThrowsArgumentNullException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			string accountId = null;

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		[Fact]
		public void ForServerToServer_EmptyAccountId_ThrowsArgumentException()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accountId = string.Empty;

			// Act & Assert
			Should.Throw<ArgumentException>(() => OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId));
		}

		#endregion

		#region Property Tests

		[Fact]
		public void Properties_CanBeSetInternally()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");
			var newAccessToken = "new_access_token";
			var newRefreshToken = "new_refresh_token";
			var newScopes = new[] { "scope1", "scope2" };
			var newExpiration = DateTime.UtcNow.AddHours(1);

			// Act
			connectionInfo.AccessToken = newAccessToken;
			connectionInfo.RefreshToken = newRefreshToken;
			connectionInfo.Scopes = newScopes;
			connectionInfo.TokenExpiration = newExpiration;
			connectionInfo.GrantType = OAuthGrantType.RefreshToken;

			// Assert
			connectionInfo.AccessToken.ShouldBe(newAccessToken);
			connectionInfo.RefreshToken.ShouldBe(newRefreshToken);
			connectionInfo.Scopes.ShouldBe(newScopes);
			connectionInfo.TokenExpiration.ShouldBe(newExpiration);
			connectionInfo.GrantType.ShouldBe(OAuthGrantType.RefreshToken);
		}

		[Fact]
		public void Scopes_InitiallyNull()
		{
			// Arrange & Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Assert
			connectionInfo.Scopes.ShouldBeNull();
		}

		[Fact]
		public void AccountId_NullForNonServerToServer()
		{
			// Arrange & Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Assert
			connectionInfo.AccountId.ShouldBeNull();
		}

		[Fact]
		public void AuthorizationCode_NullForNonAuthorizationCodeGrant()
		{
			// Arrange & Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Assert
			connectionInfo.AuthorizationCode.ShouldBeNull();
		}

		[Fact]
		public void RedirectUri_NullForNonAuthorizationCodeGrant()
		{
			// Arrange & Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Assert
			connectionInfo.RedirectUri.ShouldBeNull();
		}

		[Fact]
		public void CodeVerifier_NullForNonAuthorizationCodeGrant()
		{
			// Arrange & Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Assert
			connectionInfo.CodeVerifier.ShouldBeNull();
		}

		#endregion

		#region OnTokenRefreshedDelegate Tests

		[Fact]
		public void OnTokenRefreshedDelegate_CanBeInvoked()
		{
			// Arrange
			var refreshTokenReceived = string.Empty;
			var accessTokenReceived = string.Empty;
			var callbackInvoked = false;

			OnTokenRefreshedDelegate callback = (refreshToken, accessToken) =>
			{
				callbackInvoked = true;
				refreshTokenReceived = refreshToken;
				accessTokenReceived = accessToken;
			};

			var connectionInfo = OAuthConnectionInfo.WithRefreshToken("client", "secret", "refresh", callback);

			// Act
			connectionInfo.OnTokenRefreshed?.Invoke("new_refresh", "new_access");

			// Assert
			callbackInvoked.ShouldBeTrue();
			refreshTokenReceived.ShouldBe("new_refresh");
			accessTokenReceived.ShouldBe("new_access");
		}

		[Fact]
		public void OnTokenRefreshedDelegate_NullCallback_DoesNotThrow()
		{
			// Arrange
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials("client", "secret");

			// Act & Assert
			Should.NotThrow(() => connectionInfo.OnTokenRefreshed?.Invoke("refresh", "access"));
		}

		#endregion

		#region Edge Cases

		[Fact]
		public void WithClientCredentials_EmptyAccessToken_SetsMinValueExpiration()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accessToken = string.Empty;

			// Act
			var connectionInfo = OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret, accessToken, null);

			// Assert
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
		}

		[Fact]
		public void WithRefreshToken_EmptyAccessToken_SetsMinValueExpiration()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";
			var accessToken = string.Empty;

			// Act
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, accessToken, null);

			// Assert
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
		}

		[Fact]
		public void ForServerToServer_EmptyAccessToken_SetsMinValueExpiration()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var accountId = "test_account_id";
			var accessToken = string.Empty;

			// Act
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, accessToken);

			// Assert
			connectionInfo.TokenExpiration.ShouldBe(DateTime.MinValue);
		}

		[Fact]
		public void WithAuthorizationCode_NullCallback_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var authorizationCode = "test_auth_code";

			// Act
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode, null);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.OnTokenRefreshed.ShouldBeNull();
		}

		[Fact]
		public void WithRefreshToken_NullCallback_CreatesInstanceCorrectly()
		{
			// Arrange
			var clientId = "test_client_id";
			var clientSecret = "test_client_secret";
			var refreshToken = "test_refresh_token";

			// Act
			var connectionInfo = OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken, null);

			// Assert
			connectionInfo.ShouldNotBeNull();
			connectionInfo.OnTokenRefreshed.ShouldBeNull();
		}

		#endregion
	}
}
