using System;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A Chatbot message validator.
/// </summary>
public interface IChatbotValidate
{
	/// <summary>
	/// Verify that the content is valid.
	/// </summary>
	/// <param name="enableMarkdownSupport">True if the content has markdown syntax enabled.</param>
	/// <exception cref="InvalidOperationException">Some or all of the content is invalid.</exception>
	internal void Validate(bool enableMarkdownSupport);
}
