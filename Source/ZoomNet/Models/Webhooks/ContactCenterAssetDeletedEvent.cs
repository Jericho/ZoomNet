using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the asset is deleted.
	/// </summary>
	public class ContactCenterAssetDeletedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about the deleted asset.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAssetDeleted Asset { get; set; }
	}
}
