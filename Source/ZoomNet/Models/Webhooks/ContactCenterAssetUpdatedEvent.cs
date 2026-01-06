using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an asset is updated.
	/// </summary>
	public class ContactCenterAssetUpdatedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about the updated asset.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAssetUpdated Asset { get; set; }
	}
}
