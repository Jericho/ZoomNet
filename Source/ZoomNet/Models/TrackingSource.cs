using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Tracking Source.
	/// </summary>
	public class TrackingSource
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the source (platform) where the registration URL was shared.
		/// </summary>
		[JsonProperty(PropertyName = "source_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL that was shared for the registration.
		/// </summary>
		[JsonProperty(PropertyName = "tracking_url")]
		public string TrackingUrl { get; set; }

		/// <summary>
		/// Gets or sets the number of visitors who visited the registration page from this source.
		/// </summary>
		[JsonProperty(PropertyName = "visitor_count")]
		public long VisitorCount { get; set; }
	}
}
