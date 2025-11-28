using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a voicemail transcription has completed.
	/// </summary>
	public class PhoneVoicemailTranscriptCompletedEvent : PhoneVoicemailEvent
	{
		/// <summary>
		/// Gets or sets information about completed voicemail transcript.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookVoicemail Voicemail { get; set; }
	}
}
