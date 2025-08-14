using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Recurrence for recurring events.
	/// </summary>
	public class EventRecurrenceInfo : RecurrenceInfo
	{
		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }
	}
}
