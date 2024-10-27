using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// The attachment information.
/// </summary>
public class ChatbotAttachmentInformation
{
	/// <summary>
	/// Gets or sets the title line of the attachment.
	/// </summary>
	[JsonPropertyName("title")]
	public ChatbotMessageText Title { get; set; }

	/// <summary>
	/// Gets or sets the description line of the attachment.
	/// </summary>
	[JsonPropertyName("description")]
	public ChatbotMessageText Description { get; set; }
}
