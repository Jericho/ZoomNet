using System.Text.Json.Serialization;

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
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the source (platform) where the registration URL was shared.
		/// </summary>
		[JsonPropertyName("source_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL that was shared for the registration.
		/// </summary>
		[JsonPropertyName("tracking_url")]
		public string TrackingUrl { get; set; }

		/// <summary>
		/// Gets or sets the number of registrations made from from this source.
		/// </summary>
		[JsonPropertyName("registration_count")]
		public long RegistrationCount { get; set; }

		/// <summary>
		/// Gets or sets the number of visitors who visited the registration page from this source.
		/// </summary>
		[JsonPropertyName("visitor_count")]
		public long VisitorCount { get; set; }
	}
}
