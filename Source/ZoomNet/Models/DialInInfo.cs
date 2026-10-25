using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Dial-in information.</summary>
	public record DialInInfo
	{
		/// <summary>Gets or sets the dial-in city.</summary>
		[JsonPropertyName("city")]
		public string City { get; init; }

		/// <summary>Gets or sets the country code.</summary>
		[JsonPropertyName("country")]
		public string CountryCode { get; init; }

		/// <summary>Gets or sets the name of the country.</summary>
		[JsonPropertyName("country_name")]
		public string CountryName { get; init; }

		/// <summary>Gets or sets the dial-in phone number.</summary>
		[JsonPropertyName("number")]
		public string Number { get; init; }

		/// <summary>Gets or sets the dial-in number type.</summary>
		[JsonPropertyName("type")]
		public string Type { get; init; }
	}
}
