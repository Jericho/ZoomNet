using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a new asset is created.
	/// </summary>
	public class ContactCenterAssetCreatedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about created asset.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAssetCreated Asset { get; set; }
	}
}
