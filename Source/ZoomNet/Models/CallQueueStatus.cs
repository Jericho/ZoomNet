using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Represents the status of a Call Queue.
/// </summary>
public enum CallQueueStatus
{
	/// <summary>
	/// An active call queue.
	/// </summary>
	[EnumMember(Value = "active")]
	Active,

	/// <summary>
	/// Call queue has been deactivated.
	/// </summary>
	[EnumMember(Value = "inactive")]
	Inactive
}
