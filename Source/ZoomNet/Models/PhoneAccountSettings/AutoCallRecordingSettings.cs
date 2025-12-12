using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that control automatic calls recording.
	/// </summary>
	public class AutoCallRecordingSettings : CallRecordingSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the stop and resume of automatic call recording is enabled.
		/// </summary>
		[JsonPropertyName("allow_stop_resume_recording")]
		public bool? AllowStopResumeRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a call disconnects when there is an issue with the automatic call recording and the call cannot reconnect after five seconds.
		/// This does not include emergency calls.
		/// </summary>
		[JsonPropertyName("disconnect_on_recording_failure")]
		public bool? DisconnectOnRecordingFailure { get; set; }

		/// <summary>
		/// Gets or sets the type of automatically recorded calls.
		/// </summary>
		[JsonPropertyName("recording_calls")]
		public AutoRecordedCallType? RecordingCalls { get; set; }

		/// <summary>
		/// Gets or sets the audio that plays when the recording has started.
		/// </summary>
		[JsonPropertyName("recording_start_prompt_audio_id")]
		public string RecordingStartPromptAudioId { get; set; }
	}
}
