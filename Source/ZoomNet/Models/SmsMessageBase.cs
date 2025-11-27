using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base information about SMS message common for API endpoint and webhook events.
	/// </summary>
	public abstract class SmsMessageBase
	{
		/// <summary>
		/// Gets or sets the SMS message ID.
		/// </summary>
		[JsonPropertyName("message_id")]
		public string MessageId { get; set; }

		/// <summary>
		/// Gets or sets the SMS message type.
		/// </summary>
		[JsonPropertyName("message_type")]
		public SmsMessageType Type { get; set; }

		/// <summary>
		/// Gets or sets the UTC time when the message was created.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the SMS text contents.
		/// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the SMS attachment array.
		/// </summary>
		[JsonPropertyName("attachments")]
		public SmsAttachment[] Attachments { get; set; }
	}
}
