using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Chat : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ACCOUNT CHAT CHANNELS *****\n").ConfigureAwait(false);

			// GET THE CHANNELS FOR THIS USER
			var paginatedChannels = await client.Chat.GetAccountChannelsForUserAsync(myUser.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedChannels.TotalRecords} account channels for user {myUser.Id}").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedChannels.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldChannel =>
				{
					await client.Chat.DeleteChannelAsync(oldChannel.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Channel {oldChannel.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// CREATE A NEW CHANNEL
			var channel = await client.Chat.CreateAccountChannelAsync(myUser.Id, "ZoomNet Integration Testing: new channel", ChatChannelType.Public, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Name}\" created (Id={channel.Id}").ConfigureAwait(false);

			// UPDATE THE CHANNEL
			await client.Chat.UpdateAccountChannelAsync(myUser.Id, channel.Id, "ZoomNet Integration Testing: updated channel", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" updated").ConfigureAwait(false);

			// RETRIEVE THE CHANNEL
			channel = await client.Chat.GetAccountChannelAsync(myUser.Id, channel.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" retrieved").ConfigureAwait(false);

			// RETRIEVE THE CHANNEL MEMBERS
			var paginatedMembers = await client.Chat.GetAccountChannelMembersAsync(myUser.Id, channel.Id, 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" has {paginatedMembers.TotalRecords} members").ConfigureAwait(false);

			// SEND A MESSAGE TO THE CHANNEL
			var messageId = await client.Chat.SendMessageToChannelAsync(channel.Id, "This is a test from integration test", null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Message \"{messageId}\" sent").ConfigureAwait(false);
			await Task.Delay(1000, cancellationToken).ConfigureAwait(false); // Allow the Zoom system to process this message and avoid subsequent "message doesn't exist" error messages

			// UPDATE THE MESSAGE
			await client.Chat.UpdateMessageToChannelAsync(messageId, channel.Id, "This is an updated message from integration testing", null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Message \"{messageId}\" updated").ConfigureAwait(false);

			// RETRIEVE LIST OF MESSAGES
			var paginatedMessages = await client.Chat.GetMessagesToChannelAsync(channel.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedMessages.TotalRecords} messages in channel \"{channel.Id}\"").ConfigureAwait(false);

			// DELETE THE MESSAGE
			await client.Chat.DeleteMessageToChannelAsync(messageId, channel.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Message \"{messageId}\" deleted").ConfigureAwait(false);

			// DELETE THE CHANNEL
			await client.Chat.DeleteAccountChannelAsync(myUser.Id, channel.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" deleted").ConfigureAwait(false);
		}
	}
}
