using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user account sends an SMS message.
	/// </summary>
	public class PhoneSmsSentEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets SMS information payload that was sent.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsMessage Message { get; set; }
	}
}
