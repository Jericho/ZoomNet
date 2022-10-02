using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate when a password is required for personal meetings.
	/// </summary>
	public enum PmiMeetingPasswordRequirementType
	{
		/// <summary>
		/// Password is only necessary for JBH (???).
		/// </summary>
		[EnumMember(Value = "jbh_only")]
		JbhOnly,

		/// <summary>
		/// Password is always necessary.
		/// </summary>
		[EnumMember(Value = "all")]
		Always,

		/// <summary>
		/// Password is never necessary.
		/// </summary>
		[EnumMember(Value = "none")]
		Never
	}
}
