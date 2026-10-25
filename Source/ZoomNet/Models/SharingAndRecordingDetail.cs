using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Details of a sharing or recording by a meeting participant.</summary>
	public record SharingAndRecordingDetail
	{
		/// <summary>Gets or sets the type of content shared.</summary>
		[JsonPropertyName("content")]
		public string Content { get; init; }

		/// <summary>Gets or sets the end time of the sharing.</summary>
		[JsonPropertyName("end_time")]
		public string EndTime { get; init; }

		/// <summary>Gets or sets the start time of the sharing.</summary>
		[JsonPropertyName("start_time")]
		public string StartTime { get; init; }
	}
}
