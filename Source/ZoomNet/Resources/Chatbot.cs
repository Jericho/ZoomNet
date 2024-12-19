using Pathoschild.Http.Client;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.ChatbotMessage;

namespace ZoomNet.Resources;

/// <inheritdoc/>
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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
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
