using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the TSP (Telephony Service Provider) dial-in number.
	/// </summary>
	public class DialInNumber
	{
		/// <summary>
		/// Gets or sets the country code (max length 6).
		/// </summary>
		[JsonPropertyName("code")]
		public string CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the TSP country label (max length 10).
		/// </summary>
		[JsonPropertyName("country_label")]
		public string CountryLabel { get; set; }

		/// <summary>
		/// Gets or sets the dial-in phone number (max length 16).
		/// </summary>
		[JsonPropertyName("number")]
		public string Number { get; set; }

		/// <summary>
		/// Gets or sets the type of the phone number.
		/// </summary>
		[JsonPropertyName("type")]
		public PhoneNumberType Type { get; set; }
	}
}
