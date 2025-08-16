using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the event authentication method for a ticket.
	/// </summary>
	public enum EventAuthenticationMethod
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

		/// <summary>No authentication.</summary>
		[EnumMember(Value = "no_auth")]
		None,

		/// <summary>Registration using email OTP verification.</summary>
		[EnumMember(Value = "email_opt")]
		EmailOpt,

		/// <summary>Pre-registration using email OTP verification.</summary>
		[EnumMember(Value = "zoom_account_otp_at_join")]
		PreRegistrationWithEmailOtpAtJoin
	}
}
