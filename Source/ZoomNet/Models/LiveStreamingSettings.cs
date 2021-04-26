using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "stream_url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the stream key.
		/// </summary>
		[JsonProperty(PropertyName = "stream_key")]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the stream page URL.
		/// </summary>
		[JsonProperty(PropertyName = "page_url")]
		public string PageUrl { get; set; }
	}
}
