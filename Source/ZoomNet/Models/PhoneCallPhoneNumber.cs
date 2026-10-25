using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>Represents a phone number used in a phone call.</summary>
public record PhoneCallPhoneNumber
{
	/// <summary>Gets or sets the phone number ID.</summary>
	[JsonPropertyName("id")]
	public string PhoneNumberId { get; init; }

	/// <summary>Gets or sets the phone number.</summary>
	[JsonPropertyName("number")]
	public string PhoneNumber { get; init; }
}
