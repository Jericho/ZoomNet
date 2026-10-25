using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Line subscription information.</summary>
	public record LineSubscription
	{
		/// <summary>Gets or sets the display name.</summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; init; }

		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public long ExtensionNumber { get; init; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; init; }
	}
}
