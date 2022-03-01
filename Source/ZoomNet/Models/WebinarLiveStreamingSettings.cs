using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Webinar live streaming settings.
	/// </summary>
	public class WebinarLiveStreamingSettings
	{
		/// <summary>Gets or sets the specific instructions to allow your account's meeting hosts to configure a custom live stream.</summary>
		[JsonProperty(PropertyName = "custom_service_instructions")]
		public string CustomServiceInstructions { get; set; }

		/// <summary>Gets or sets a value indicating whether webinar live streaming is enabled.</summary>
		[JsonProperty(PropertyName = "enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets a value indicating whether to notify users to watch the live stream. This does not apply to custom RTMP (real-time messaging protocol).</summary>
		[JsonProperty(PropertyName = "live_streaming_reminder")]
		public bool SendReminder { get; set; }

		/// <summary>Gets or sets the available live streaming services.</summary>
		[JsonProperty(PropertyName = "live_streaming_service")]
		public StreamingService[] AvailableLiveStreamingServices { get; set; }
	}
}
