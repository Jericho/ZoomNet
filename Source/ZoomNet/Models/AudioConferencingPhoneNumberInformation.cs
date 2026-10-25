using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Toll-free and Fee-based Toll Call phone number information.</summary>
	public record AudioConferencingPhoneNumberInformation
	{
		/// <summary>Gets or sets the phone number's E.164 country calling code.</summary>
		[JsonPropertyName("code")]
		public string CallingCode { get; init; }

		/// <summary>Gets or sets the phone number's E.164 country calling code.</summary>
		[JsonPropertyName("country_code")]
		public Country CountryCode { get; init; }

		/// <summary>Gets or sets the name of the country.</summary>
		[JsonPropertyName("country_name")]
		public string CountryName { get; init; }

		/// <summary>Gets or sets the phone number display number.</summary>
		[JsonPropertyName("display_number")]
		public string DisplayNumber { get; init; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("number")]
		public string Number { get; init; }
	}
}
