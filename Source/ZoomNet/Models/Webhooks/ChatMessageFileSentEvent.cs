using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents event related to meeting or webinar chat message file sending.</summary>
	public abstract record ChatMessageFileSentEvent : Event
	{
		/// <summary>Gets or sets the account id of the user who hosted the meeting or webinar.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the chat message file information.</summary>
		public ChatMessageFile File { get; init; }

		/// <summary>Gets or sets the chat message information.</summary>
		public WebhookChatMessage Message { get; init; }

		/// <summary>Gets or sets the chat message file sender.</summary>
		public ChatMessageParty Sender { get; init; }

		/// <summary>Gets or sets the chat message file recipient.</summary>
		public ChatMessageParty Recipient { get; init; }
	}
}
