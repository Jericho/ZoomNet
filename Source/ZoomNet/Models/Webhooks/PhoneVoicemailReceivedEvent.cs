using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a callee misses a call and receives a voicemail from the caller.
	/// </summary>
	public class PhoneVoicemailReceivedEvent : PhoneVoicemailEvent
	{
		/// <summary>
		/// Gets or sets information about received voicemail.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookVoicemail Voicemail { get; set; }
	}
}
