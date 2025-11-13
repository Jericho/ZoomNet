using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording file.
	/// </summary>
	public class RecordingFile : RecordingFileBase
	{
		/// <summary>
		/// Gets or sets the ID of the meeting.
		/// </summary>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the recording started.
		/// </summary>
		[JsonPropertyName("recording_start")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the recording ended.
		/// </summary>
		[JsonPropertyName("recording_end")]
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Gets or sets the URL which can be used to play the file.
		/// </summary>
		[JsonPropertyName("play_url")]
		public string PlayUrl { get; set; }

		/// <summary>
		/// Gets or sets the recording status (completed or processing).
		/// </summary>
		[JsonPropertyName("status")]
		public RecordingStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the recording was deleted.
		/// </summary>
		/// <remarks>
		/// Returned in the response only for trash query.
		/// </remarks>
		[JsonPropertyName("deleted_time")]
		public DateTime? DeleteTime { get; set; }
	}
}
