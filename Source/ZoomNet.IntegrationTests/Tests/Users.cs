using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Users : IIntegrationTest
	{
		public async Task RunAsync(string userId, IClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** USERS *****\n").ConfigureAwait(false);

			// GET ALL THE USERS
			var paginatedUsers = await client.Users.GetAllAsync(UserStatus.Active, 100, 1, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedUsers.Records.Length} users").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			//var cleanUpTasks = paginatedUsers.Records
			//	.Where(m => m.FirstName.StartsWith("ZoomNet Integration Testing:"))
			//	.Select(async oldUsers =>
			//	{
			//		await client.Users.DeleteAsync(userId, oldUsers.Id, null, cancellationToken).ConfigureAwait(false);
			//		await log.WriteLineAsync($"User {oldUsers.Id} deleted").ConfigureAwait(false);
			//		await Task.Delay(250).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
			//	});
			//await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
