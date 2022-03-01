using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// User phone number.
	/// </summary>
	public class UserPhoneNumber
	{
		/// <summary>Gets or sets the country code. For example, for United States phone numbers, this will be a +1 value.</summary>
		[JsonProperty("code")]
		public string CountryCode { get; set; }

		/// <summary>Gets or sets the country.</summary>
		[JsonProperty(PropertyName = "country")]
		public Country Country { get; set; }

		/// <summary>Gets or sets the type.</summary>
		[JsonProperty(PropertyName = "label")]
		public UserPhoneType Type { get; set; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonProperty(PropertyName = "number")]
		public string Number { get; set; }

		/// <summary>Gets or sets a value indicating whether Zoom has verified the phone number.</summary>
		[JsonProperty(PropertyName = "verified")]
		public bool IsVerified { get; set; }
	}
}
