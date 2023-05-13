using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a participant.
	/// </summary>
	public class ReportParticipant : Participant
	{
		/// <summary>
		/// Gets or sets the CustomerKey of the participant.
		/// </summary>
		/// <value>
		/// The CustomerKey of the participant.
		/// This is another identifier with a max length of 15 characters.
		/// </value>
		[JsonPropertyName("customer_key")]
		public string CustomerKey { get; set; }
	}
}
