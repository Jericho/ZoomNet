using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Chat : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ACCOUNT CHAT CHANNELS *****\n").ConfigureAwait(false);

			// GET THE CHANNELS FOR THIS USER
			var paginatedChannels = await client.Chat.GetAccountChannelsForUserAsync(userId, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedChannels.TotalRecords} account channels for user {userId}").ConfigureAwait(false);

			// CREATE A NEW CHANNEL
			var channel = await client.Chat.CreateAccountChannelAsync(userId, "INTEGRATION TESTING: new channel", ChatChannelType.Public, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Name}\" created (Id={channel.Id}").ConfigureAwait(false);

			// UPDATE THE CHANNEL
			await client.Chat.UpdateAccountChannelAsync(userId, channel.Id, "INTEGRATION TESTING: updated channel", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" updated").ConfigureAwait(false);

			// RETRIEVE THE CHANNEL
			channel = await client.Chat.GetAccountChannelAsync(userId, channel.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" retrieved").ConfigureAwait(false);

			// RETRIEVE THE CHANNEL MEMBERS
			var paginatedMembers = await client.Chat.GetAccountChannelMembersAsync(userId, channel.Id, 10, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" has {paginatedMembers.TotalRecords} members").ConfigureAwait(false);

			// DELETE THE CHANNEL
			await client.Chat.DeleteAccountChannelAsync(userId, channel.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Account channel \"{channel.Id}\" deleted").ConfigureAwait(false);
		}
	}
}
