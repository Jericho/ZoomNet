using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Recurrence for recurring events.</summary>
	public record EventRecurrenceInfo : RecurrenceInfo
	{
		/// <summary>Gets or sets the duration.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; init; }
	}
}
