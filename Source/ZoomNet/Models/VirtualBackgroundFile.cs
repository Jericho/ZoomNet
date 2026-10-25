using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Virtual background file.</summary>
	public record VirtualBackgroundFile
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets a value indicating whether this file is the default virtual background.</summary>
		[JsonPropertyName("is_default")]
		public bool IsDefault { get; init; }

		/// <summary>Gets or sets name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the size.</summary>
		[JsonPropertyName("size")]
		public int Size { get; init; }

		/// <summary>Gets or sets the type.</summary>
		[JsonPropertyName("type")]
		public VirtualBackgroundType Type { get; init; }
	}
}
