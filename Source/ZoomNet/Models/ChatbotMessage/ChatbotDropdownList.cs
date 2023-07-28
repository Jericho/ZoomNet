using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A message line containing a dropdown list.
/// </summary>
public class ChatbotDropdownList : IChatbotBody, IChatbotSection
{
	/// <summary>
	/// Gets or sets the text of this message line.
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; }

	/// <summary>
	/// Gets or sets the default selected item.
	/// </summary>
	[JsonPropertyName("selected_item")]
	public ChatbotDropdownItem SelectedItem { get; set; }

	/// <summary>
	/// Gets or sets the data for this dropdown list.
	/// </summary>
	/// <remarks>Should be null when <see cref="StaticSource"/> is set.</remarks>
	[JsonPropertyName("select_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public ICollection<ChatbotDropdownItem> SelectItems { get; set; }

	/// <summary>
	/// Gets or sets the static source of the data for this dropdown list.
	/// </summary>
	/// <remarks>Should be <see cref="ChatbotDropdownStaticSource.Unspecified"/> when <see cref="SelectItems"/> is set.</remarks>
	[JsonPropertyName("static_source")]
	[JsonConverter(typeof(StringEnumConverter<ChatbotDropdownStaticSource>))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public ChatbotDropdownStaticSource StaticSource { get; set; } = default;
}
