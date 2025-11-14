using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording.
	/// </summary>
	public class Recording : RecordedMeetingOrWebinarInfo
	{
		/// <summary>
		/// Gets or sets the ID of the user account who completed the recording.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the host's email.
		/// </summary>
		[JsonPropertyName("host_email")]
		public string HostEmail { get; set; }

		/// <summary>
		/// Gets or sets the total size of the recording files and audio files in bytes.
		/// </summary>
		[JsonPropertyName("total_size")]
		public long TotalSize { get; set; }

		/// <summary>
		/// Gets or sets the total number of recording files and audio files.
		/// </summary>
		[JsonPropertyName("recording_count")]
		public int FilesCount { get; set; }

		/// <summary>
		/// Gets or sets the recording files.
		/// </summary>
		[JsonPropertyName("recording_files")]
		public RecordingFile[] RecordingFiles { get; set; }

		/// <summary>
		/// Gets or sets the URL which can be used to share the recording.
		/// </summary>
		[JsonPropertyName("share_url")]
		public string ShareUrl { get; set; }

		/// <summary>
		/// Gets or sets the password of the sharing recording file.
		/// </summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the audio files for each participant.
		/// </summary>
		/// <remarks>
		/// This is available only when the "Record a separate audio file of each participant" setting is enabled.
		/// </remarks>
		[JsonPropertyName("participant_audio_files")]
		public RecordingFile[] ParticipantAudioFiles { get; set; }

		/// <summary>
		/// Gets or sets the cloud recording's passcode to be used in the URL.
		/// </summary>
		/// <remarks>
		/// Directly splice recording's passcode in play_url or share_url with ?pwd= to access and play.
		/// </remarks>
		[JsonPropertyName("recording_play_passcode")]
		public string PlayPasscode { get; set; }

		/// <summary>
		/// Gets or sets the token to download the meeting's recording.
		/// </summary>
		[JsonPropertyName("download_access_token")]
		public string DownloadAccessToken { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether cloud recording deletion is enabled.
		/// </summary>
		/// <remarks>
		/// To get the auto-delete status, the host of the recording must have
		/// the recording setting 'Delete cloud recordings after a specified number of days' enabled.
		/// </remarks>
		[JsonPropertyName("auto_delete")]
		public bool? AutoDelete { get; set; }

		/// <summary>
		/// Gets or sets the date on which the recording will be auto-deleted.
		/// </summary>
		/// <remarks>
		/// This value is returned only if <see cref="AutoDelete"/> is TRUE.
		/// </remarks>
		[JsonPropertyName("auto_delete_date")]
		public string AutoDeleteDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the recording is an on-premise recording.
		/// </summary>
		[JsonPropertyName("on_prem")]
		public bool? OnPremise { get; set; }
	}
}
