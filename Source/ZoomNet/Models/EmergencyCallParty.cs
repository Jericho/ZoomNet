using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Information about emergency call party, i.e. caller or callee.</summary>
	public record EmergencyCallParty
	{
		/// <summary>Gets or sets the user's display name.</summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; init; }

		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; init; }

		/// <summary>Gets or sets the extension type.</summary>
		[JsonPropertyName("extension_type")]
		public EmergencyCallExtensionType? ExtensionType { get; init; }

		/// <summary>Gets or sets Zoom user id.</summary>
		[JsonPropertyName("id")]
		public string UserId { get; init; }

		/// <summary>Gets or sets the phone number in E.164 format.</summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; init; }

		/// <summary>Gets or sets site id.</summary>
		[JsonPropertyName("site_id")]
		public string SiteId { get; init; }

		/// <summary>Gets or sets site name.</summary>
		[JsonPropertyName("site_name")]
		public string SiteName { get; init; }

		/// <summary>Gets or sets user's timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones? Timezone { get; init; }
	}
}
