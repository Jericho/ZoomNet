using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A live stream settings.
	/// </summary>
	public class LiveStreamingSettings
	{
		/// <summary>
		/// Gets or sets the stream URL.
		/// </summary>
		[JsonPropertyName("stream_url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the stream key.
		/// </summary>
		[JsonPropertyName("stream_key")]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the stream page URL.
		/// </summary>
		[JsonPropertyName("page_url")]
		public string PageUrl { get; set; }

		/// <summary>
		/// Gets or sets the number of pixels in each dimension that the video camera can display.
		/// </summary>
		[JsonPropertyName("resolution")]
		public string Resolution { get; set; }
	}
}
