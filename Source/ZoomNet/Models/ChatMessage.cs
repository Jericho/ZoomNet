using Newtonsoft.Json;
using System;

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
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the message content.
		/// </summary>
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the date and time at which the message was sent.
		/// </summary>
		[JsonProperty(PropertyName = "date_time")]
		public DateTime SentOn { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the message in microseconds.
		/// </summary>
		[JsonProperty(PropertyName = "timestamp")]
		public long Timestamp { get; set; }
	}
}
