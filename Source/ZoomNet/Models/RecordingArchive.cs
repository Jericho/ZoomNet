using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about archived recordings.
	/// </summary>
	public class RecordingArchive : RecordedMeetingOrWebinarInfo
	{
		/// <summary>
		/// Gets or sets the user's account name.
		/// </summary>
		[JsonPropertyName("account_name")]
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets the total size of archive in bytes.
		/// </summary>
		[JsonPropertyName("total_size")]
		public long TotalSize { get; set; }

		/// <summary>
		/// Gets or sets the number of archived files.
		/// </summary>
		[JsonPropertyName("recording_count")]
		public int FilesCount { get; set; }

		/// <summary>
		/// Gets or sets information about archived files.
		/// </summary>
		[JsonPropertyName("archive_files")]
		public RecordingArchiveFile[] Files { get; set; }

		/// <summary>
		/// Gets or sets the archive processing status.
		/// </summary>
		[JsonPropertyName("status")]
		public RecordingArchiveFileStatus Status { get; set; }

		/// <summary>
		/// Gets or sets whether the meeting or webinar is internal or external.
		/// </summary>
		[JsonPropertyName("meeting_type")]
		public MeetingKind MeetingKind { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar archive completion time.
		/// </summary>
		[JsonPropertyName("complete_time")]
		public DateTime? CompleteTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar duration in seconds.
		/// </summary>
		[JsonPropertyName("duration_in_second")]
		public int DurationInSeconds { get; set; }

		/// <summary>
		/// Gets or sets primary group IDs (comma-separated) of participants who belong to the account.
		/// </summary>
		[JsonPropertyName("group_id")]
		public string GroupId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether recording archive is related to breakout room.
		/// </summary>
		[JsonPropertyName("is_breakout_room")]
		public bool? IsBreakoutRoom { get; set; }

		/// <summary>
		/// Gets or sets the parent meeting uuid.
		/// </summary>
		/// <remarks>
		/// This field is returned only if <see cref="IsBreakoutRoom"/> is TRUE.
		/// </remarks>
		[JsonPropertyName("parent_meeting_id")]
		public string ParentMeetingUuid { get; set; }
	}
}
