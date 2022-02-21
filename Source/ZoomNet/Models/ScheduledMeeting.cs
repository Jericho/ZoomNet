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

		/// <summary>
		/// Gets or sets the timezone.
		/// For example, "America/Los_Angeles".
		/// Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.
		/// </summary>
		/// <value>The meeting timezone. For example, "America/Los_Angeles". Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.</value>
		[JsonPropertyName("timezone")]
		public string Timezone { get; set; }
	}
}
