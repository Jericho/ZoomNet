using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>Represents a phone number used in a phone call.</summary>
public class PhoneCallPhoneNumber
{
	/// <summary>Gets or sets the phone number ID.</summary>
	[JsonPropertyName("id")]
	public string PhoneNumberId { get; set; }

	/// <summary>Gets or sets the phone number.</summary>
	[JsonPropertyName("number")]
	public string PhoneNumber { get; set; }
}
