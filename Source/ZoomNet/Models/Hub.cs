using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A hub.
	/// </summary>
	public class Hub
	{
		/// <summary>Gets or sets the hub id.</summary>
		[JsonPropertyName("hub_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the access level.</summary>
		[JsonPropertyName("access_level")]
		public string AccessLevel { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the description</summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the public URL.</summary>
		[JsonPropertyName("public_url")]
		public string PublicUrl { get; set; }

		/// <summary>Gets or sets a value indicating whether the hub is hidden or not.</summary>
		[JsonPropertyName("hidden_hub")]
		public bool IsHidden { get; set; }

		/// <summary>Gets or sets a value indicating whether the hub is active or not.</summary>
		[JsonPropertyName("hub_active")]
		public bool IsActive { get; set; }

		/// <summary>Gets or sets a value indicating whether the events in the hub are automatically listed.</summary>
		[JsonPropertyName("auto_list_events")]
		public bool AutomaticallyListEvents { get; set; }

		/// <summary>Gets or sets a value indicating whether the hub is a landing hub.</summary>
		[JsonPropertyName("landing_hub")]
		public bool IsLandingHub { get; set; }
	}
}
