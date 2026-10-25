using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A meeting.</summary>
	/// <seealso cref="ZoomNet.Models.Webinar" />
	public record RecurringWebinar : Webinar
	{
		/// <summary>Gets or sets the occurrences.</summary>
		[JsonPropertyName("occurrences")]
		public MeetingOccurrence[] Occurrences { get; init; }

		/// <summary>Gets or sets the recurrence info.</summary>
		[JsonPropertyName("recurrence")]
		public RecurrenceInfo RecurrenceInfo { get; init; }
	}
}
