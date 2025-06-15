using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the user role type.
	/// </summary>
	public enum UserRoleType
	{
		/// <summary>Host.</summary>
		[EnumMember(Value = "host")]
		Host,

		/// <summary>Attendee.</summary>
		[EnumMember(Value = "attendee")]
		Attendee
	}
}
