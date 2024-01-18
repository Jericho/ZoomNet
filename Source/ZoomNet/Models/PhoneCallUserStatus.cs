using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Represents the status of a phone user.
/// </summary>
public enum PhoneCallUserStatus
{
	/// <summary>
	/// An active user.
	/// </summary>
	[EnumMember(Value = "activate")]
	Activate,

	/// <summary>
	/// User has been deactivated from the Zoom Phone system.
	/// </summary>
	[EnumMember(Value = "deactivate")]
	Deactivate
}
