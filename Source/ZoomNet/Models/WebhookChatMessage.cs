using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat message information as received in meeting or webhook chat message webhook events.
	/// </summary>
	public class WebhookChatMessage
	{
		/// <summary>
		/// Gets or sets the chat message uuid.
		/// </summary>
		[JsonPropertyName("message_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the chat message was sent.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the content of the chat message.
		/// </summary>
		/// <remarks>
		/// Available only in <see cref="Webhooks.MeetingChatMessageSentEvent"/> and
		/// <see cref="Webhooks.WebinarChatMessageSentEvent"/>.
		/// </remarks>
		[JsonPropertyName("message_content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the chat file uuids, in base64 encoded format.
		/// </summary>
		/// <remarks>
		/// Available only in <see cref="Webhooks.MeetingChatMessageSentEvent"/> and
		/// <see cref="Webhooks.WebinarChatMessageSentEvent"/>.
		/// </remarks>
		[JsonPropertyName("file_ids")]
		public string[] FileIds { get; set; }
	}
}
