using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public class RecurringMeeting : Meeting
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
		/// Gets or sets the timezone.
		/// For example, "America/Los_Angeles".
		/// Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.
		/// </summary>
		/// <value>The meeting timezone. For example, "America/Los_Angeles". Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.</value>
		[JsonProperty(PropertyName = "timezone", NullValueHandling = NullValueHandling.Ignore)]
		public string Timezone { get; set; }


		/// <summary>
		/// Gets or sets the occurrences.
		/// </summary>
		[JsonProperty(PropertyName = "occurrences", NullValueHandling = NullValueHandling.Ignore)]
		public MeetingOccurrence[] Occurrences { get; set; }

		/// <summary>
		/// Gets or sets the recurrence info.
		/// </summary>
		[JsonProperty(PropertyName = "recurrence", NullValueHandling = NullValueHandling.Ignore)]
		public RecurrenceInfo RecurrenceInfo { get; set; }
	}
}
