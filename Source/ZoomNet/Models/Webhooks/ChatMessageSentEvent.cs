using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents event related to meeting or webinar chat message sending.
	/// </summary>
	public abstract class ChatMessageSentEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the meeting or webinar.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the chat message information.
		/// </summary>
		public WebhookChatMessage Message { get; set; }

		/// <summary>
		/// Gets or sets the chat message sender.
		/// </summary>
		public ChatMessageParty Sender { get; set; }

		/// <summary>
		/// Gets or sets the chat message recipient.
		/// </summary>
		public ChatMessageParty Recipient { get; set; }
	}
}
