using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user sends a public or private chat message during a meeting using the in-meeting Zoom chat feature.
	/// </summary>
	public class MeetingChatMessageSentEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		public long MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the meeting instance uuid.
		/// </summary>
		public string MeetingUuid { get; set; }

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
