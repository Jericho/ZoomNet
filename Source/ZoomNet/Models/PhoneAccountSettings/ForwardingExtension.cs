using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Call forwarding extension information.</summary>
	public record ForwardingExtension
	{
		/// <summary>Gets or sets the extension's name.</summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; init; }

		/// <summary>Gets or sets the extension id.</summary>
		[JsonPropertyName("extension_id")]
		public string ExtensionId { get; init; }

		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public long ExtensionNumber { get; init; }

		/// <summary>Gets or sets the type of extension.</summary>
		[JsonPropertyName("extension_type")]
		public ForwardingExtensionType ExtensionType { get; init; }

		/// <summary>Gets or sets the ID of the extension.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }
	}
}
