using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "code")]
		public string CallingCode { get; set; }

		/// <summary>
		/// Gets or sets the phone number's E.164 country calling code.
		/// </summary>
		[JsonProperty(PropertyName = "country_code")]
		public Country CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the name of the country.
		/// </summary>
		[JsonProperty(PropertyName = "country_name")]
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the phone number display number.
		/// </summary>
		[JsonProperty(PropertyName = "display_number")]
		public string DisplayNumber { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonProperty(PropertyName = "number")]
		public string Number { get; set; }
	}
}
