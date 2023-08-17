using Pathoschild.Http.Client;
using System.Text.Json.Nodes;
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
public class Chatbot : IChatbot
{
	private readonly IClient _client;

	/// <summary>
	/// Initializes a new instance of the <see cref="Chatbot" /> class.
	/// </summary>
	/// <param name="client">The HTTP client.</param>
	internal Chatbot(IClient client)
	{
		_client = client;
	}

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
	public Task<ChatbotMessageInformation> DeleteMessageAsync(string messageId, string accountId, string userJId, string robotJId, CancellationToken cancellationToken = default)
	{
		return _client
			.DeleteAsync($"im/chat/messages/{messageId}")
			.WithArgument("account_id", accountId)
			.WithArgument("user_jid", userJId)
			.WithArgument("robot_jid", robotJId)
			.WithCancellationToken(cancellationToken)
			.AsObject<ChatbotMessageInformation>();
	}

	/// <summary>
	/// Send Chatbot message.
	/// </summary>
	/// <param name="accountId">The account ID to which the message was sent.</param>
	/// <param name="toJId">The JID of group channel or user to whom the message should be sent.</param>
	/// <param name="robotJId">The robot JID.</param>
	/// <param name="message">The simple text message to send.</param>
	/// <param name="enableMarkdownSupport">True if the message contains markdown syntax.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// The async task.
	/// </returns>
	public Task<ChatbotMessageInformation> SendMessageAsync(string accountId, string toJId, string robotJId, string message, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
	{
		return SendMessageAsync(accountId, toJId, robotJId, new ChatbotContent() { Head = new ChatbotHeader(message) }, enableMarkdownSupport, cancellationToken);
	}

	/// <summary>
	/// Send Chatbot message.
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
	public Task<ChatbotMessageInformation> SendMessageAsync(string accountId, string toJId, string robotJId, ChatbotContent content, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
	{
		content.Validate(enableMarkdownSupport);

		var data = new JsonObject
		{
			{ "robot_jid", robotJId },
			{ "to_jid", toJId },
			{ "account_id", accountId },
			{ "content", content },
			{ "is_markdown_support", enableMarkdownSupport }
		};

		return _client
			.PostAsync("im/chat/messages")
			.WithJsonBody(data)
			.WithCancellationToken(cancellationToken)
			.AsObject<ChatbotMessageInformation>();
	}

	/// <summary>
	/// Edit a Chatbot message.
	/// </summary>
	/// <param name="messageId">The message ID of the message to edit.</param>
	/// <param name="accountId">The account ID to which the message was sent.</param>
	/// <param name="toJId">The JID of group channel or user to whom the message should be sent.</param>
	/// <param name="robotJId">The robot JID.</param>
	/// <param name="message">The simple text message to send.</param>
	/// <param name="enableMarkdownSupport">True if the message contains markdown syntax.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// The async task.
	/// </returns>
	public Task<ChatbotMessageInformation> EditMessageAsync(string messageId, string accountId, string toJId, string robotJId, string message, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
	{
		return EditMessageAsync(messageId, accountId, toJId, robotJId, new ChatbotContent() { Head = new ChatbotHeader(message) }, enableMarkdownSupport, cancellationToken);
	}

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
	public Task<ChatbotMessageInformation> EditMessageAsync(string messageId, string accountId, string toJId, string robotJId, ChatbotContent content, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
	{
		content.Validate(enableMarkdownSupport);

		var data = new JsonObject
		{
			{ "robot_jid", robotJId },
			{ "to_jid", toJId },
			{ "account_id", accountId },
			{ "content", content },
			{ "is_markdown_support", enableMarkdownSupport }
		};

		return _client
			.PutAsync($"im/chat/messages/{messageId}")
			.WithJsonBody(data)
			.WithCancellationToken(cancellationToken)
			.AsObject<ChatbotMessageInformation>();
	}
}
