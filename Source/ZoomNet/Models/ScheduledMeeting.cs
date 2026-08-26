using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A scheduled meeting.</summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public class ScheduledMeeting : Meeting
	{
		/// <summary>Gets or sets the meeting duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>Gets or sets the personal meeting id.</summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; set; }

		/// <summary>Gets or sets a value indicating whether the prescheduled meeting was created via the GSuite app.</summary>
		[JsonPropertyName("pre_scheduled")]
		public bool PreScheduled { get; set; } = false;

		/// <summary>Gets or sets the meeting start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }
	}
}
