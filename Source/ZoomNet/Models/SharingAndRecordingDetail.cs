using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a sharing or recording by a meeting participant.
	/// </summary>
	public class SharingAndRecordingDetail
	{
		/// <summary>
		/// Gets or sets the type of content shared.
		/// </summary>
		/// <value>
		/// The type of content of the sharing/recording.
		/// </value>
		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the start time of the sharing.
		/// </summary>
		/// <value>
		/// The start time of the sharing.
		/// </value>
		[JsonProperty(PropertyName = "start_time")]
		public string StartTime { get; set; }

		/// <summary>
		/// Gets or sets the end time of the sharing.
		/// </summary>
		/// <value>
		/// The end time of the sharing.
		/// </value>
		[JsonProperty(PropertyName = "end_time")]
		public string EndTime { get; set; }
	}
}
