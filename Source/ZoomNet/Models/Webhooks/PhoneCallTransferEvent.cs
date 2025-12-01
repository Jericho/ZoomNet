using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the phone call transfer.
	/// </summary>
	public abstract class PhoneCallTransferEvent : Event
	{
		/// <summary>
		/// Gets or sets user's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets call transfer information.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookCallTransferInfo CallTransfer { get; set; }
	}
}
