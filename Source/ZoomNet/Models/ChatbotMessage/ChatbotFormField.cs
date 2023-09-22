using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A single form field element.
/// </summary>
public class ChatbotFormField : IChatbotValidate
{
	/// <summary>
	/// Gets or sets a key.
	/// </summary>
	[JsonPropertyName("key")]
	public string Key { get; set; }

	/// <summary>
	/// Gets or sets a default value. A single space is used to indicate a blank value.
	/// </summary>
	[JsonPropertyName("value")]
	public string Value { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the field can be compressed to two columns.
	/// </summary>
	[JsonPropertyName("short")]
	public bool Short { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the field is editable.
	/// </summary>
	[JsonPropertyName("editable")]
	public bool Editable { get; set; }

	/// <summary>
	/// Verify that the content is valid.
	/// </summary>
	/// <param name="enableMarkdownSupport">True if the content has markdown syntax enabled.</param>
	/// <exception cref="InvalidOperationException">Some or all of the content is invalid.</exception>
	public void Validate(bool enableMarkdownSupport)
	{
		if (enableMarkdownSupport && Editable)
		{
			throw new InvalidOperationException("Editable property cannot be used on Form Field with EnableMarkdownSupport");
		}
	}
}
