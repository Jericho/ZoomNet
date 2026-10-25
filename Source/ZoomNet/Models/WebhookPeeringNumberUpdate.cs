using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Peering number update model used in webhooks.</summary>
	public record WebhookPeeringNumberUpdate
	{
		/// <summary>Gets or sets the carrier code.</summary>
		[JsonPropertyName("carrier_code")]
		public int CarrierCode { get; init; }

		/// <summary>Gets or sets the caller id name (CNAM).</summary>
		[JsonPropertyName("cnam")]
		public string CallerIdName { get; init; }

		/// <summary>Gets or sets the emergency address.</summary>
		[JsonPropertyName("emergency_address")]
		public EmergencyAddress EmergencyAddress { get; init; }

		/// <summary>Gets or sets phone numbers with updates.</summary>
		[JsonPropertyName("phone_numbers")]
		public string[] PhoneNumbers { get; init; }
	}
}
