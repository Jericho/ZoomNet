using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A ssponsor tier for an event.
	/// </summary>
	public class SponsorTier
	{
		/// <summary>Gets or sets the unique identifier for the sponsor tier.</summary>
		[JsonPropertyName("tier_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the sponsor tier.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the description of the sponsor tier.</summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }
	}
}
