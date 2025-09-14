using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.ChatbotMessage;
using ZoomNet.Resources;
using ZoomNet.Utilities;

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
		/// <param name="usersResource">The users resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The current user.</returns>
		public static Task<User> GetCurrentAsync(this IUsers usersResource, CancellationToken cancellationToken = default)
		{
			return usersResource.GetAsync("me", cancellationToken);
		}

		/// <summary>
		/// Retrieve the permissions that have been granted to the current user.
		/// </summary>
		/// <param name="usersResource">The users resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of strings.
		/// </returns>
		public static Task<string[]> GetCurrentPermissionsAsync(this IUsers usersResource, CancellationToken cancellationToken = default)
		{
			return usersResource.GetPermissionsAsync("me", cancellationToken);
		}

		/// <summary>
		/// Retrieve the current user's settings.
		/// </summary>
		/// <param name="usersResource">The users resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="UserSettings">settings</see>.
		/// </returns>
		public static Task<UserSettings> GetCurrentSettingsAsync(this IUsers usersResource, CancellationToken cancellationToken = default)
		{
			return usersResource.GetSettingsAsync("me", cancellationToken);
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
		/// <param name="replyMessageId">The reply message's ID. </param>
		/// <param name="fileIds">A list of the file IDs to send. This field only accepts a maximum of six file IDs.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		public static Task<string> SendMessageToContactAsync(this IChat chatResource, string recipientEmail, string message, string replyMessageId = null, IEnumerable<string> fileIds = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.SendMessageToContactAsync("me", recipientEmail, message, replyMessageId, fileIds, mentions, cancellationToken);
		}

		/// <summary>
		/// Send a message to a channel of which you are a member of.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="message">The message.</param>
		/// <param name="replyMessageId">The reply message's ID. </param>
		/// <param name="fileIds">A list of the file IDs to send. This field only accepts a maximum of six file IDs.</param>
		/// <param name="mentions">Mentions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The message Id.
		/// </returns>
		public static Task<string> SendMessageToChannelAsync(this IChat chatResource, string channelId, string message, string replyMessageId = null, IEnumerable<string> fileIds = null, IEnumerable<ChatMention> mentions = null, CancellationToken cancellationToken = default)
		{
			return chatResource.SendMessageToChannelAsync("me", channelId, message, replyMessageId, fileIds, mentions, cancellationToken);
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

		/// <summary>
		/// Promote a chat channel member to admin.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="email">The email address of the member to promote.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task<ChatMembersEditResult> PromoteMemberInAccountChannelByEmailAsync(this IChat chatResource, string channelId, string email, CancellationToken cancellationToken = default)
		{
			return chatResource.PromoteMembersInAccountChannelByEmailAsync("me", channelId, [email], cancellationToken);
		}

		/// <summary>
		/// Promote a chat channel member to admin.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelOwnerUserId">The user id or email of the channel owner.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="email">The email address of the member to promote.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task<ChatMembersEditResult> PromoteMemberInAccountChannelByEmailAsync(this IChat chatResource, string channelOwnerUserId, string channelId, string email, CancellationToken cancellationToken = default)
		{
			return chatResource.PromoteMembersInAccountChannelByEmailAsync(channelOwnerUserId, channelId, [email], cancellationToken);
		}

		/// <summary>
		/// Demotes administrators in an account channel by user id.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelOwnerUserId">The user id or email of the channel owner.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="userId">The user id or email with channel admin privileges.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DemoteAdminInAccountChannelByUserIdAsync(this IChat chatResource, string channelOwnerUserId, string channelId, string userId, CancellationToken cancellationToken = default)
		{
			return chatResource.DemoteAdminsInAccountChannelByUserIdAsync(channelOwnerUserId, channelId, [userId], cancellationToken);
		}

		/// <summary>
		/// Demotes administrators in an account channel by admin id.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelOwnerUserId">The user id or email of the channel owner.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="adminId">The admin ID with channel admin privileges.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DemoteAdminInAccountChannelByIdAsync(this IChat chatResource, string channelOwnerUserId, string channelId, string adminId, CancellationToken cancellationToken = default)
		{
			return chatResource.DemoteAdminsInAccountChannelByIdAsync(channelOwnerUserId, channelId, [adminId], cancellationToken);
		}

		/// <summary>
		/// Demotes administrators in an account channel by user id.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="userId">The user id or email with channel admin privileges.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DemoteAdminInAccountChannelByUserIdAsync(this IChat chatResource, string channelId, string userId, CancellationToken cancellationToken = default)
		{
			return chatResource.DemoteAdminsInAccountChannelByUserIdAsync("me", channelId, [userId], cancellationToken);
		}

		/// <summary>
		/// Demotes administrators in an account channel by admin id.
		/// </summary>
		/// <param name="chatResource">The chat resource.</param>
		/// <param name="channelId">The channel Id.</param>
		/// <param name="adminId">The admin ID with channel admin privileges.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task DemoteAdminInAccountChannelByIdAsync(this IChat chatResource, string channelId, string adminId, CancellationToken cancellationToken = default)
		{
			return chatResource.DemoteAdminsInAccountChannelByIdAsync("me", channelId, [adminId], cancellationToken);
		}

		/// <summary>
		/// Parses the event webhook asynchronously.
		/// </summary>
		/// <param name="parser">The webhook parser.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>An <see cref="Models.Webhooks.Event" />.</returns>
		public static async Task<Models.Webhooks.Event> ParseEventWebhookAsync(this IWebhookParser parser, Stream stream)
		{
			string requestBody;
			using (var streamReader = new StreamReader(stream))
			{
				requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}

			return parser.ParseEventWebhook(requestBody);
		}

		/// <summary>
		/// Verifies the signature and parses the event webhook asynchronously.
		/// </summary>
		/// <param name="parser">The webhook parser.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="secretToken">Your secret token. You can obtain this value in the 'Add Feature' configuration section of you Marketplace Zoom app.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>An <see cref="Models.Webhooks.Event" />.</returns>
		public static async Task<Models.Webhooks.Event> VerifyAndParseEventWebhookAsync(this IWebhookParser parser, Stream stream, string secretToken, string signature, string timestamp)
		{
			string requestBody;
			using (var streamReader = new StreamReader(stream))
			{
				requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}

			return parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp);
		}

		/// <summary>
		/// Verifies the signature and parses the event webhook asynchronously.
		/// </summary>
		/// <param name="parser">The webhook parser.</param>
		/// <param name="requestBody">The content submitted by Zoom's webhook.</param>
		/// <param name="secretToken">Your secret token. You can obtain this value in the 'Add Feature' configuration section of you Marketplace Zoom app.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>An <see cref="Models.Webhooks.Event" />.</returns>
		public static Models.Webhooks.Event VerifyAndParseEventWebhook(this IWebhookParser parser, string requestBody, string secretToken, string signature, string timestamp)
		{
			// Compare the signatures
			if (!parser.VerifySignature(requestBody, secretToken, signature, timestamp)) throw new SecurityException("Webhook signature validation failed.");

			// Parse the webhook event
			return parser.ParseEventWebhook(requestBody);
		}

		/// <summary>
		/// Download the recording file.
		/// </summary>
		/// <param name="cloudRecordingsResource">The cloud recordings resource.</param>
		/// <param name="recordingFile">The recording file to download.</param>
		/// <param name="accessToken">The access token. If this parameter is omitted, the token for the current oAuth session will be used.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The <see cref="Stream"/> containing the file.
		/// </returns>
		public static Task<Stream> DownloadFileAsync(this ICloudRecordings cloudRecordingsResource, RecordingFile recordingFile, string accessToken = null, CancellationToken cancellationToken = default)
		{
			return cloudRecordingsResource.DownloadFileAsync(recordingFile.DownloadUrl, accessToken, cancellationToken);
		}

		/// <summary>
		/// Invite a participant to join a live meeting.
		/// </summary>
		/// <param name="meetingsResource">The meetingds resource.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="emailAddress">The email address of the person you want to invite.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task InviteParticipantAsync(this IMeetings meetingsResource, long meetingId, string emailAddress, CancellationToken cancellationToken = default)
		{
			return meetingsResource.InviteParticipantsAsync(meetingId, new[] { emailAddress }, cancellationToken);
		}

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="meetingResource">The meeting resource.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		/// <remarks>
		/// Please note that when retrieving a recurring meeting, this method will omit previous occurrences.
		/// Use <see cref="IMeetings.GetAsync(long, string, bool, CancellationToken)"/> if you want past occurrences to be included.
		/// </remarks>
		public static Task<Meeting> GetAsync(this IMeetings meetingResource, long meetingId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return meetingResource.GetAsync(meetingId, occurrenceId, false, cancellationToken);
		}

		/// <summary>
		/// Retrieve the details of a webinar.
		/// </summary>
		/// <param name="webinarResource">The webinar resource.</param>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Webinar" />.
		/// </returns>
		/// <remarks>
		/// Please note that when retrieving a recurring meeting, this method will omit previous occurrences.
		/// Use <see cref="IWebinars.GetAsync(long, string, bool, CancellationToken)"/> if you want past occurrences to be included.
		/// </remarks>
		public static Task<Webinar> GetAsync(this IWebinars webinarResource, long webinarId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return webinarResource.GetAsync(webinarId, occurrenceId, false, cancellationToken);
		}

		/// <summary>
		/// Add user to a group.
		/// </summary>
		/// <param name="groupsResource">The group resource.</param>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emailAddress">An email address of user to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the added user.</returns>
		public static async Task<string> AddMemberByEmailAsync(this IGroups groupsResource, string groupId, string emailAddress, CancellationToken cancellationToken = default)
		{
			var result = await groupsResource.AddMembersByEmailAsync(groupId, new[] { emailAddress }, cancellationToken).ConfigureAwait(false);

			// We added a single member to a group therefore the array returned from the Zoom API contains a single element
			return result.Single();
		}

		/// <summary>
		/// Add user to a group.
		/// </summary>
		/// <param name="groupsResource">The group resource.</param>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="userId">The user unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the added user.</returns>
		public static async Task<string> AddMemberByIdAsync(this IGroups groupsResource, string groupId, string userId, CancellationToken cancellationToken = default)
		{
			var result = await groupsResource.AddMembersByIdAsync(groupId, new[] { userId }, cancellationToken).ConfigureAwait(false);

			// We added a single member to a group therefore the array returned from the Zoom API contains a single element
			return result.Single();
		}

		/// <summary>
		/// Add an administrator to a group.
		/// </summary>
		/// <param name="groupsResource">The group resource.</param>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emailAddress">An email address of user to add as an administrator to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the added user.</returns>
		public static async Task<string> AddAdministratorByEmailAsync(this IGroups groupsResource, string groupId, string emailAddress, CancellationToken cancellationToken = default)
		{
			var result = await groupsResource.AddAdministratorsByEmailAsync(groupId, new[] { emailAddress }, cancellationToken).ConfigureAwait(false);

			// We added a single member to a group therefore the array returned from the Zoom API contains a single element
			return result.Single();
		}

		/// <summary>
		/// Add an administrator to a group.
		/// </summary>
		/// <param name="groupsResource">The group resource.</param>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="userId">The user unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the added user.</returns>
		public static async Task<string> AddAdministratorByIdAsync(this IGroups groupsResource, string groupId, string userId, CancellationToken cancellationToken = default)
		{
			var result = await groupsResource.AddAdministratorsByIdAsync(groupId, new[] { userId }, cancellationToken).ConfigureAwait(false);

			// We added a single member to a group therefore the array returned from the Zoom API contains a single element
			return result.Single();
		}

		/// <summary>
		/// Determines if the specified scope has been granted.
		/// </summary>
		/// <param name="client">The ZoomNet client.</param>
		/// <param name="scope">The name of the scope.</param>
		/// <returns>True if the scope has been granted, False otherwise.</returns>
		/// <remarks>
		/// The concept of "scopes" only applies to OAuth connections.
		/// Therefore an exeption will be thrown if you invoke this method while using
		/// a JWT connection (you shouldn't be using JWT in the first place since this
		/// type of connection has been deprecated in the Zoom API since September 2023).
		/// </remarks>
		public static bool HasPermission(this IZoomClient client, string scope)
		{
			return client.HasPermissions(new[] { scope });
		}

		/// <summary>
		/// Send a Chatbot message.
		/// </summary>
		/// <param name="chatbotResource">The chatbot resource.</param>
		/// <param name="accountId">The account ID to which the message was sent.</param>
		/// <param name="toJId">The JID of group channel or user to whom the message should be sent.</param>
		/// <param name="robotJId">The robot JID.</param>
		/// <param name="message">The simple text message to send.</param>
		/// <param name="enableMarkdownSupport">True if the message contains markdown syntax.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public static Task<ChatbotMessageInformation> SendMessageAsync(this IChatbot chatbotResource, string accountId, string toJId, string robotJId, string message, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
		{
			return chatbotResource.SendMessageAsync(accountId, toJId, robotJId, new ChatbotContent() { Head = new ChatbotHeader(message) }, enableMarkdownSupport, cancellationToken);
		}

		/// <summary>
		/// Edit a Chatbot message.
		/// </summary>
		/// <param name="chatbotResource">The chatbot resource.</param>
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
		public static Task<ChatbotMessageInformation> EditMessageAsync(this IChatbot chatbotResource, string messageId, string accountId, string toJId, string robotJId, string message, bool enableMarkdownSupport = false, CancellationToken cancellationToken = default)
		{
			return chatbotResource.EditMessageAsync(messageId, accountId, toJId, robotJId, new ChatbotContent() { Head = new ChatbotHeader(message) }, enableMarkdownSupport, cancellationToken);
		}

		/// <summary>
		/// Delete a group's Virtual Background file.
		/// </summary>
		/// <param name="groupsResource">The groups ressource.</param>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="fileId">A file unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		public static Task DeleteVirtualBackgroundAsync(this IGroups groupsResource, string groupId, string fileId, CancellationToken cancellationToken = default)
		{
			return groupsResource.DeleteVirtualBackgroundsAsync(groupId, new[] { fileId }, cancellationToken);
		}

		/// <summary>
		/// Assign a tag to a Zoom Room.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="roomId">The unique identifier of the Zoom Room. </param>
		/// <param name="tagId">The Tag ID to assign to the Zoom Room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		public static Task AssignTagToRoom(this IRooms roomsResource, string roomId, string tagId, CancellationToken cancellationToken = default)
		{
			return roomsResource.AssignTagsToRoom(roomId, new[] { tagId }, cancellationToken);
		}

		/// <summary>
		/// Assign multiple tags to Zoom Rooms by location ID.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="locationId">The unique identifier of the location where all Zoom Rooms under this location to be assigned with tags.</param>
		/// <param name="tagId">The Tag ID to assign to all the Zoom Rooms in the given location.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		public static Task AssignTagToRoomsInLocation(this IRooms roomsResource, string locationId, string tagId, CancellationToken cancellationToken = default)
		{
			return roomsResource.AssignTagsToRoomsInLocation(locationId, new[] { tagId }, cancellationToken);
		}

		/// <summary>
		/// Displays the specified emergency content on all Zoom Rooms' displays in the specified account.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task DisplayEmergencyContentToAccountAsync(this IRooms roomsResource, string content, string accountId, CancellationToken cancellationToken = default)
		{
			return roomsResource.DisplayEmergencyContentToAccountsAsync(content, new[] { accountId }, cancellationToken);
		}

		/// <summary>
		/// Displays the specified emergency content on all Zoom Rooms' displays in the specified location.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="locationId">Thelocation identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task DisplayEmergencyContentToLocationAsync(this IRooms roomsResource, string content, string locationId, CancellationToken cancellationToken = default)
		{
			return roomsResource.DisplayEmergencyContentToLocationsAsync(content, new[] { locationId }, cancellationToken);
		}

		/// <summary>
		/// Displays the specified emergency content on the Zoom Rooms digital signage display.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="roomId">The  Zoom room identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task DisplayEmergencyContentToRoomAsync(this IRooms roomsResource, string content, string roomId, CancellationToken cancellationToken = default)
		{
			return roomsResource.DisplayEmergencyContentToRoomsAsync(content, new[] { roomId }, cancellationToken);
		}

		/// <summary>
		/// Remove the specified emergency content from all Zoom Rooms' displays in the specified account.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task RemoveEmergencyContentFromAccountAsync(this IRooms roomsResource, string accountId, CancellationToken cancellationToken = default)
		{
			return roomsResource.RemoveEmergencyContentFromAccountsAsync(new[] { accountId }, cancellationToken);
		}

		/// <summary>
		/// Remove the specified emergency content from all Zoom Rooms' displays in the specified location.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="locationId">The location identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task RemoveEmergencyContentFromLocationAsync(this IRooms roomsResource, string locationId, CancellationToken cancellationToken = default)
		{
			return roomsResource.RemoveEmergencyContentFromLocationsAsync(new[] { locationId }, cancellationToken);
		}

		/// <summary>
		/// Remove the specified emergency content from the Zoom Room digital signage display.
		/// </summary>
		/// <param name="roomsResource">The rooms ressource.</param>
		/// <param name="roomId">The Zoom room identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task RemoveEmergencyContentFromRoomAsync(this IRooms roomsResource, string roomId, CancellationToken cancellationToken = default)
		{
			return roomsResource.RemoveEmergencyContentFromRoomsAsync(new[] { roomId }, cancellationToken);
		}

		/// <summary>
		/// Asynchronously creates a ticket for a specified event with optional attendee details.
		/// </summary>
		/// <param name="eventsResource">The event resource interface used to create the ticket.</param>
		/// <param name="eventId">The unique identifier of the event for which the ticket is being created. Cannot be null or empty.</param>
		/// <param name="firstName">The first name of the attendee. Optional.</param>
		/// <param name="lastName">The last name of the attendee. Optional.</param>
		/// <param name="address">The address of the attendee. Optional.</param>
		/// <param name="city">The city of the attendee. Optional.</param>
		/// <param name="state">The state of the attendee. Optional.</param>
		/// <param name="zip">The ZIP code of the attendee. Optional.</param>
		/// <param name="country">The country of the attendee. Optional.</param>
		/// <param name="phone">The phone number of the attendee. Optional.</param>
		/// <param name="industry">The industry of the attendee. Optional.</param>
		/// <param name="jobTitle">The job title of the attendee. Optional.</param>
		/// <param name="organization">The organization of the attendee. Optional.</param>
		/// <param name="comments">Additional comments about the attendee. Optional.</param>
		/// <param name="externalTicketId">An external identifier for the ticket. Optional.</param>
		/// <param name="customQuestions">A collection of custom questions and answers for the attendee. Optional.</param>
		/// <param name="source">The source of the ticket creation request. Optional.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
		/// <returns>A task that represents the asynchronous operation of creating a ticket.</returns>
		public static async Task<(EventTicket Ticket, EventTicketError Error)> CreateTicketAsync(this IEvents eventsResource, string eventId, string firstName = null, string lastName = null, string address = null, string city = null, string state = null, string zip = null, string country = null, string phone = null, string industry = null, string jobTitle = null, string organization = null, string comments = null, string externalTicketId = null, IEnumerable<KeyValuePair<string, string>> customQuestions = null, string source = null, CancellationToken cancellationToken = default)
		{
			var ticket = new EventTicket
			{
				FirstName = firstName,
				LastName = lastName,
				Address = address,
				City = city,
				State = state,
				Zip = zip,
				Country = country,
				Phone = phone,
				Industry = industry,
				JobTitle = jobTitle,
				Organization = organization,
				Comments = comments,
				ExternalTicketId = externalTicketId,
				CustomQuestions = customQuestions?.ToArray()
			};

			var result = await eventsResource.CreateTicketsAsync(eventId, new[] { ticket }, source, cancellationToken).ConfigureAwait(false);

			return (result.Tickets.FirstOrDefault(), result.Errors.FirstOrDefault());
		}

		/// <summary>
		/// Adds and configures a ZoomNet client to the specified <see cref="IServiceCollection"/>.
		/// </summary>
		/// <remarks>This method registers the ZoomNet client as a singleton service in the dependency injection
		/// container.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> to which the ZoomNet client will be added.</param>
		/// <param name="connectionInfo">The connection information used to authenticate with the Zoom API. This parameter cannot be <see
		/// langword="null"/>.</param>
		/// <param name="clientOptions">Optional configuration options for the ZoomNet client. If <see langword="null"/>, default options will be used.</param>
		/// <param name="httpClientName">The name of the <see cref="HttpClient"/> to be created. Defaults to "ZoomNet".</param>
		/// <returns>An <see cref="IHttpClientBuilder"/> that can be used to further configure the underlying <see cref="HttpClient"/>.</returns>
		public static IHttpClientBuilder AddZoomNet(this IServiceCollection services, IConnectionInfo connectionInfo, ZoomClientOptions clientOptions = null, string httpClientName = "ZoomNet")
		{
			return services.AddZoomNet(connectionInfo, null, clientOptions, httpClientName);
		}

		/// <summary>
		/// Adds and configures a ZoomNet client to the specified <see cref="IServiceCollection"/>.
		/// </summary>
		/// <remarks>This method registers the ZoomNet client as a singleton service in the dependency injection
		/// container. The <see cref="HttpClient"/> used by the ZoomNet client is configured with the specified proxy
		/// settings.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> to which the ZoomNet client will be added.</param>
		/// <param name="connectionInfo">The connection information used to authenticate with the Zoom API. This parameter cannot be <see
		/// langword="null"/>.</param>
		/// <param name="proxy">The <see cref="IWebProxy"/> to use for HTTP requests. If <see langword="null"/>, no proxy will be used.</param>
		/// <param name="clientOptions">Optional configuration options for the ZoomNet client. If <see langword="null"/>, default options will be used.</param>
		/// <param name="httpClientName">The name of the <see cref="HttpClient"/> to be created. Defaults to "ZoomNet".</param>
		/// <returns>An <see cref="IHttpClientBuilder"/> that can be used to further configure the underlying <see cref="HttpClient"/>.</returns>
		public static IHttpClientBuilder AddZoomNet(this IServiceCollection services, IConnectionInfo connectionInfo, IWebProxy proxy, ZoomClientOptions clientOptions = null, string httpClientName = "ZoomNet")
		{
			var httpClientBuilder = services
				.AddHttpClient(httpClientName)
				.RemoveAllLoggers() // No need for the built-in HttlClient logger(s). We rely on ZoomNet's custom logger instead.
				.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
				{
					Proxy = proxy,
					UseProxy = proxy != null,
				});

			// ConnectionInfo must be a singleton because the state of the connection (including the oAuth token) must be preserved between calls
			services.AddSingleton(connectionInfo);

			// ZoomClient can be transient since it does not carry state
			services.AddTransient<IZoomClient>(serviceProvider =>
			{
				var connectionInfo = serviceProvider.GetRequiredService<IConnectionInfo>();
				var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName);
				var logger = serviceProvider.GetRequiredService<ILogger<IZoomClient>>();
				return new ZoomClient(connectionInfo, httpClient, clientOptions, logger);
			});

			return httpClientBuilder;
		}

		/// <summary>
		/// Adds and configures a ZoomNet client to the specified <see cref="IServiceCollection"/>.
		/// </summary>
		/// <remarks>This method registers the ZoomNet client as a singleton service in the dependency injection
		/// container.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> to which the ZoomNet client will be added.</param>
		/// <param name="connectionInfo">The connection information used to authenticate with the Zoom API. This parameter cannot be <see langword="null"/>.</param>
		/// <param name="serviceKey">The key that identifies this particular ZoomNet client instance.</param>
		/// <param name="clientOptions">Optional configuration options for the ZoomNet client. If <see langword="null"/>, default options will be used.</param>
		/// <param name="httpClientName">The name of the <see cref="HttpClient"/> to be created. Defaults to "ZomNet".</param>
		/// <returns>An <see cref="IHttpClientBuilder"/> that can be used to further configure the underlying <see cref="HttpClient"/>.</returns>
		public static IHttpClientBuilder AddKeyedZoomNet(this IServiceCollection services, IConnectionInfo connectionInfo, string serviceKey, ZoomClientOptions clientOptions = null, string httpClientName = "ZoomNet")
		{
			return services.AddKeyedZoomNet(connectionInfo, serviceKey, null, clientOptions, httpClientName);
		}

		/// <summary>
		/// Adds and configures a ZoomNet client to the specified <see cref="IServiceCollection"/>.
		/// </summary>
		/// <remarks>This method registers the ZoomNet client as a singleton service in the dependency injection
		/// container. The <see cref="HttpClient"/> used by the ZoomNet client is configured with the specified proxy
		/// settings.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> to which the ZoomNet client will be added.</param>
		/// <param name="connectionInfo">The connection information used to authenticate with the Zoom API. This parameter cannot be <see langword="null"/>.</param>
		/// <param name="serviceKey">The key that identifies this particular ZoomNet client instance.</param>
		/// <param name="proxy">The <see cref="IWebProxy"/> to use for HTTP requests. If <see langword="null"/>, no proxy will be used.</param>
		/// <param name="clientOptions">Optional configuration options for the ZoomNet client. If <see langword="null"/>, default options will be used.</param>
		/// <param name="httpClientName">The name of the <see cref="HttpClient"/> to be created. Defaults to "ZomNet".</param>
		/// <returns>An <see cref="IHttpClientBuilder"/> that can be used to further configure the underlying <see cref="HttpClient"/>.</returns>
		public static IHttpClientBuilder AddKeyedZoomNet(this IServiceCollection services, IConnectionInfo connectionInfo, string serviceKey, IWebProxy proxy, ZoomClientOptions clientOptions = null, string httpClientName = "ZomNet")
		{
			var httpClientBuilder = services
				.AddHttpClient(httpClientName)
				.RemoveAllLoggers() // No need for the built-in HttlClient logger(s). We rely on ZoomNet's custom logger instead.
				.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
				{
					Proxy = proxy,
					UseProxy = proxy != null,
				});

			// ConnectionInfo must be a singleton because the state of the connection (including the oAuth token) must be preserved between calls
			services.AddKeyedSingleton(serviceKey, connectionInfo);

			// ZoomClient can be transient since it does not carry state
			services.AddKeyedTransient<IZoomClient>(serviceKey, (serviceProvider, keyOject) =>
			{
				var key = keyOject as string;
				var connectionInfo = serviceProvider.GetKeyedService<IConnectionInfo>(key);
				var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName);
				var logger = serviceProvider.GetRequiredService<ILogger<IZoomClient>>();
				return new ZoomClient(connectionInfo, httpClient, clientOptions, logger);
			});

			return httpClientBuilder;
		}
	}
}
