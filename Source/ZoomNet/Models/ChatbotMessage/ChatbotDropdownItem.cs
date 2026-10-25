using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>A dropdown list item.</summary>
public record ChatbotDropdownItem
{
	/// <summary>Gets or sets the text of this dropdown list item.</summary>
	[JsonPropertyName("text")]
	public string Text { get; init; }

	/// <summary>Gets or sets the value of this dropdown list item.</summary>
	[JsonPropertyName("value")]
	public string Value { get; init; }
}
