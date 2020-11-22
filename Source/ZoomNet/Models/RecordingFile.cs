using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording file.
	/// </summary>
	public class RecordingFile
	{
		/// <summary>Gets or sets the recording file id.</summary>
		/// <value>The id.</value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>Gets or sets the ID of the meeting.</summary>
		/// <value>The meeting id.</value>
		[JsonProperty("meeting_id", NullValueHandling = NullValueHandling.Ignore)]
		public long MeetingId { get; set; }

		/// <summary>Gets or sets the date and time when the recording started.</summary>
		/// <value>The start time.</value>
		[JsonProperty(PropertyName = "recording_start", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the date and time when the recording ended.</summary>
		/// <value>The end time.</value>
		[JsonProperty(PropertyName = "recording_end", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime EndTime { get; set; }

		/// <summary>Gets or sets the file type.</summary>
		/// <value>The file type.</value>
		[JsonProperty("file_type", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingFileType FileType { get; set; }

		/// <summary>Gets or sets the size of the file.</summary>
		/// <value>The size.</value>
		[JsonProperty(PropertyName = "file_size", NullValueHandling = NullValueHandling.Ignore)]
		public long Size { get; set; }

		/// <summary>Gets or sets the URL which can be used to play the file.</summary>
		/// <value>The play URL.</value>
		[JsonProperty(PropertyName = "play_url", NullValueHandling = NullValueHandling.Ignore)]
		public string PlayUrl { get; set; }

		/// <summary>Gets or sets the URL which can be used to download the file.</summary>
		/// <value>The download URL.</value>
		[JsonProperty(PropertyName = "download_url", NullValueHandling = NullValueHandling.Ignore)]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the recording status.</summary>
		/// <value>The status.</value>
		[JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
		public string Status { get; set; }

		/// <summary>Gets or sets the date and time when the recording was deleted.</summary>
		/// <value>The delete time.</value>
		/// <remarks>Returned in the response only for trash query.</remarks>
		[JsonProperty(PropertyName = "deleted_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? DeleteTime { get; set; }

		/// <summary>Gets or sets the content type.</summary>
		/// <value>The content type.</value>
		[JsonProperty("recording_type", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingContentType ContentType { get; set; }
	}
}
