using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording file.
	/// </summary>
	public class RecordingFile
	{
		/// <summary>Gets or sets the recording file id.</summary>
		/// <value>The id.</value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the ID of the meeting.</summary>
		/// <value>The meeting id.</value>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>Gets or sets the date and time when the recording started.</summary>
		/// <value>The start time.</value>
		[JsonPropertyName("recording_start")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the date and time when the recording ended.</summary>
		/// <value>The end time.</value>
		[JsonPropertyName("recording_end")]
		public DateTime? EndTime { get; set; }

		/// <summary>Gets or sets the file type.</summary>
		/// <value>The file type.</value>
		[JsonPropertyName("file_type")]
		public RecordingFileType FileType { get; set; }

		/// <summary>Gets or sets the size of the file.</summary>
		/// <value>The size.</value>
		[JsonPropertyName("file_size")]
		public long Size { get; set; }

		/// <summary>Gets or sets the URL which can be used to play the file.</summary>
		/// <value>The play URL.</value>
		[JsonPropertyName("play_url")]
		public string PlayUrl { get; set; }

		/// <summary>Gets or sets the URL which can be used to download the file.</summary>
		/// <value>The download URL.</value>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the recording status.</summary>
		/// <value>The status.</value>
		[JsonPropertyName("status")]
		public RecordingStatus Status { get; set; }

		/// <summary>Gets or sets the date and time when the recording was deleted.</summary>
		/// <value>The delete time.</value>
		/// <remarks>Returned in the response only for trash query.</remarks>
		[JsonPropertyName("deleted_time")]
		public DateTime? DeleteTime { get; set; }

		/// <summary>Gets or sets the content type.</summary>
		/// <value>The content type.</value>
		[JsonPropertyName("recording_type")]
		public RecordingContentType ContentType { get; set; }

		/// <summary>
		/// Gets or sets the file extension of the recording file.
		/// </summary>
		[JsonPropertyName("file_extension")]
		public RecordingFileExtension FileExtension { get; set; }

		/// <summary>Gets or sets the file name.</summary>
		/// <value>The file name.</value>
		[JsonPropertyName("file_name")]
		public string FileName { get; set; }

		/// <summary>Gets or sets the file path to the on-premise account recording.</summary>
		/// <remarks>For Zoom On-Premise accounts.</remarks>
		/// <value>The file path.</value>
		[JsonPropertyName("file_path")]
		public string FilePath { get; set; }
	}
}
