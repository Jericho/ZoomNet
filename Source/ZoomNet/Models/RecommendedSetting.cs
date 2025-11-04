using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Recommended meeting settings (to enable or disable).
	/// </summary>
	public enum RecommendedSetting
	{
		/// <summary>
		/// Enable annotations.
		/// </summary>
		[EnumMember(Value = "enableAnnotation")]
		EnableAnnotation,

		/// <summary>
		/// Enable meeting chat.
		/// </summary>
		[EnumMember(Value = "enableMeetingChat")]
		EnableMeetingChat,

		/// <summary>
		/// Enable screen share.
		/// </summary>
		[EnumMember(Value = "enableScreenShare")]
		EnableScreenShare,

		/// <summary>
		/// Enable multiple share.
		/// </summary>
		[EnumMember(Value = "enableMultipleShare")]
		EnableMultipleShare,

		/// <summary>
		/// Enable password requirement.
		/// </summary>
		[EnumMember(Value = "enablePassword")]
		EnablePassword,

		/// <summary>
		/// Enable waiting room before join.
		/// </summary>
		[EnumMember(Value = "enableWaitingRoom")]
		EnableWaitingRoom,

		/// <summary>
		/// Enable only authenticated users join.
		/// </summary>
		[EnumMember(Value = "enableOnlyAuthenticated")]
		EnableOnlyAuthenticated,

		/// <summary>
		/// Enable registration requirement.
		/// </summary>
		[EnumMember(Value = "enableRegistration")]
		EnableRegistration,

		/// <summary>
		/// Enable screen share lock.
		/// </summary>
		[EnumMember(Value = "enableScreenShareLock")]
		EnableScreenShareLock,

		/// <summary>
		/// Enable screen share lock for host only.
		/// </summary>
		[EnumMember(Value = "enableScreenShareHostOnly")]
		EnableScreenShareHostOnly,

		/// <summary>
		/// Enable users from specified domain.
		/// </summary>
		[EnumMember(Value = "enableSpecifiedDomain")]
		EnableSpecifiedDomain,
	}
}
