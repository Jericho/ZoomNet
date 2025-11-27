using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user account receives an SMS message.
	/// </summary>
	public class PhoneSmsReceivedEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets SMS information payload that was received.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsMessage Message { get; set; }
	}
}
