using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Peering number update model used in webhooks.
	/// </summary>
	public class WebhookPeeringNumberUpdate
	{
		/// <summary>
		/// Gets or sets the carrier code.
		/// </summary>
		[JsonPropertyName("carrier_code")]
		public int CarrierCode { get; set; }

		/// <summary>
		/// Gets or sets phone numbers with updates.
		/// </summary>
		[JsonPropertyName("phone_numbers")]
		public string[] PhoneNumbers { get; set; }

		/// <summary>
		/// Gets or sets the caller id name (CNAM).
		/// </summary>
		[JsonPropertyName("cnam")]
		public string CallerIdName { get; set; }

		/// <summary>
		/// Gets or sets the emergency address.
		/// </summary>
		[JsonPropertyName("emergency_address")]
		public EmergencyAddress EmergencyAddress { get; set; }
	}
}
