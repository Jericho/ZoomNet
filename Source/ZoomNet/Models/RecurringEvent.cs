using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A recurring event.</summary>
	/// <seealso cref="ZoomNet.Models.Event" />
	public record RecurringEvent : Event
	{
		/// <summary>Gets or sets the recurrence info.</summary>
		[JsonPropertyName("recurrence")]
		public EventRecurrenceInfo RecurrenceInfo { get; set; }
	}
}
