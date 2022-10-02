using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Toll-free and Fee-based Toll Call phone number information.
	/// </summary>
	public class AudioConferencingPhoneNumberInformation
	{
		/// <summary>
		/// Gets or sets the phone number's E.164 country calling code.
		/// </summary>
		[JsonPropertyName("code")]
		public string CallingCode { get; set; }

		/// <summary>
		/// Gets or sets the phone number's E.164 country calling code.
		/// </summary>
		[JsonPropertyName("country_code")]
		public Country CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the name of the country.
		/// </summary>
		[JsonPropertyName("country_name")]
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the phone number display number.
		/// </summary>
		[JsonPropertyName("display_number")]
		public string DisplayNumber { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonPropertyName("number")]
		public string Number { get; set; }
	}
}
