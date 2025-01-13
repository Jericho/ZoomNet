using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class ExternalContacts : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** EXTERNAL CONTACTS *****\n").ConfigureAwait(false);

			// GET ALL THE EXTERNAL CONTACTS
			var paginatedContacts = await client.ExternalContacts.GetAllAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedContacts.TotalRecords} external contacts under the main account").ConfigureAwait(false);
		}
	}
}
