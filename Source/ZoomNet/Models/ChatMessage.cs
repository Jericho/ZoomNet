using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Chat message.</summary>
	public record ChatMessage
	{
		/// <summary>Gets or sets the date and time at which the message was sent.</summary>
		[JsonPropertyName("date_time")]
		public DateTime SentOn { get; init; }

		/// <summary>Gets or sets the unique identifier of the message.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the message content.</summary>
		[JsonPropertyName("message")]
		public string Message { get; init; }

		/// <summary>Gets or sets the timestamp of the message in microseconds.</summary>
		[JsonPropertyName("timestamp")]
		public long Timestamp { get; init; }
	}
}
