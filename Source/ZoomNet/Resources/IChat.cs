using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage chat channels, messages, etc.
	/// </summary>
	/// <remarks>
	/// See Zoom documentation <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/chat-channels-account-level/">here</a>,
	/// <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/chat-channels/">here</a>,
	/// <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/chat-messages/">here</a>
	/// and <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/chatbot-messages/">here</a> for more information.
	/// </remarks>
	public interface IChat
	{
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
		Task<PaginatedResponseWithToken<ChatChannel>> GetAccountChannelsForUserAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve information about a specific chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="ChatChannel"/>.
		/// </returns>
		Task<ChatChannel> GetAccountChannelAsync(string userId, string channelId, CancellationToken cancellationToken = default);

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
		Task<ChatChannel> CreateAccountChannelAsync(string userId, string name, ChatChannelType type, IEnumerable<string> emails = null, CancellationToken cancellationToken = default);

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
		Task UpdateAccountChannelAsync(string userId, string channelId, string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a chat channel.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAccountChannelAsync(string userId, string channelId, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<ChatChannelMember>> GetAccountChannelMembersAsync(string userId, string channelId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		Task<string[]> InviteMembersToAccountChannelAsync(string userId, string channelId, IEnumerable<string> emails, CancellationToken cancellationToken = default);

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
		Task RemoveMemberFromAccountChannelAsync(string userId, string channelId, string memberId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve information about a specific chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="ChatChannel"/>.
		/// </returns>
		Task<ChatChannel> GetChannelAsync(string channelId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="name">The name of the channel.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateChannelAsync(string channelId, string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteChannelAsync(string channelId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove a member from a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="memberId">The member Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RemoveMemberFromChannelAsync(string channelId, string memberId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Join a chat channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The member Id.
		/// </returns>
		Task<string> JoinChannelAsync(string channelId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send a message to a user on the sender's contact list.
		/// </summary>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="recipientEmail">The email address of the contact to whom you would like to send the message.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		Task<string> SendMessageToContactAsync(string userId, string recipientEmail, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send a message to a channel of which the sender is a member.
		/// </summary>
		/// <param name="userId">The unique identifier of the sender.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="message">The message.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		Task<string> SendMessageToChannelAsync(string userId, string channelId, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToContactAsync(string userId, string recipientEmail, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<ChatMessage>> GetMessagesToChannelAsync(string userId, string channelId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		Task UpdateMessageToContactAsync(string messageId, string userId, string recipientEmail, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default);

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
		Task UpdateMessageToChannelAsync(string messageId, string userId, string channelId, string message, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default);

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
		Task DeleteMessageToContactAsync(string messageId, string userId, string recipientEmail, CancellationToken cancellationToken = default);

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
		Task DeleteMessageToChannelAsync(string messageId, string userId, string channelId, CancellationToken cancellationToken = default);
	}
}
