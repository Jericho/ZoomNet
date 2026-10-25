using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Call transfer/forward information.</summary>
	public record CallLogOwnerInfo
	{
		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public int ExtensionNumber { get; init; }

		/// <summary>Gets or sets the owner ID.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the owner type.</summary>
		[JsonPropertyName("type")]
		public CallLogOwnerType? Type { get; init; }
	}
}
