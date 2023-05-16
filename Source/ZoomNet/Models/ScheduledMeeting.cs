using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A scheduled meeting.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public class ScheduledMeeting : Meeting
	{
		/// <summary>
		/// Gets or sets the meeting start time.
		/// </summary>
		/// <value>The meeting start time. Only used for scheduled meetings and recurring meetings with fixed time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting duration in minutes.
		/// </summary>
		/// <value>The meeting duration in minutes.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }
	}
}
