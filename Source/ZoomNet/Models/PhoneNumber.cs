using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone number.
	/// </summary>
	public class PhoneNumber
	{
		/// <summary>Gets or sets the country code. For example, for United States phone numbers, this will be a +1 value.</summary>
		[JsonPropertyName("code")]
		public string CountryCode { get; set; }

		/// <summary>Gets or sets the country.</summary>
		[JsonPropertyName("country")]
		public Country Country { get; set; }

		/// <summary>Gets or sets the type.</summary>
		[JsonPropertyName("label")]
		public PhoneType Type { get; set; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("number")]
		public string Number { get; set; }

		/// <summary>Gets or sets a value indicating whether Zoom has verified the phone number.</summary>
		[JsonPropertyName("verified")]
		public bool IsVerified { get; set; }
	}
}
