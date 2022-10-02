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
		/// For example, "America/Los_Angeles".
		/// Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.
		/// </summary>
		/// <value>The webinar timezone. For example, "America/Los_Angeles". Please reference our <a href="https://marketplace.zoom.us/docs/api-reference/other-references/abbreviation-lists#timezones">timezone list</a> for supported timezones and their formats.</value>
		[JsonPropertyName("timezone")]
		public string Timezone { get; set; }
	}
}
