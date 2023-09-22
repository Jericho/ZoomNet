using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
///     A message header which is above the main body of the message.
/// </summary>
public class ChatbotHeader
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ChatbotHeader" /> class.
	/// </summary>
	public ChatbotHeader()
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ChatbotHeader" /> class.
	/// </summary>
	/// <param name="text">The text of the message header.</param>
	public ChatbotHeader(string text)
	{
		Text = text;
	}

	/// <summary>
	/// Gets or sets the text of the message.
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; }

	/// <summary>
	/// Gets or sets the style of the message text.
	/// </summary>
	[JsonPropertyName("style")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public ChatbotMessageStyle Style { get; set; }

	/// <summary>
	///     Gets or sets the sub-header of the message header.
	/// </summary>
	[JsonPropertyName("sub_head")]
	public ChatbotMessageText SubHeader { get; set; }
}
