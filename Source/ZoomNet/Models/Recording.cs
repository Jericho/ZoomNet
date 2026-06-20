using System;
using System.Linq;
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

		/// <summary>
		/// Calculates the total duration covered by all recording files.
		/// </summary>
		/// <remarks>The total duration is determined by subtracting the earliest start time from the latest end time
		/// across all available recording files. If any recording file does not have an end time, it is excluded from the
		/// calculation.</remarks>
		/// <returns>A <see cref="TimeSpan"/> representing the time span from the earliest start time to the latest end time among all
		/// recording files. Returns <see cref="TimeSpan.Zero"/> if there are no recording files or if no end time is
		/// available.</returns>
		public TimeSpan CalculateTotalDuration()
		{
			if (RecordingFiles != null && RecordingFiles.Length > 0)
			{
				var start = RecordingFiles.Min(f => f.StartTime);
				var end = RecordingFiles.Max(f => f.EndTime);

				if (end.HasValue) return end.Value - start;
			}

			return TimeSpan.Zero;
		}

		/// <summary>
		/// Retrieves the primary video recording file from the collection.
		/// </summary>
		/// <remarks>If multiple files are marked as primary, only the first is returned. Returns null if the
		/// collection is empty or contains no primary video.</remarks>
		/// <returns>The primary video recording file if one exists; otherwise, null.</returns>
		public RecordingFile GetPrimaryVideo()
		{
			return RecordingFiles?.FirstOrDefault(f => f.IsPrimaryVideo);
		}

		/// <summary>
		/// Retrieves the transcript or closed-caption file associated with the recording.
		/// If both a transcript and a closed-caption file are available, the transcript file is returned.
		/// If only one of them is available, that file is returned.
		/// If neither is available, null is returned.
		/// </summary>
		/// <returns>A <see cref="RecordingFile"/> representing the transcript file if one exists; otherwise, <c>null</c>.</returns>
		public RecordingFile GetTranscript()
		{
			var file = RecordingFiles?.FirstOrDefault(f => f.FileType is RecordingFileType.Transcript);
			if (file == null) file = RecordingFiles?.FirstOrDefault(f => f.FileType is RecordingFileType.ClosedCaptioning);
			return file;
		}

		/// <summary>
		/// Gets the first audio-only recording file from the collection, if available.
		/// </summary>
		/// <returns>A <see cref="RecordingFile"/> representing the first audio-only file in the collection; or <see langword="null"/>
		/// if no audio-only file exists.</returns>
		public RecordingFile GetAudioOnly()
		{
			return RecordingFiles?.FirstOrDefault(f => f.FileType is RecordingFileType.Audio);
		}

		/// <summary>
		/// Gets the first chat log recording file, if available.
		/// </summary>
		/// <returns>A <see cref="RecordingFile"/> representing the first file with a chat or chat message type; or <see
		/// langword="null"/> if no such file exists.</returns>
		public RecordingFile GetChatLog()
		{
			return RecordingFiles?.FirstOrDefault(f => f.FileType is RecordingFileType.Chat or RecordingFileType.ChatMessage);
		}

		/// <summary>
		/// Gets the first recording file of type Timeline from the collection.
		/// </summary>
		/// <returns>A <see cref="RecordingFile"/> representing the first file with a file type of Timeline, or <see langword="null"/>
		/// if no such file exists.</returns>
		public RecordingFile GetTimeline()
		{
			return RecordingFiles?.FirstOrDefault(f => f.FileType is RecordingFileType.Timeline);
		}
	}
}
