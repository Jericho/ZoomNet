using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A message line containing one or more actions.
/// </summary>
public class ChatbotActions : IChatbotBody, IChatbotSection
{
	/// <summary>
	/// Gets or sets the actions.
	/// </summary>
	[JsonPropertyName("items")]
	public ICollection<ChatbotAction> Items { get; set; }

	/// <summary>
	/// Gets or sets the number of buttons visible.
	/// If the number of items are over this limit, it will group the buttons into a list.
	/// </summary>
	[JsonPropertyName("limit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? Limit { get; set; }
}
