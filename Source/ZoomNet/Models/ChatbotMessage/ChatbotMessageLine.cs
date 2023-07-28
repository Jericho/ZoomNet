using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A line of the message body.
/// </summary>
public class ChatbotMessageLine : IChatbotBody, IChatbotSection, IChatbotValidate
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ChatbotMessageLine"/> class.
	/// </summary>
	public ChatbotMessageLine()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ChatbotMessageLine"/> class.
	/// </summary>
	/// <param name="text">The text of the message line.</param>
	public ChatbotMessageLine(string text)
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
	/// Gets or sets a value indicating whether the message is editable.
	/// </summary>
	[JsonPropertyName("editable")]
	public bool Editable { get; set; }

	/// <summary>
	/// Gets or sets the link for the message.
	/// Converts the entire message text into a link.
	/// Should only be used if not using markdown.
	/// For markdown, use the undocumented link text feature: &lt;https://example.com|Link Text&gt;.
	/// </summary>
	/// <value>The link.</value>
	[JsonPropertyName("link")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Link { get; set; }

	/// <inheritdoc />
	public void Validate(bool enableMarkdownSupport)
	{
		if (enableMarkdownSupport && Link != null)
		{
			throw new InvalidOperationException("Link property cannot be used with EnableMarkdownSupport");
		}
	}
}
