using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Enumeration to indicate the type of a host.
/// </summary>
public enum ReportHostType
{
	/// <summary>
	/// Active.
	/// </summary>
	[EnumMember(Value = "active")]
	Active,

	/// <summary>
	/// Inactive.
	/// </summary>
	[EnumMember(Value = "inactive")]
	Inactive,
}
