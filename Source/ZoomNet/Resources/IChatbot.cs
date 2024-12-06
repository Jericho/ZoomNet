using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.ChatbotMessage;

namespace ZoomNet.Resources;

/// <summary>
/// Allows you to manage Chatbot messages.
/// </summary>
/// <remarks>
/// See <a href="https://developers.zoom.us/docs/api/rest/reference/chatbot/methods/">Zoom documentation</a> for more information.
/// </remarks>
public interface IChatbot
{
	/// <summary>
	/// Delete a Chatbot message.
	/// </summary>
	/// <param name="messageId">The message ID.</param>
	/// <param name="accountId">The account ID to which the message was sent.</param>
	/// <param name="userJId">The JID of the user on whose behalf the message is being sent. Optional.</param>
	/// <param name="robotJId">The robot JID.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// The async task.
	/// </returns>
	public Task<ChatbotMessageInformation> DeleteMessageAsync(string messageId, string accountId, string userJId, string robotJId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Send a Chatbot message.
	/// </summary>
	/// <param name="accountId">The account ID to which the message was sent.</param>
	/// <param name="toJId">The JID of group channel or user to whom the message should be sent.</param>
	/// <param name="robotJId">The robot JID.</param>
	/// <param name="content">The content of the message.</param>
	/// <param name="enableMarkdownSupport">True if the content contains markdown syntax.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// The async task.
	/// </returns>
	public Task<ChatbotMessageInformation> SendMessageAsync(string accountId, string toJId, string robotJId, ChatbotContent content, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default);

	/// <summary>
	/// Edit a Chatbot message.
	/// </summary>
	/// <param name="messageId">The message ID of the message to edit.</param>
	/// <param name="accountId">The account ID to which the message was sent.</param>
	/// <param name="toJId">The JID of group channel or user to whom the message should be sent.</param>
	/// <param name="robotJId">The robot JID.</param>
	/// <param name="content">The content of the message.</param>
	/// <param name="enableMarkdownSupport">True if the content contains markdown syntax.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// The async task.
	/// </returns>
	public Task<ChatbotMessageInformation> EditMessageAsync(string messageId, string accountId, string toJId, string robotJId, ChatbotContent content, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default);
}
