using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Contacts : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CONTACTS *****\n").ConfigureAwait(false);

			// GET THE CONTACTS FOR THIS USER
			var paginatedContacts = await client.Contacts.GetUserContactsAsync(userId, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedContacts.TotalRecords} contacts for user {userId}").ConfigureAwait(false);
		}
	}
}
