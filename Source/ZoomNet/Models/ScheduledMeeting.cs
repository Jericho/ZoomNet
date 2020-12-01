using Newtonsoft.Json;
using System;

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
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting duration in minutes.
		/// </summary>
		/// <value>The meeting duration in minutes.</value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public int Duration { get; set; }

		/// <summary>
		/// Sets the userid of another user to schedule the meeting for.
		/// </summary>
		/// <value>Email or UserId if you want to schedule meeting for another user.</value>
		[JsonProperty(PropertyName = "schedule_for", NullValueHandling = NullValueHandling.Ignore)]
		public string ScheduleFor { get; set; }

		/// <summary>
		/// Gets or sets the timezone.
		/// For example, "America/Los_Angeles".
		/// Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.
		/// </summary>
		/// <value>The meeting timezone. For example, "America/Los_Angeles". Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.</value>
		[JsonProperty(PropertyName = "timezone", NullValueHandling = NullValueHandling.Ignore)]
		public string Timezone { get; set; }
	}
}
