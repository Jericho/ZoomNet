using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>A response from the Chatbot endpoints.</summary>
public record ChatbotMessageInformation
{
	/// <summary>Gets or sets the message ID.</summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; init; }

	/// <summary>Gets or sets the Robot JID.</summary>
	[JsonPropertyName("robot_jid")]
	public string RobotJId { get; init; }

	/// <summary>Gets or sets the sent time of the message.</summary>
	[JsonPropertyName("sent_time")]
	public string SentTime { get; init; }

	/// <summary>Gets or sets the To JID. May be a channel or a user.</summary>
	[JsonPropertyName("to_jid")]
	public string ToJId { get; init; }
}
