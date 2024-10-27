using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A message line containing a set of form fields.
/// </summary>
public class ChatbotFormFields : IChatbotBody, IChatbotSection, IChatbotValidate
{
	/// <summary>
	/// Gets or sets the fields of the message.
	/// </summary>
	[JsonPropertyName("items")]
	public ICollection<ChatbotFormField> Items { get; set; }

	/// <inheritdoc/>
	public void Validate(bool enableMarkdownSupport)
	{
		foreach (var item in Items)
		{
			item.Validate(enableMarkdownSupport);
		}
	}
}
