using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>
	/// Quality of service data for video.
	/// </summary>
	public class VideoQualityOfServiceMetrics : PacketQualityOfServiceMetrics
	{
		/// <summary>
		/// Gets or sets the resolution value.
		/// </summary>
		/// <value>
		/// The video resolution.
		/// </value>
		[JsonPropertyName("resolution")]
		public string Resolution { get; set; }

		/// <summary>
		/// Gets or sets the frame rate value.
		/// </summary>
		/// <value>
		/// The frame rate.
		/// </value>
		[JsonPropertyName("frame_rate")]
		public string FrameRate { get; set; }
	}
}
