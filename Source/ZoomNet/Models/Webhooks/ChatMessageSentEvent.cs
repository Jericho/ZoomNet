using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents event related to meeting or webinar chat message sending.</summary>
	public abstract record ChatMessageSentEvent : Event
	{
		/// <summary>Gets or sets the account id of the user who created the meeting or webinar.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the chat message information.</summary>
		public WebhookChatMessage Message { get; init; }

		/// <summary>Gets or sets the chat message sender.</summary>
		public ChatMessageParty Sender { get; init; }

		/// <summary>Gets or sets the chat message recipient.</summary>
		public ChatMessageParty Recipient { get; init; }
	}
}
