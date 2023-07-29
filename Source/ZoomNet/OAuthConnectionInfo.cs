using System;
using System.Collections.Generic;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// The delegate invoked when a token is refreshed.
	/// </summary>
	/// <param name="newRefreshToken">The new refresh token.</param>
	/// <param name="newAccessToken">The new access token.</param>
	public delegate void OnTokenRefreshedDelegate(string newRefreshToken, string newAccessToken);

	/// <summary>
	/// Connect using OAuth.
	/// </summary>
	public class OAuthConnectionInfo : IConnectionInfo
	{
		/// <summary>
		/// Gets the token repository.
		/// </summary>
		public ITokenRepository TokenRepository { get; private set; }

		/// <summary>
		/// Gets the account id.
		/// </summary>
		/// <remarks>This is relevant only when using Server-to-Server authentication.</remarks>
		public string AccountId { get; private set; }

		/// <summary>
		/// Gets the client id.
		/// </summary>
		public string ClientId { get; private set; }

		/// <summary>
		/// Gets the client secret.
		/// </summary>
		public string ClientSecret { get; private set; }

		/// <summary>
		/// Gets the grant type.
		/// </summary>
		public OAuthGrantType GrantType { get; internal set; }

		/// <summary>
		/// Gets the authorization code.
		/// </summary>
		public string AuthorizationCode { get; private set; }

		/// <summary>
		/// Gets the token scope.
		/// </summary>
		public IReadOnlyDictionary<string, string[]> TokenScope { get; internal set; }

		/// <summary>
		/// Gets the delegate invoked when the token is refreshed.
		/// </summary>
		public OnTokenRefreshedDelegate OnTokenRefreshed { get; private set; }

		/// <summary>
		/// Gets the redirectUri required for refresh of tokens.
		/// </summary>
		public string RedirectUri { get; private set; }

		/// <summary>
		/// Gets the cryptographically random string used to correlate the authorization request to the token request.
		/// </summary>
		public string CodeVerifier { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is used to get access token for APIs that do not
		/// need a user's permission, but rather a service's permission.
		/// Within the realm of Zoom APIs, Client Credentials grant should be
		/// used to get access token from the Chatbot Service in order to use
		/// the "Send Chatbot Messages API". See the "Using OAuth 2.0 / Client
		/// Credentials" section in the "Using Zoom APIs" document for more details
		/// (https://marketplace.zoom.us/docs/api-reference/using-zoom-apis).
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		[Obsolete("This constructor has been replaced with OAuthConnectionInfo.WithClientCredentials")]
		public OAuthConnectionInfo(string clientId, string clientSecret)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

			ClientId = clientId;
			ClientSecret = clientSecret;
			GrantType = OAuthGrantType.ClientCredentials;
			TokenRepository = new MemoryTokenRepository(null, null, DateTime.MinValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// The authorization code is generated by Zoom when the user authorizes the app.
		/// This code can be used only one time to get the initial access token and refresh token.
		/// Once the authorization code has been used, is is no longer valid and should be discarded.
		///
		/// Also, Zoom's documentation says that the redirect uri must be provided when validating an
		/// authorization code and converting it into tokens. However I have observed that it's not
		/// always necessary. It seems that some developers get a "REDIRECT URI MISMATCH" exception when
		/// they omit this value but other developers don't. Therefore, the redirectUri parameter is
		/// marked as optional in ZoomNet which allows you to specify it or omit it depending on your
		/// situation. See this <a href="https://github.com/Jericho/ZoomNet/issues/104">Github issue</a>
		/// and this <a href="https://devforum.zoom.us/t/trying-to-integrate-not-understanding-the-need-for-the-second-redirect-uri/43833">support thread</a>
		/// for more details.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="authorizationCode">The authorization code.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when the token is refreshed.</param>
		/// <param name="redirectUri">The Redirect Uri.</param>
		/// <param name="codeVerifier">The cryptographically random string used to correlate the authorization request to the token request.</param>
		[Obsolete("This constructor has been replaced with OAuthConnectionInfo.WithAuthorizationCode")]
		public OAuthConnectionInfo(string clientId, string clientSecret, string authorizationCode, OnTokenRefreshedDelegate onTokenRefreshed, string redirectUri = null, string codeVerifier = null)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(authorizationCode)) throw new ArgumentNullException(nameof(authorizationCode));

			ClientId = clientId;
			ClientSecret = clientSecret;
			AuthorizationCode = authorizationCode;
			RedirectUri = redirectUri;
			GrantType = OAuthGrantType.AuthorizationCode;
			OnTokenRefreshed = onTokenRefreshed;
			CodeVerifier = codeVerifier;
			TokenRepository = new MemoryTokenRepository(null, null, DateTime.MinValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This is the most commonly used grant type for Zoom APIs.
		///
		/// Please note that the 'accessToken' parameter is optional.
		/// In fact, we recommend that you specify a null value which
		/// will cause ZoomNet to automatically obtain a new access
		/// token from the Zoom API. The reason we recommend you omit
		/// this parameter is that access tokens are ephemeral (they
		/// expire in 60 minutes) and even if you specify a token that
		/// was previously issued to you and that you preserved, this
		/// token is very likely to be expired and therefore useless.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="refreshToken">The refresh token.</param>
		/// <param name="accessToken">(Optional) The access token. We recommend you specify a null value. See remarks for more details.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when the token is refreshed.</param>
		[Obsolete("This constructor has been replaced with OAuthConnectionInfo.WithRefreshToken")]
		public OAuthConnectionInfo(string clientId, string clientSecret, string refreshToken, string accessToken, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

			ClientId = clientId;
			ClientSecret = clientSecret;
			GrantType = OAuthGrantType.RefreshToken;
			OnTokenRefreshed = onTokenRefreshed;
			TokenRepository = new MemoryTokenRepository(refreshToken, accessToken, string.IsNullOrEmpty(accessToken) ? DateTime.MinValue : DateTime.MaxValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor when you want to use Server-to-Server OAuth authentication.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accountId">Your Account Id.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when the token is refreshed. In the Server-to-Server scenario, this delegate is optional.</param>
		[Obsolete("This constructor has been replaced with OAuthConnectionInfo.ForServerToServer")]
		public OAuthConnectionInfo(string clientId, string clientSecret, string accountId, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(accountId)) throw new ArgumentNullException(nameof(accountId));

			ClientId = clientId;
			ClientSecret = clientSecret;
			AccountId = accountId;
			GrantType = OAuthGrantType.AccountCredentials;
			OnTokenRefreshed = onTokenRefreshed;
			TokenRepository = new MemoryTokenRepository(null, null, DateTime.MinValue);
		}

		private OAuthConnectionInfo() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is used to get access token for APIs that do not
		/// need a user's permission, but rather a service's permission.
		/// Within the realm of Zoom APIs, Client Credentials grant should be
		/// used to get access token from the Chatbot Service in order to use
		/// the "Send Chatbot Messages API". See the "Using OAuth 2.0 / Client
		/// Credentials" section in the "Using Zoom APIs" document for more details
		/// (https://marketplace.zoom.us/docs/api-reference/using-zoom-apis).
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithClientCredentials(string clientId, string clientSecret)
		{
			return WithClientCredentials(clientId, clientSecret, null, null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is used to get access token for APIs that do not
		/// need a user's permission, but rather a service's permission.
		/// Within the realm of Zoom APIs, Client Credentials grant should be
		/// used to get access token from the Chatbot Service in order to use
		/// the "Send Chatbot Messages API". See the "Using OAuth 2.0 / Client
		/// Credentials" section in the "Using Zoom APIs" document for more details
		/// (https://marketplace.zoom.us/docs/api-reference/using-zoom-apis).
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithClientCredentials(string clientId, string clientSecret, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			return WithClientCredentials(clientId, clientSecret, null, onTokenRefreshed);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is used to get access token for APIs that do not
		/// need a user's permission, but rather a service's permission.
		/// Within the realm of Zoom APIs, Client Credentials grant should be
		/// used to get access token from the Chatbot Service in order to use
		/// the "Send Chatbot Messages API". See the "Using OAuth 2.0 / Client
		/// Credentials" section in the "Using Zoom APIs" document for more details
		/// (https://marketplace.zoom.us/docs/api-reference/using-zoom-apis).
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accessToken">The access token.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithClientCredentials(string clientId, string clientSecret, string accessToken, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

			return new OAuthConnectionInfo
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				GrantType = OAuthGrantType.ClientCredentials,
				OnTokenRefreshed = onTokenRefreshed,
				TokenRepository = new MemoryTokenRepository(null, accessToken, string.IsNullOrEmpty(accessToken) ? DateTime.MinValue : DateTime.MaxValue),
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// The authorization code is generated by Zoom when the user authorizes the app.
		/// This code can be used only one time to get the initial access token and refresh token.
		/// Once the authorization code has been used, is is no longer valid and should be discarded.
		///
		/// Also, Zoom's documentation says that the redirect uri must be provided when validating an
		/// authorization code and converting it into tokens. However I have observed that it's not
		/// always necessary. It seems that some developers get a "REDIRECT URI MISMATCH" exception when
		/// they omit this value but other developers don't. Therefore, the redirectUri parameter is
		/// marked as optional in ZoomNet which allows you to specify it or omit it depending on your
		/// situation. See this <a href="https://github.com/Jericho/ZoomNet/issues/104">Github issue</a>
		/// and this <a href="https://devforum.zoom.us/t/trying-to-integrate-not-understanding-the-need-for-the-second-redirect-uri/43833">support thread</a>
		/// for more details.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="authorizationCode">The authorization code.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued.</param>
		/// <param name="redirectUri">The Redirect Uri.</param>
		/// <param name="codeVerifier">The cryptographically random string used to correlate the authorization request to the token request.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithAuthorizationCode(string clientId, string clientSecret, string authorizationCode, OnTokenRefreshedDelegate onTokenRefreshed, string redirectUri = null, string codeVerifier = null)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(authorizationCode)) throw new ArgumentNullException(nameof(authorizationCode));

			return new OAuthConnectionInfo
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				AuthorizationCode = authorizationCode,
				RedirectUri = redirectUri,
				GrantType = OAuthGrantType.AuthorizationCode,
				OnTokenRefreshed = onTokenRefreshed,
				CodeVerifier = codeVerifier,
				TokenRepository = new MemoryTokenRepository(null, null, DateTime.MinValue),
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This is the most commonly used grant type for Zoom APIs.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="refreshToken">The refresh token.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued or refreshed.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithRefreshToken(string clientId, string clientSecret, string refreshToken, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			return WithRefreshToken(clientId, clientSecret, refreshToken, null, onTokenRefreshed);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This is the most commonly used grant type for Zoom APIs.
		///
		/// Please note that the 'accessToken' parameter is optional.
		/// In fact, we recommend that you specify a null value which
		/// will cause ZoomNet to automatically obtain a new access
		/// token from the Zoom API. The reason we recommend you omit
		/// this parameter is that access tokens are ephemeral (they
		/// expire in 60 minutes) and even if you specify a token that
		/// was previously issued to you and that you preserved, this
		/// token is very likely to be expired and therefore useless.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="refreshToken">The refresh token.</param>
		/// <param name="accessToken">(Optional) The access token. We recommend you specify a null value. See remarks for more details.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued or refreshed.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo WithRefreshToken(string clientId, string clientSecret, string refreshToken, string accessToken, OnTokenRefreshedDelegate onTokenRefreshed)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

			return new OAuthConnectionInfo
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				GrantType = OAuthGrantType.RefreshToken,
				OnTokenRefreshed = onTokenRefreshed,
				TokenRepository = new MemoryTokenRepository(refreshToken, accessToken, string.IsNullOrEmpty(accessToken) ? DateTime.MinValue : DateTime.MaxValue),
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor when you want to use Server-to-Server OAuth authentication.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accountId">Your Account Id.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued. In the Server-to-Server scenario, this delegate is optional.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo ForServerToServer(string clientId, string clientSecret, string accountId, OnTokenRefreshedDelegate onTokenRefreshed = null)
		{
			var tokenRepository = new MemoryTokenRepository(null, null, DateTime.MinValue);
			return ForServerToServer(clientId, clientSecret, accountId, tokenRepository, onTokenRefreshed);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor when you want to use Server-to-Server OAuth authentication.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accountId">Your Account Id.</param>
		/// <param name="accessToken">The access token.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when a token is issued. In the Server-to-Server scenario, this delegate is optional.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo ForServerToServer(string clientId, string clientSecret, string accountId, string accessToken, OnTokenRefreshedDelegate onTokenRefreshed = null)
		{
			var tokenRepository = new MemoryTokenRepository(null, accessToken, string.IsNullOrEmpty(accessToken) ? DateTime.MinValue : DateTime.MaxValue);
			return ForServerToServer(clientId, clientSecret, accountId, tokenRepository, onTokenRefreshed);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor when you want to use Server-to-Server OAuth authentication.
		///
		/// Also note that by default the only 'token index' available on a Zoom account is zero.
		/// You have to contact Zoom support to enable additional indices if desired.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accountId">Your Account Id.</param>
		/// <param name="tokenRepository">The token repository.</param>
		/// <param name="onTokenRefreshed">The delegate invoked when the token is refreshed. In the Server-to-Server scenario, this delegate is optional.</param>
		/// <returns>The connection info.</returns>
		public static OAuthConnectionInfo ForServerToServer(string clientId, string clientSecret, string accountId, ITokenRepository tokenRepository, OnTokenRefreshedDelegate onTokenRefreshed = null)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(accountId)) throw new ArgumentNullException(nameof(accountId));
			if (tokenRepository == null) throw new ArgumentNullException(nameof(tokenRepository));

			return new OAuthConnectionInfo
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				AccountId = accountId,
				GrantType = OAuthGrantType.AccountCredentials,
				OnTokenRefreshed = onTokenRefreshed,
				TokenRepository = tokenRepository,
			};
		}
	}
}
