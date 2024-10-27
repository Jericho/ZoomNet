using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Recording user settings.
	/// </summary>
	public class RecordingUserSettings
	{
		/// <summary>Gets or sets a value indicating whether to ask the host to confirm the disclaimer.</summary>
		[JsonPropertyName("ask_host_to_confirm_disclaimer")]
		public bool AskHostToConfirmDisclaimer { get; set; }

		/// <summary>Gets or sets a value indicating whether to ask participants for consent when a recording starts.</summary>
		/// <remarks>This property can be used if <see cref="ShowDisclaimerBeforeRecordingStarts"/> is set to true.</remarks>
		[JsonPropertyName("ask_participants_to_consent_disclaimer")]
		public bool AskParticipantsToConsentDisclaimer { get; set; }

		/// <summary>Gets or sets a value indicating whether to automatically delete recording in the cloud.</summary>
		[JsonPropertyName("auto_delete_cmr")]
		public bool AutoDeleteCloudRecordings { get; set; }

		/// <summary>Gets or sets the number of days to keep a recording before automatically deleting it.</summary>
		/// <remarks>Zoom only accepts the following values: 30, 60, 90 and 120.</remarks>
		[JsonPropertyName("auto_delete_cmr_days")]
		public int NumberOfDaysToKeepCloudRecordings { get; set; }

		/// <summary>Gets or sets the auto recording.</summary>
		[JsonPropertyName("auto_recording")]
		public AutoRecordingType AutoRecording { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable cloud recording.</summary>
		[JsonPropertyName("cloud_recording")]
		public bool EnableCloudRecording { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to pause/stop the auto recording in the cloud.</summary>
		[JsonPropertyName("host_pause_stop_recording")]
		public bool AllowHostStopCloudRecording { get; set; }

		/// <summary>Gets or sets the settings to allow cloud recording access only from specific IP address ranges.</summary>
		[JsonPropertyName("ip_address_access_control")]
		public IpAddressAccessControlSettings IpAddressAccessControl { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable local recording.</summary>ut
		[JsonPropertyName("local_recording")]
		public bool EnableLocalRecording { get; set; }

		/// <summary>Gets or sets a value indicating whether to record an audio only file.</summary>
		[JsonPropertyName("record_audio_file")]
		public bool RecordAudioOnly { get; set; }

		/// <summary>Gets or sets a value indicating whether to record the gallery view.</summary>
		[JsonPropertyName("record_gallery_view")]
		public bool RecordGalleryView { get; set; }

		/// <summary>Gets or sets a value indicating whether to record the active speaker view.</summary>
		[JsonPropertyName("record_speaker_view")]
		public bool RecordSpeakerView { get; set; }

		/// <summary>Gets or sets a value indicating whether to record the audio transcript.</summary>
		[JsonPropertyName("recording_audio_transcript")]
		public bool RecordAudioTranscript { get; set; }

		/// <summary>Gets or sets a value indicating whether to show a disclaimer to participants before a recording starts.</summary>
		[JsonPropertyName("recording_disclaimer")]
		public bool ShowDisclaimerBeforeRecordingStarts { get; set; }

		/// <summary>Gets or sets the password requirements.</summary>
		[JsonPropertyName("recording_password_requirement")]
		public PasswordRequirements PasswordRequirements { get; set; }

		/// <summary>Gets or sets a value indicating whether to save chat text from the meeting.</summary>
		[JsonPropertyName("save_chat_text")]
		public bool SaveChatText { get; set; }

		/// <summary>Gets or sets a value indicating whether to show timestamp on video.</summary>
		[JsonPropertyName("show_timestamp")]
		public bool ShowTimestamp { get; set; }
	}
}
