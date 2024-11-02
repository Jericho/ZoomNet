using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting to be listed.
	/// </summary>
	public enum MeetingListType
	{
		/// <summary>
		/// All valid previous (unexpired) meetings, live meetings, and upcoming scheduled meetings.
		/// </summary>
		[EnumMember(Value = "scheduled")]
		Scheduled,

		/// <summary>
		/// All the ongoing meetings.
		/// </summary>
		[EnumMember(Value = "live")]
		Live,

		/// <summary>
		/// All upcoming meetings, including live meetings.
		/// </summary>
		[EnumMember(Value = "upcoming")]
		Upcoming,

		/// <summary>
		/// All upcoming meetings, including live meetings.
		/// </summary>
		/// <remarks>I don't know what the distinction between "upcoming" and "upcoming_meetings is.</remarks>
		[EnumMember(Value = "upcoming_meetings")]
		UpcomingMeetings,

		/// <summary>
		/// All the previous meetings.
		/// </summary>
		[EnumMember(Value = "previous_meetings")]
		PreviousMeetings,
	}
}
