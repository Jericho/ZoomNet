using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording.
	/// </summary>
	public class Recording : MeetingBasicInfo
	{
		/// <summary>Gets or sets the ID of the user account.</summary>
		/// <value>The account id.</value>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the date and time when the meeting started.</summary>
		/// <value>The start time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the meeting duration.</summary>
		/// <value> The duration.</value>
		[JsonPropertyName("duration")]
		public long Duration { get; set; }

		/// <summary>Gets or sets the total size of the recording files and audio files.</summary>
		/// <value>The total size.</value>
		[JsonPropertyName("total_size")]
		public long TotalSize { get; set; }

		/// <summary>Gets or sets the total number of recording files and audio files.</summary>
		/// <value>The number of files.</value>
		[JsonPropertyName("recording_count")]
		public long FilesCount { get; set; }

		/// <summary>Gets or sets the recording files.</summary>
		[JsonPropertyName("recording_files")]
		public RecordingFile[] RecordingFiles { get; set; }

		/// <summary>Gets or sets the URL which can be used to share the recording.</summary>
		/// <value>The play URL.</value>
		[JsonPropertyName("share_url")]
		public string ShareUrl { get; set; }

		/// <summary>Gets or sets the password of the sharing recording file.</summary>
		/// <value>The password.</value>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>Gets or sets the audio files for each participant.</summary>
		/// <remarks>This is available only when the "Record a separate audio file of each participant" setting is enabled.</remarks>
		[JsonPropertyName("participant_audio_files")]
		public RecordingFile[] ParticipantAudioFiles { get; set; }

		/// <summary>Gets or sets the type of recorded meeting or webinar.</summary>
		[JsonPropertyName("type")]
		public RecordingType Type { get; set; }

		/// <summary>Gets or sets the cloud recording's passcode to be used in the URL. Directly splice this recording's passcode in play_url or share_url with ?pwd= to access and play.</summary>
		[JsonPropertyName("recording_play_passcode")]
		public string PlayPasscode { get; set; }

		/// <summary>Gets or sets the token to download the meeting's recording.</summary>
		[JsonPropertyName("download_access_token")]
		public string DownloadAccessToken { get; set; }
	}
}
