using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the event authentication method during registration or during join, depending on the access link type.
	/// </summary>
	public enum EventAccessLinkAuthenticationMethod
	{
		/// <summary>Zoom account holder.</summary>
		[EnumMember(Value = "zoom_account")]
		ZoomAccount,

		/// <summary>Zoom account holder with OTP.</summary>
		[EnumMember(Value = "zoom_account_otp")]
		ZoomAccountOtp,

		/// <summary>Corporate IDP .</summary>
		[EnumMember(Value = "corporate_idp")]
		CorporateIdp,

		/// <summary>Bypass authentication.</summary>
		[EnumMember(Value = "bypass_auth")]
		BypassAuthentication,

		/// <summary>Authentication through vanity URL.</summary>
		[EnumMember(Value = "accelerated_auth")]
		VanityUrl,
	}
}
