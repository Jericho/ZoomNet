using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// An action which can be clicked.
/// </summary>
public class ChatbotAction
{
	/// <summary>
	/// Gets or sets the text of this action.
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; }

	/// <summary>
	/// Gets or sets the value of this action.
	/// </summary>
	[JsonPropertyName("value")]
	public string Value { get; set; }

	/// <summary>
	/// Gets or sets the style of this action.
	/// </summary>
	[JsonPropertyName("style")]
	[JsonConverter(typeof(StringEnumConverter<ChatbotActionStyle>))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public ChatbotActionStyle Style { get; set; }
}
