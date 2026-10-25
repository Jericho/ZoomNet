using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>The attachment information.</summary>
public record ChatbotAttachmentInformation
{
	/// <summary>Gets or sets the description line of the attachment.</summary>
	[JsonPropertyName("description")]
	public ChatbotMessageText Description { get; init; }

	/// <summary>Gets or sets the title line of the attachment.</summary>
	[JsonPropertyName("title")]
	public ChatbotMessageText Title { get; init; }
}
