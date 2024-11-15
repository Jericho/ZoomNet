using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
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
		/// Parses the event webhook asynchronously.
		/// </summary>
		/// <param name="parser">The webhook parser.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>An <see cref="Event" />.</returns>
		public static async Task<Event> ParseEventWebhookAsync(this IWebhookParser parser, Stream stream)
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
		/// <returns>An <see cref="Event" />.</returns>
		public static async Task<Event> VerifyAndParseEventWebhookAsync(this IWebhookParser parser, Stream stream, string secretToken, string signature, string timestamp)
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
		/// <returns>An <see cref="Event" />.</returns>
		public static Event VerifyAndParseEventWebhook(this IWebhookParser parser, string requestBody, string secretToken, string signature, string timestamp)
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
		/// Adds user to a group.
		/// </summary>
		/// <param name="groupsResource">The group resource.</param>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emailAddress">An email address of user to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the operation. The result will be a string representing the ID of the added user.</returns>
		public static async Task<string> AddUserToGroupAsync(this IGroups groupsResource, string groupId, string emailAddress, CancellationToken cancellationToken = default)
		{
			var result = await groupsResource.AddUsersToGroupAsync(groupId, new[] { emailAddress }, cancellationToken).ConfigureAwait(false);

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
	}
}
