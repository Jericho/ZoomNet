using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Recording user settings.
	/// </summary>
	public class RecordingUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to enable local recording.
		/// </summary>
		[JsonProperty(PropertyName = "local_recording")]
		public bool EnableLocalRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable cloud recording.
		/// </summary>
		[JsonProperty(PropertyName = "cloud_recording")]
		public bool EnableCloudRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to record the active speaker view.
		/// </summary>
		[JsonProperty(PropertyName = "record_speaker_view")]
		public bool RecordSpeakerView { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to record the gallery view.
		/// </summary>
		[JsonProperty(PropertyName = "record_gallery_view")]
		public bool RecordGalleryView { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to record an audio only file.
		/// </summary>
		[JsonProperty(PropertyName = "record_audio_file")]
		public bool RecordAudioOnly { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to save chat text from the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "save_chat_text")]
		public bool SaveChatText { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show timestamp on video.
		/// </summary>
		[JsonProperty(PropertyName = "show_timestamp")]
		public bool ShowTimestamp { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to audio transcript (???).
		/// </summary>
		[JsonProperty(PropertyName = "recording_audio_transcript")]
		public bool AudioTranscript { get; set; }

		/// <summary>
		/// Gets or sets the auto recording.
		/// </summary>
		[JsonProperty(PropertyName = "auto_recording")]
		public RecordingType AutoRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow the host to pause/stop the auto recording in the cloud.
		/// </summary>
		[JsonProperty(PropertyName = "host_pause_stop_recording")]
		public bool AllowHostStopCloudRecording { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to automatically delete recording in the cloud.
		/// </summary>
		[JsonProperty(PropertyName = "auto_delete_cmr")]
		public bool AutoDeleteCloudRecordings { get; set; }

		/// <summary>
		/// Gets or sets the number of days to keep a recording before automatically deleting it.
		/// </summary>
		[JsonProperty(PropertyName = "auto_delete_cmr_days")]
		public int NumberOfDaysToKeepCloudRecordings { get; set; }

		/// <summary>
		/// Gets or sets the password requirements.
		/// </summary>
		[JsonProperty(PropertyName = "recording_password_requirement")]
		public PasswordRequirements PasswordRequirements { get; set; }
	}
}
