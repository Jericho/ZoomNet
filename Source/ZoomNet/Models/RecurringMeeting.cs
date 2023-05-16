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
		/// Gets or sets the occurrences.
		/// </summary>
		[JsonPropertyName("occurrences")]
		public MeetingOccurrence[] Occurrences { get; set; }

		/// <summary>
		/// Gets or sets the recurrence info.
		/// </summary>
		[JsonPropertyName("recurrence")]
		public RecurrenceInfo RecurrenceInfo { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting id.
		/// </summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the prescheduled meeting was created via the GSuite app.
		/// </summary>
		[JsonPropertyName("pre_scheduled")]
		public bool PreScheduled { get; set; } = false;
	}
}
