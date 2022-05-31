using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Virtual background file.
	/// </summary>
	public class VirtualBackgroundFile
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets a value indicating whether this file is the default virtual background.</summary>
		[JsonPropertyName("is_default")]
		public bool IsDefault { get; set; }

		/// <summary>Gets or sets name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the size.</summary>
		[JsonPropertyName("size")]
		public int Size { get; set; }

		/// <summary>Gets or sets the type.</summary>
		[JsonPropertyName("type")]
		public VirtualBackgroundType Type { get; set; }
	}
}
