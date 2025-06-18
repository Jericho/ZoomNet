using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of an event registrant.
	/// </summary>
	public enum EventRegistrantStatus
	{
		/// <summary>Registered.</summary>
		[EnumMember(Value = "REGISTERED")]
		Registered,

		/// <summary>Invited.</summary>
		[EnumMember(Value = "INVITED")]
		Invited,

		/// <summary>Gifted.</summary>
		[EnumMember(Value = "GIFTED")]
		Gifted,

		/// <summary>Direct join.</summary>
		[EnumMember(Value = "DIRECT_JOIN")]
		DirectJoin,

		/// <summary>Pre-registered.</summary>
		[EnumMember(Value = "PRE_REGISTERED")]
		PreRegistered,

		/// <summary>Pre-register invited.</summary>
		[EnumMember(Value = "PRE_REGISTER_INVITED")]
		PreRegisterInvited,
	}
}
