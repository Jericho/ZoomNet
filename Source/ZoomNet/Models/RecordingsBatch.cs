using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the meeting or webinar recordings.
	/// </summary>
	public class RecordingsBatch
	{
		/// <summary>
		/// Gets or sets the uuid of the recorded meeting or webinar instance.
		/// </summary>
		[JsonPropertyName("meeting_uuid")]
		public string MeetingUuid { get; set; }

		/// <summary>
		/// Gets or sets the recording files ids.
		/// </summary>
		/// <remarks>
		/// NULL value indicates all meeting or webinar files.
		/// </remarks>
		[JsonPropertyName("recording_file_ids")]
		public string[] RecordingFileIds { get; set; }
	}
}
