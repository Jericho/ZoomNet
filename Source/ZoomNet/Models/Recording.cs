using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording.
	/// </summary>
	public class Recording
	{
		/// <summary>Gets or sets the unique id.</summary>
		/// <value>The unique id.</value>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>Gets or sets the recording id.</summary>
		/// <value>The id.</value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>Gets or sets the ID of the user account.</summary>
		/// <value>The account id.</value>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the ID of the user who is set as the host of the meeting.</summary>
		/// <value>The user id.</value>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>Gets or sets the topic of the meeting.</summary>
		/// <value>The topic.</value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

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
		/// <remarks>This is available only when the "Record a seperate audio file of each participant" setting is enabled.</remarks>
		[JsonPropertyName("participant_audio_files")]
		public RecordingFile[] ParticipantAudioFiles { get; set; }

		/// <summary>Gets or sets the type of recorded meeting or webinar.</summary>
		[JsonPropertyName("type")]
		public RecordingType Type { get; set; }
	}
}
