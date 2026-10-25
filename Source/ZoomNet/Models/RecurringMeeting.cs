using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A meeting.</summary>
	/// <seealso cref="ZoomNet.Models.Meeting" />
	public record RecurringMeeting : Meeting
	{
		/// <summary>Gets or sets the occurrences.</summary>
		[JsonPropertyName("occurrences")]
		public MeetingOccurrence[] Occurrences { get; init; }

		/// <summary>Gets or sets the personal meeting id.</summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; init; }

		/// <summary>Gets or sets a value indicating whether the prescheduled meeting was created via the GSuite app.</summary>
		[JsonPropertyName("pre_schedule")]
		public bool PreScheduled { get; init; } = false;

		/// <summary>Gets or sets the recurrence info.</summary>
		[JsonPropertyName("recurrence")]
		public RecurrenceInfo RecurrenceInfo { get; init; }
	}
}
