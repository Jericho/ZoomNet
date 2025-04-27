using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A scheduled webinar.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Webinar" />
	public class ScheduledWebinar : Webinar
	{
		/// <summary>
		/// Gets or sets the webinar start time.
		/// </summary>
		/// <value>The webinar start time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }
	}
}
