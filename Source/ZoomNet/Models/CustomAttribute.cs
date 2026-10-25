using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Custom attribute.</summary>
	public record CustomAttribute
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("key")]
		public string Key { get; init; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the value.</summary>
		[JsonPropertyName("value")]
		public string Value { get; init; }
	}
}
