using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Webinar" />
	public class RecurringWebinar : Webinar
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
	}
}
