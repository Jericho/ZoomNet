using System.Drawing;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A style applied to a message.
/// </summary>
public class ChatbotMessageStyle
{
	/// <summary>
	/// Gets or sets the color of this message.
	/// </summary>
	[JsonPropertyName("color")]
	[JsonConverter(typeof(NullableHexColorConverter))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Color? Color { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the bold style is set for this message.
	/// </summary>
	[JsonPropertyName("bold")]
	public bool Bold { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the italic style is set for this message.
	/// </summary>
	[JsonPropertyName("italic")]
	public bool Italic { get; set; }
}
