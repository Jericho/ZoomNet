using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the OAuth grant type.
	/// </summary>
	public enum OAuthGrantType
	{
		/// <summary>
		/// Account Credentials. This is the grant type for "Server-to-Server" OAuth.
		/// </summary>
		[EnumMember(Value = "account_credentials")]
		AccountCredentials,

		/// <summary>
		/// Authorization code. This is the most commonly used grant type for Zoom APIs.
		/// </summary>
		[EnumMember(Value = "authorization_code")]
		AuthorizationCode,

		/// <summary>
		/// Client Credentials.
		/// </summary>
		[EnumMember(Value = "client_credentials")]
		ClientCredentials,

		/// <summary>
		/// Refresh token.
		/// </summary>
		[EnumMember(Value = "refresh_token")]
		RefreshToken
	}
}
