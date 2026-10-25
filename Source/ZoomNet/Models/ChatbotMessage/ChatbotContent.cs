using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>The message content for a Chatbot message.</summary>
public record ChatbotContent
{
	/// <summary>Gets or sets the body of the message.</summary>
	[JsonPropertyName("body")]
	public ICollection<IChatbotBody> Body { get; init; }

	/// <summary>Gets or sets the header and sub-header of the message.</summary>
	[JsonPropertyName("head")]
	public ChatbotHeader Head { get; init; }

	/// <summary>Verify that the content is valid.</summary>
	/// <param name="enableMarkdownSupport">True if the content has markdown syntax enabled.</param>
	/// <exception cref="InvalidOperationException">Some or all of the content is invalid.</exception>
	internal void Validate(bool enableMarkdownSupport)
	{
		if (Body == null)
		{
			return;
		}

		foreach (var message in Body.OfType<IChatbotValidate>())
		{
			message.Validate(enableMarkdownSupport);
		}
	}
}
