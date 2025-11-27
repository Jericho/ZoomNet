using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user account fails to send the SMS message.
	/// </summary>
	public class PhoneSmsSentFailedEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets SMS information payload that failed to be sent.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsMessage Message { get; set; }
	}
}
