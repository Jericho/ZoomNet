using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings for inbound/outbound recording audio notification.
	/// </summary>
	public class RecordingAudioNotification
	{
		/// <summary>
		/// Gets or sets a value indicating whether the 'Press 1' option for recording consent is enabled.
		/// </summary>
		[JsonPropertyName("recording_explicit_consent")]
		public bool RecordingExplicitConsent { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a prompt plays to call participants when the recording has started.
		/// </summary>
		[JsonPropertyName("recording_start_prompt")]
		public bool RecordingStartPrompt { get; set; }

		/// <summary>
		/// Gets or sets the audio that plays when the recording has started.
		/// </summary>
		[JsonPropertyName("recording_start_prompt_audio_id")]
		public string RecordingStartPromptAudioId { get; set; }
	}
}
