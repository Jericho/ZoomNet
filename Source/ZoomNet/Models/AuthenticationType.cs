using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of authentication.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AuthenticationType
	{
		/// <summary>
		/// Internal.
		/// </summary>
		[EnumMember(Value = "internally")]
		Internal,

		/// <summary>
		/// Enforce login.
		/// </summary>
		[EnumMember(Value = "enforce_login")]
		EnforceLogin,

		/// <summary>
		/// Enforce login with domains.
		/// </summary>
		[EnumMember(Value = "enforce_login_with_domains")]
		EnforceLoginWithDomains
	}
}
