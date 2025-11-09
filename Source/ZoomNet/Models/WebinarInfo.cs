using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about webinar.
	/// </summary>
	public class WebinarInfo : WebinarBasicInfo
	{
		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the webinar.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the webinar.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the webinar type.
		/// </summary>
		[JsonPropertyName("type")]
		public WebinarType Type { get; set; }

		/// <summary>
		/// Gets or sets the webinar start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the webinar timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>
		/// Gets or sets the scheduled webinar duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }
	}
}
