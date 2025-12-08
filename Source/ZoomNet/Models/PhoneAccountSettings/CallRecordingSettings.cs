using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings related to call recording.
	/// </summary>
	public abstract class CallRecordingSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets settings related to playing the recording beep tone.
		/// </summary>
		[JsonPropertyName("play_recording_beep_tone")]
		public PlayRecordingBeepTone PlayRecordingBeepTone { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the 'Press 1' option for recording consent is enabled.
		/// </summary>
		[JsonPropertyName("recording_explicit_consent")]
		public bool? RecordingExplicitConsent { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a prompt plays to call participants when the recording has started.
		/// </summary>
		[JsonPropertyName("recording_start_prompt")]
		public bool? RecordingStartPrompt { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the call recording transcription is enabled.
		/// </summary>
		[JsonPropertyName("recording_transcription")]
		public bool? RecordingTranscription { get; set; }

		/// <summary>
		/// Gets or sets settings for inbound recording audio notification.
		/// </summary>
		[JsonPropertyName("inbound_audio_notification")]
		public RecordingAudioNotification InboundAudioNotification { get; set; }

		/// <summary>
		/// Gets or sets settings for outbound recording audio notification.
		/// </summary>
		[JsonPropertyName("outbound_audio_notification")]
		public RecordingAudioNotification OutboundAudioNotification { get; set; }
	}
}
