using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS history information.
	/// </summary>
	public class SmsHistory
	{
		/// <summary>
		/// Gets or sets the SMS attachment array.
		/// </summary>
		[JsonPropertyName("attachments")]
		public SmsAttachment[] Attachments { get; set; }

		/// <summary>
		/// Gets or sets the UTC time when the message was created.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets the SMS direction.
		/// </summary>
		[JsonPropertyName("direction")]
		public SmsDirection Direction { get; set; }

		/// <summary>
		/// Gets or sets the SMS text contents.
		/// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the SMS message ID.
		/// </summary>
		[JsonPropertyName("message_id")]
		public string MessageId { get; set; }

		/// <summary>
		/// Gets or sets the SMS message type.
		/// </summary>
		[JsonPropertyName("message_type")]
		public SmsType MessageType { get; set; }

		/// <summary>
		/// Gets or sets the SMS sender.
		/// </summary>
		[JsonPropertyName("sender")]
		public SmsHistoryParticipant Sender { get; set; }

		/// <summary>
		/// Gets or sets the SMS receivers.
		/// </summary>
		[JsonPropertyName("to_members")]
		public SmsHistoryParticipant[] ToMembers { get; set; }
	}
}
