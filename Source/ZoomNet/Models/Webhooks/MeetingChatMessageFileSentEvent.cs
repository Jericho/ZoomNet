using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting's message file is available to view or download.
	/// </summary>
	public class MeetingChatMessageFileSentEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who hosted the meeting.
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
		/// Gets or sets the chat message file information.
		/// </summary>
		public ChatMessageFile File { get; set; }

		/// <summary>
		/// Gets or sets the chat message information.
		/// </summary>
		public WebhookChatMessage Message { get; set; }

		/// <summary>
		/// Gets or sets the chat message file sender.
		/// </summary>
		public ChatMessageParty Sender { get;set; }

		/// <summary>
		/// Gets or sets the chat message file recipient.
		/// </summary>
		public ChatMessageParty Recipient { get; set; }
	}
}
