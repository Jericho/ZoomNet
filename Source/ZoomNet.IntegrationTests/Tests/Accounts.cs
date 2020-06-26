using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Accounts : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ACCOUNTS *****\n").ConfigureAwait(false);

			// GET ALL THE ACCOUNTS
			var paginatedAccounts = await client.Accounts.GetAllAsync(100, 1, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedAccounts.TotalRecords} sub accounts under the main account").ConfigureAwait(false);
		}
	}
}
