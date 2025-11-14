using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Represents an emergency address.
/// </summary>
public class EmergencyAddress
{
	/// <summary>
	/// Gets or sets the first line of the address.
	/// </summary>
	[JsonPropertyName("address_line1")]
	public string AddressLine1 { get; set; }

	/// <summary>
	/// Gets or sets the second line of the address.
	/// </summary>
	[JsonPropertyName("address_line2")]
	public string AddressLine2 { get; set; }

	/// <summary>
	/// Gets or sets the city.
	/// </summary>
	[JsonPropertyName("city")]
	public string City { get; set; }

	/// <summary>
	/// Gets or sets the country.
	/// </summary>
	[JsonPropertyName("country")]
	public string Country { get; set; }

	/// <summary>
	/// Gets or sets the emergency address id.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	/// Gets or sets the state code.
	/// </summary>
	[JsonPropertyName("state_code")]
	public string StateCode { get; set; }

	/// <summary>
	/// Gets or sets the postal code.
	/// </summary>
	[JsonPropertyName("zip")]
	public string Zip { get; set; }
}
