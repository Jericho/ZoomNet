using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents event related to meeting or webinar chat message file downloading.
	/// </summary>
	public abstract class ChatMessageFileDownloadedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who downloaded the file.
		/// The value is blank for external users.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user who downloaded the file.
		/// The value contains operator name for external users.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who downloaded the file.
		/// The value is blank for external users.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar host account id.
		/// The value is blank if this info is from an external user.
		/// </summary>
		public string HostAccountId { get; set; }

		/// <summary>
		/// Gets or sets the chat message file information.
		/// </summary>
		public ChatMessageFile File { get; set; }
	}
}
