using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Details of a sharing or recording by a meeting participant.</summary>
	public class SharingAndRecordingDetail
	{
		/// <summary>Gets or sets the type of content shared.</summary>
		[JsonPropertyName("content")]
		public string Content { get; set; }

		/// <summary>Gets or sets the end time of the sharing.</summary>
		[JsonPropertyName("end_time")]
		public string EndTime { get; set; }

		/// <summary>Gets or sets the start time of the sharing.</summary>
		[JsonPropertyName("start_time")]
		public string StartTime { get; set; }
	}
}
