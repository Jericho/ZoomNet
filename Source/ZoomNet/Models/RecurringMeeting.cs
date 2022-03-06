using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public class RecurringMeeting : Meeting
	{
		/// <summary>
		/// Gets or sets the timezone.
		/// For example, "America/Los_Angeles".
		/// Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.
		/// </summary>
		/// <value>The meeting timezone. For example, "America/Los_Angeles". Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.</value>
		[JsonPropertyName("timezone")]
		public string Timezone { get; set; }

		/// <summary>
		/// Gets or sets the occurrences.
		/// </summary>
		[JsonPropertyName("occurrences")]
		public MeetingOccurrence[] Occurrences { get; set; }

		/// <summary>
		/// Gets or sets the recurrence info.
		/// </summary>
		[JsonPropertyName("recurrence")]
		public RecurrenceInfo RecurrenceInfo { get; set; }
	}
}
