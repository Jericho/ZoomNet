using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Enumeration to indicate the type of meeting metrics are being returned for.
/// </summary>
public enum ReportMeetingType
{
	/// <summary>
	/// All past meetings.
	/// </summary>
	[EnumMember(Value = "past")]
	Past,

	/// <summary>
	/// A single past user meeting.
	/// </summary>
	[EnumMember(Value = "pastOne")]
	PastOne,

	/// <summary>
	/// All past meetings the account's users hosted or joined.
	/// </summary>
	[EnumMember(Value = "pastJoined")]
	PastJoined
}
