using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A message line containing an attachment.
/// </summary>
public class ChatbotAttachment : IChatbotBody, IChatbotSection
{
	/// <summary>
	/// Gets or sets the resource URL. Used to download the file.
	/// </summary>
	[JsonPropertyName("resource_url")]
	public string ResourceUrl { get; set; }

	/// <summary>
	/// Gets or sets the image URL.
	/// </summary>
	[JsonPropertyName("img_url")]
	public string ImageUrl { get; set; }

	/// <summary>
	/// Gets or sets the information of this attachment.
	/// </summary>
	[JsonPropertyName("information")]
	public ChatbotAttachmentInformation Information { get; set; }

	/// <summary>
	/// Gets or sets the extension of the attachment.
	/// </summary>
	[JsonPropertyName("ext")]
	public string Extension { get; set; }

	/// <summary>
	/// Gets or sets the size in bytes.
	/// </summary>
	[JsonPropertyName("size")]
	public int? Size { get; set; }
}
