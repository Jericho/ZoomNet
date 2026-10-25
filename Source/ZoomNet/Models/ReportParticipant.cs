using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics of a participant.</summary>
	public record ReportParticipant : Participant
	{
		/// <summary>Gets or sets the CustomerKey of the participant.</summary>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; init; }
	}
}
