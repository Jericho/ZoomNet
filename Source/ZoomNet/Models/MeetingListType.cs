using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting to be listed.
	/// </summary>
	public enum MeetingListType
	{
		/// <summary>
		/// Scheduled.
		/// </summary>
		[EnumMember(Value = "scheduled")]
		Scheduled,

		/// <summary>
		/// Live.
		/// </summary>
		[EnumMember(Value = "live")]
		Live,

		/// <summary>
		/// Upcoming.
		/// </summary>
		[EnumMember(Value = "upcoming")]
		Upcoming,
	}
}
