using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A scheduled meeting.</summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public record ScheduledMeeting : Meeting
	{
		/// <summary>Gets or sets the meeting duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; init; }

		/// <summary>Gets or sets the personal meeting id.</summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; init; }

		/// <summary>Gets or sets a value indicating whether the prescheduled meeting was created via the GSuite app.</summary>
		[JsonPropertyName("pre_schedule")]
		public bool PreScheduled { get; init; } = false;

		/// <summary>Gets or sets the meeting start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; init; }
	}
}
