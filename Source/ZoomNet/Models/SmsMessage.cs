using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS message information as provided in API endpoints.
	/// </summary>
	public class SmsMessage : SmsMessageBase
	{
		/// <summary>
		/// Gets or sets the SMS direction.
		/// </summary>
		[JsonPropertyName("direction")]
		public SmsDirection Direction { get; set; }

		/// <summary>
		/// Gets or sets the SMS sender.
		/// </summary>
		[JsonPropertyName("sender")]
		public SmsHistoryParticipant Sender { get; set; }

		/// <summary>
		/// Gets or sets the SMS receivers.
		/// </summary>
		[JsonPropertyName("to_members")]
		public SmsHistoryParticipant[] Recipients { get; set; }
	}
}
