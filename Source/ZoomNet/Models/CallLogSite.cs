using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Call transfer/forward information.</summary>
	public record CallLogSite
	{
		/// <summary>Gets or sets the Id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }
	}
}
