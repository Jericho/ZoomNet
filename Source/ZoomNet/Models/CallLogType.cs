using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Enumeration to indicate the type of a call log.
/// </summary>
public enum CallLogType
{
	/// <summary>
	/// All.
	/// </summary>
	[EnumMember(Value = "all")]
	All,

	/// <summary>
	/// Missed.
	/// </summary>
	[EnumMember(Value = "missed")]
	Missed,
}
