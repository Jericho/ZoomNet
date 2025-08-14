using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of an event when retrieving a list of events.
	/// </summary>
	public enum EventListStatus
	{
		/// <summary>Upcoming.</summary>
		[EnumMember(Value = "upcoming")]
		Upcoming,

		/// <summary>Past.</summary>
		[EnumMember(Value = "past")]
		Past,

		/// <summary>Draft.</summary>
		[EnumMember(Value = "draft")]
		Draft,

		/// <summary>Cancelled.</summary>
		[EnumMember(Value = "cancelled")]
		Cancelled,
	}
}
