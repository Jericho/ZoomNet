using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the peering number update.
	/// </summary>
	public abstract class PeeringNumberUpdatedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the customer using provider exchange.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets peering number update information.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookPeeringNumberUpdate PeeringNumber { get; set; }
	}
}
