using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Marketplace : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** MARKETPLACE *****\n").ConfigureAwait(false);

			// GET THE USER'S ENTITLEMENTS
			var entitlements = await client.Marketplace.GetUserEntitlementsAsync(myUser.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"This user has {entitlements.Length} entitlements.").ConfigureAwait(false);

			// GET THE APPS
			var paginatedPublicApps = await client.Marketplace.GetPublicAppsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved {paginatedPublicApps.Records.Length} public apps on the marketplace").ConfigureAwait(false);

			var paginatedCreatedApps = await client.Marketplace.GetCreatedAppsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"This account created {paginatedCreatedApps.TotalRecords} apps.").ConfigureAwait(false);

			// GET THE APP REQUESTS
			var paginatedPastRequests = await client.Marketplace.GetPastAppRequestsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedPastRequests.TotalRecords} past app requests.").ConfigureAwait(false);

			var paginatedActiveRequests = await client.Marketplace.GetActiveAppRequestsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedActiveRequests.TotalRecords} active app requests.").ConfigureAwait(false);

			// GET THE USER APP REQUESTS
			var paginatedPastUserRequests = await client.Marketplace.GetPastUserAppRequestsAsync(myUser.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedPastUserRequests.TotalRecords} past user app requests.").ConfigureAwait(false);

			var paginatedActiveUserRequests = await client.Marketplace.GetActiveUserAppRequestsAsync(myUser.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedActiveUserRequests.TotalRecords} active user app requests.").ConfigureAwait(false);
		}
	}
}
