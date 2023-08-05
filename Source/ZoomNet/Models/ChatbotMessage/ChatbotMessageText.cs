using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// The attachment information field.
/// </summary>
public class ChatbotMessageText
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ChatbotMessageText"/> class.
	/// </summary>
	public ChatbotMessageText()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ChatbotMessageText"/> class.
	/// </summary>
	/// <param name="text">The text of the message line.</param>
	public ChatbotMessageText(string text)
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
}
