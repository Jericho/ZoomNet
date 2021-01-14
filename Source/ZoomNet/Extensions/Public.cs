using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Public extension methods.
	/// </summary>
	public static class Public
	{
		/// <summary>
		/// Returns information about the current user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The current user.</returns>
		public static Task<User> GetCurrentAsync(this IUsers usersResource, CancellationToken cancellationToken)
		{
			return usersResource.GetAsync("me", cancellationToken);
		}

		/// <summary>
		/// Add an assistant to a user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantId">The id of the assistant to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task AddAssistantByIdAsync(this IUsers usersResource, string userId, string assistantId, CancellationToken cancellationToken = default)
		{
			return usersResource.AddAssistantsByIdAsync(userId, new[] { assistantId }, cancellationToken);
		}

		/// <summary>
		/// Add an assistant to a user.
		/// </summary>
		/// <param name="usersResource">The user resource.</param>
		/// <param name="userId">The user Id.</param>
		/// <param name="assistantEmail">The email address of the assistant to associate with this user.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task AddAssistantByEmailAsync(this IUsers usersResource, string userId, string assistantEmail, CancellationToken cancellationToken = default)
		{
			return usersResource.AddAssistantsByIdAsync(userId, new[] { assistantEmail }, cancellationToken);
		}

		/// <summary>
		/// Leave a chat channel.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task LeaveChannelAsync(this IChat chatResource, string channelId, CancellationToken cancellationToken = default)
		{
			return chatResource.RemoveMemberFromChannelAsync(channelId, "me", cancellationToken);
		}

		/// <summary>
		/// Send a message to a user on your contact list.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="recipientEmail">The email address of the contact to whom you would like to send the message.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		public static Task<string> SendMessageToContactAsync(this IChat chatResource, string recipientEmail, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.SendMessageToContactAsync("me", recipientEmail, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Send a message to a channel of which you are a member of.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		public static Task<string> SendMessageToChannelAsync(this IChat chatResource, string channelId, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.SendMessageToChannelAsync("me", channelId, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Retrieve the messages that you have sent/received to/from a user on your contact list.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="recipientEmail">The email address of the contact with whom the user conversed.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatMessage">messages</see>.
		/// </returns>
		/// <remarks>The Zoom API doesn't seem to return the 'TotalRecords' property in the response.</remarks>
		public static Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToContactAsync(this IChat chatResource, string recipientEmail, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return chatResource.GetMessagesToContactAsync("me", recipientEmail, recordsPerPage, pagingToken, cancellationToken);
		}

		/// <summary>
		/// Retrieve the messages you have sent/received to/from a chat channel.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatMessage">messages</see>.
		/// </returns>
		/// <remarks>The Zoom API doesn't seem to return the 'TotalRecords' property in the response.</remarks>
		public static Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToChannelAsync(this IChat chatResource, string channelId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return chatResource.GetMessagesToChannelAsync("me", channelId, recordsPerPage, pagingToken, cancellationToken);
		}

		/// <summary>
		/// Edit a chat message that you previously sent to a user on your contact list.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="recipientEmail">The email address of the contact to whom you would like to send the message.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task UpdateMessageToContactAsync(this IChat chatResource, string messageId, string recipientEmail, string message = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.UpdateMessageToContactAsync(messageId, "me", recipientEmail, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Edit a chat message that you previously sent to a channel that you are a member of.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="channelId">The unique identifier of the channel.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task UpdateMessageToChannelAsync(this IChat chatResource, string messageId, string channelId, string message = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.UpdateMessageToChannelAsync(messageId, "me", channelId, message, mentions, cancellationToken);
		}

		/// <summary>
		/// Delete a chat message that you previously sent to a user on your contact list.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="recipientEmail">The email address of the contact to whom the message was sent.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DeleteMessageToContactAsync(this IChat chatResource, string messageId, string recipientEmail, CancellationToken cancellationToken = default)
		{
			return chatResource.DeleteMessageToContactAsync(messageId, "me", recipientEmail, cancellationToken);
		}

		/// <summary>
		/// Delete a chat message that you previously sent to a channel that you are a member of.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="messageId">The unique identifier of the message.</param>
		/// <param name="channelId">The unique identifier of the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DeleteMessageToChannelAsync(this IChat chatResource, string messageId, string channelId, CancellationToken cancellationToken = default)
		{
			return chatResource.DeleteMessageToChannelAsync(messageId, "me", channelId, cancellationToken);
		}
	}
}
