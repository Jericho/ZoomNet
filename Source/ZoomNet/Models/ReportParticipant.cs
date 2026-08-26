using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics of a participant.</summary>
	public class ReportParticipant : Participant
	{
		/// <summary>Gets or sets the CustomerKey of the participant.</summary>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }
	}
}
