using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage sub accounts under the master account.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IAccounts" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/accounts/accounts">Zoom documentation</a> for more information.
	/// </remarks>
	public class Chat : IChat
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Chat" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Chat(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve a user's chat channels.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatChannel">channels</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<ChatChannel>> GetAccountChannelsForUserAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"chat/users/{userId}/channels")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ChatChannel>("channels");
		}

		/// <summary>
		/// Retrieve information about a specific chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="ChatChannel"/>.
		/// </returns>
		public Task<ChatChannel> GetAccountChannelAsync(string userId, string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"chat/users/{userId}/channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ChatChannel>();
		}

		/// <summary>
		/// Create a chat channel that allows users to communicate via chat in private or public groups.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="name">The name of the channel.</param>
		/// <param name="type">The type of channel.</param>
		/// <param name="emails">The email addresses of the members to include in the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <remarks>Zoom allows a maximum of 5 members to be added at once.</remarks>
		/// <returns>
		/// The new <see cref="ChatChannel"/>.
		/// </returns>
		public Task<ChatChannel> CreateAccountChannelAsync(string userId, string name, ChatChannelType type, IEnumerable<string> emails = null, CancellationToken cancellationToken = default)
		{
			if (emails != null && emails.Count() > 5) throw new ArgumentOutOfRangeException(nameof(emails), "You can invite up to 5 members at once");

			var data = new JsonObject
			{
				{ "name", name },
				{ "type", type },
				{ "members", emails?.Select(e => new JsonObject { { "email", e } }) }
			};

			return _client
				.PostAsync($"chat/users/{userId}/channels")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ChatChannel>();
		}

		/// <summary>
		/// Update a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="name">The name of the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateAccountChannelAsync(string userId, string channelId, string name, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name }
			};

			return _client
				.PatchAsync($"chat/users/{userId}/channels/{channelId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAccountChannelAsync(string userId, string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"chat/users/{userId}/channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the members of a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatChannelMember">channel members</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<ChatChannelMember>> GetAccountChannelMembersAsync(string userId, string channelId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"chat/users/{userId}/channels/{channelId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ChatChannelMember>("members");
		}

		/// <summary>
		/// Invite members to join a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="emails">The email addresses of the members to include in the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <remarks>Zoom allows a maximum of 5 members to be added at once.</remarks>
		/// <returns>
		/// An array containing the unique identifiers of the new members.
		/// </returns>
		public Task<string[]> InviteMembersToAccountChannelAsync(string userId, string channelId, IEnumerable<string> emails, CancellationToken cancellationToken = default)
		{
			if (emails == null || !emails.Any()) throw new ArgumentNullException(nameof(emails), "You must specify at least one member to invite");
			if (emails.Count() > 5) throw new ArgumentOutOfRangeException(nameof(emails), "You can invite up to 5 members at once");

			var data = new JsonObject
			{
				{ "members", emails.Select(e => new JsonObject() { { "email", e } }).ToArray() }
			};

			return _client
				.PostAsync($"chat/users/{userId}/channels/{channelId}/members")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string[]>("ids");
		}

		/// <summary>
		/// Remove a member from a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="memberId">The member Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RemoveMemberFromAccountChannelAsync(string userId, string channelId, string memberId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"chat/users/{userId}/channels/{channelId}/members/{memberId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve information about a specific chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="ChatChannel"/>.
		/// </returns>
		public Task<ChatChannel> GetChannelAsync(string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"chat/channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ChatChannel>();
		}

		/// <summary>
		/// Update a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="name">The name of the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateChannelAsync(string channelId, string name, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name }
			};

			return _client
				.PatchAsync($"chat/channels/{channelId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteChannelAsync(string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"chat/channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Remove a member from a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="memberId">The member Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RemoveMemberFromChannelAsync(string channelId, string memberId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"chat/channels/{channelId}/members/{memberId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Join a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The member Id.
		/// </returns>
		public Task<string> JoinChannelAsync(string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"chat/channels/{channelId}/members/me")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		/// <inheritdoc/>
		public Task<string> SendMessageToContactAsync(string userId, string recipientEmail, string message, string replyMessageId = null, IEnumerable<string> fileIds = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return SendMessageAsync(userId, recipientEmail, null, message, replyMessageId, fileIds, mentions, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<string> SendMessageToChannelAsync(string userId, string channelId, string message, string replyMessageId = null, IEnumerable<string> fileIds = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return SendMessageAsync(userId, null, channelId, message, replyMessageId, fileIds, mentions, cancellationToken);
		}

		/// <summary>
		/// Retrieve the chat messages sent/received to/from a contact.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recipientEmail">The email address of the contact to whom the user conversed.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatMessage">chat messages</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToContactAsync(string userId, string recipientEmail, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetMessagesAsync(userId, recipientEmail, null, recordsPerPage, pagingToken, cancellationToken);
		}

		/// <summary>
		/// Retrieve the chat messages sent/received in a channel of which the user is a member.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatMessage">chat messages</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToChannelAsync(string userId, string channelId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetMessagesAsync(userId, null, channelId, recordsPerPage, pagingToken, cancellationToken);
		}

		/// <summary>
		/// Update a message that was previously sent to a user on on the sender's contact list.
		/// </summary>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="recipientEmail">The email address of the contact to whom you would like to send the message.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateMessageToContactAsync(string messageId, string userId, string recipientEmail, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return UpdateMessageAsync(messageId, userId, recipientEmail, null, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Update a message that was previously sent to a channel of which the sender is a member.
		/// </summary>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateMessageToChannelAsync(string messageId, string userId, string channelId, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return UpdateMessageAsync(messageId, userId, null, channelId, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Delete a message that was previously sent to a user on on the sender's contact list.
		/// </summary>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="recipientEmail">The email address of the contact to whom you would like to send the message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteMessageToContactAsync(string messageId, string userId, string recipientEmail, CancellationToken cancellationToken = default)
		{
			return DeleteMessageAsync(messageId, userId, recipientEmail, null, cancellationToken);
		}

		/// <summary>
		/// Delete a message that was previously sent to a channel of which the sender is a member.
		/// </summary>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteMessageToChannelAsync(string messageId, string userId, string channelId, CancellationToken cancellationToken = default)
		{
			return DeleteMessageAsync(messageId, userId, null, channelId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<string> SendFileAsync(string messageId, string userId, string recipientId, string channelId, string fileName, Stream fileData, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"https://file.zoom.us/v2/chat/users/{userId}/messages/files")
				.WithBody(bodyBuilder =>
				{
					// The file name as well as the name of the other 'parts' in the request must be quoted otherwise the Zoom API would return the following error message: Invalid 'Content-Disposition' in multipart form
					var content = new MultipartFormDataContent
					{
						{ new StreamContent(fileData), "files", $"\"{fileName}\"" }
					};
					if (!string.IsNullOrEmpty(messageId)) content.Add(new StringContent(messageId), "\"reply_main_message_id\"");
					if (!string.IsNullOrEmpty(channelId)) content.Add(new StringContent(channelId), "\"to_channel\"");
					if (!string.IsNullOrEmpty(recipientId)) content.Add(new StringContent(recipientId), "\"to_contact\"");

					return content;
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		/// <inheritdoc/>
		public Task<string> UploadFileAsync(string userId, string fileName, Stream fileData, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"https://file.zoom.us/v2/chat/users/{userId}/files")
				.WithBody(bodyBuilder =>
				{
					// The file name must be quoted otherwise the Zoom API would return the following error message: Invalid 'Content-Disposition' in multipart form
					var content = new MultipartFormDataContent
					{
						{ new StreamContent(fileData), "file", $"\"{fileName}\"" }
					};
					return content;
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		private Task<string> SendMessageAsync(string userId, string recipientEmail, string channelId, string message, string replyMessageId = null, IEnumerable<string> fileIds = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "message", message },
				{ "file_ids", fileIds?.ToArray() },
				{ "reply_main_message_id", replyMessageId },
				{ "to_contact", recipientEmail },
				{ "to_channel", channelId },
				{ "at_items", mentions?.ToArray() }
			};

			return _client
				.PostAsync($"chat/users/{userId}/messages")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		private Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesAsync(string userId, string recipientEmail, string channelId, int recordsPerPage, string pagingToken, CancellationToken cancellationToken = default)
		{
			Debug.Assert(recipientEmail != null || channelId != null, "You must provide either recipientEmail or channelId");
			Debug.Assert(recipientEmail == null || channelId == null, "You can't provide both recipientEmail and channelId");

			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"chat/users/{userId}/messages")
				.WithArgument("to_contact", recipientEmail)
				.WithArgument("to_channel", channelId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ChatMessage>("messages");
		}

		private Task UpdateMessageAsync(string messageId, string userId, string recipientEmail, string channelId, string message, IEnumerable<ChatMention> mentions, CancellationToken cancellationToken = default)
		{
			Debug.Assert(recipientEmail != null || channelId != null, "You must provide either recipientEmail or channelId");
			Debug.Assert(recipientEmail == null || channelId == null, "You can't provide both recipientEmail and channelId");

			var data = new JsonObject
			{
				{ "message", message },
				{ "to_contact", recipientEmail },
				{ "to_channel", channelId },
				{ "at_items", mentions?.ToArray() }
			};

			return _client
				.PutAsync($"chat/users/{userId}/messages/{messageId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		private Task DeleteMessageAsync(string messageId, string userId, string recipientEmail, string channelId, CancellationToken cancellationToken = default)
		{
			Debug.Assert(recipientEmail != null || channelId != null, "You must provide either recipientEmail or channelId");
			Debug.Assert(recipientEmail == null || channelId == null, "You can't provide both recipientEmail and channelId");

			return _client
				.DeleteAsync($"chat/users/{userId}/messages/{messageId}")
				.WithArgument("to_contact", recipientEmail)
				.WithArgument("to_channel", channelId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
