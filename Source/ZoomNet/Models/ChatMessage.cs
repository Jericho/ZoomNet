using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat message.
	/// </summary>
	public class ChatMessage
	{
		/// <summary>
		/// Gets or sets the unique identifier of the message.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the message content.
		/// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the date and time at which the message was sent.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime SentOn { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the message in microseconds.
		/// </summary>
		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }
	}
}
