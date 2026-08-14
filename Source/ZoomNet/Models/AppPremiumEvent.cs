using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a premium event for an app.
	/// </summary>
	public class AppPremiumEvent
	{
		/// <summary>Gets or sets the name of the premium event.</summary>
		[JsonPropertyName("event_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the id of the premium event.</summary>
		[JsonPropertyName("event")]
		public string Id { get; set; }
	}
}
