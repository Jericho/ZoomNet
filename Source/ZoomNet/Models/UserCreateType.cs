using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to specify how to create a new user.
	/// </summary>
	public enum UserCreateType
	{
		/// <summary>
		/// User will get an email sent from Zoom.
		/// There is a confirmation link in this email.
		/// User will then need to click this link to activate their account to the Zoom service.
		/// The user can set or change their password in Zoom.
		/// </summary>
		[EnumMember(Value = "create")]
		Normal,

		/// <summary>
		/// This action is provided for enterprise customer who has a managed domain.
		/// This feature is disabled by default because of the security risk involved in creating a user who does not belong to your domain without notifying the user.
		/// </summary>
		[EnumMember(Value = "autoCreate")]
		Auto,

		/// <summary>
		/// This action is provided for API partner only.
		/// User created in this way has no password and is not able to log into the Zoom web site or client.
		/// </summary>
		[EnumMember(Value = "custCreate")]
		Cust,

		/// <summary>
		/// This action is provided for enabled "Pre-provisioning SSO User" option.
		/// User created in this way has no password.
		/// If it is not a basic user, will generate a Personal Vanity URL using user name (no domain) of the provisioning email.
		/// If user name or pmi is invalid or occupied, will use random number/random personal vanity URL.
		/// </summary>
		[EnumMember(Value = "ssoCreate")]
		SSo
	}
}
