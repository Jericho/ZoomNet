using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Dial-in information.</summary>
	public class DialInInfo
	{
		/// <summary>Gets or sets the dial-in city.</summary>
		[JsonPropertyName("city")]
		public string City { get; set; }

		/// <summary>Gets or sets the country code.</summary>
		[JsonPropertyName("country")]
		public string CountryCode { get; set; }

		/// <summary>Gets or sets the name of the country.</summary>
		[JsonPropertyName("country_name")]
		public string CountryName { get; set; }

		/// <summary>Gets or sets the dial-in phone number.</summary>
		[JsonPropertyName("number")]
		public string Number { get; set; }

		/// <summary>Gets or sets the dial-in number type.</summary>
		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}
