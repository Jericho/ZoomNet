using System;
using ZoomNet.Models;

namespace ZoomNet
{
	/// <summary>
	/// Connect using OAuth.
	/// </summary>
	public class OAuthConnectionInfo : IConnectionInfo
	{
		/// <summary>
		/// Gets the client id.
		/// </summary>
		public string ClientId { get; }

		/// <summary>
		/// Gets the client secret.
		/// </summary>
		public string ClientSecret { get; }

		/// <summary>
		/// Gets the grant type.
		/// </summary>
		public OAuthGrantType GrantType { get; }

		/// <summary>
		/// Gets the authorization code.
		/// </summary>
		/// <remarks>This value is relevant only if the grant type is "AuthorizationCode".</remarks>
		public string AuthorizationCode { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is used to get access token for APIs that do not
		/// need a user’s permission, but rather a service’s permission.
		/// Within the realm of Zoom APIs, Client Credentials grant should be
		/// used to get access token from the Chatbot Service in order to use
		/// the "Send Chatbot Messages API". See the "Using OAuth 2.0 / Client
		/// Credentials" section in the "Using Zoom APIs" document for more details
		/// (https://marketplace.zoom.us/docs/api-reference/using-zoom-apis).
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		public OAuthConnectionInfo(string clientId, string clientSecret)
		{
			ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
			GrantType = OAuthGrantType.ClientCredentials;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthConnectionInfo"/> class.
		/// </summary>
		/// <remarks>
		/// This is the most commonly used grant type for Zoom APIs.
		/// </remarks>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="authorizationCode">The authorization code.</param>
		public OAuthConnectionInfo(string clientId, string clientSecret, string authorizationCode)
		{
			ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
			AuthorizationCode = authorizationCode ?? throw new ArgumentNullException(nameof(authorizationCode));
			GrantType = OAuthGrantType.AuthorizationCode;
		}
	}
}
