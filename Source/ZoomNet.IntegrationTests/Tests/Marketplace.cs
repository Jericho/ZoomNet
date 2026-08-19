using System.IO;
using System.Linq;
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
			var paginatedPublicApps = await client.Marketplace.GetPublicAppsAsync(300, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved {paginatedPublicApps.Records.Length} public apps on the marketplace").ConfigureAwait(false);

			var paginatedCreatedApps = await client.Marketplace.GetAccountAppsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"This account created {paginatedCreatedApps.TotalRecords} apps.").ConfigureAwait(false);

			// GET DETAILED INFO ABOUT AN APP
			if (paginatedCreatedApps.Records.Length > 0)
			{
				var appId = paginatedCreatedApps.Records.First().Id;
				var appInfo = await client.Marketplace.GetAppInfoAsync(appId, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Retrieved info about app: {appInfo.Name}").ConfigureAwait(false);
			}

			// GENERATE A DEEPLINK
			var deeplink = await client.Marketplace.GenerateAppDeeplinkAsync(DeeplinkType.Meeting, myUser.Id, "my-action", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Generated deeplink: {deeplink}").ConfigureAwait(false);

			// GET MY APP'S MANIFEST
			var myApp = paginatedCreatedApps.Records.SingleOrDefault(a => a.Name.Equals("General app 42", System.StringComparison.OrdinalIgnoreCase));
			if (myApp != null)
			{
				var manifest = await client.Marketplace.GetAppManifestAsync(myApp.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Retrieved manifest for app: {myApp.Name}").ConfigureAwait(false);

				await client.Marketplace.ValidateAppManifestAsync(myApp.Id, manifest, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Validated manifest for app: {myApp.Name}").ConfigureAwait(false);
			}

			// GET THE USER APP REQUESTS
			var paginatedPastUserRequests = await client.Marketplace.GetUserAppRequestsAsync(myUser.Id, AppRequestType.Active, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedPastUserRequests.TotalRecords} past user app requests.").ConfigureAwait(false);

			var paginatedActiveUserRequests = await client.Marketplace.GetUserAppRequestsAsync(myUser.Id, AppRequestType.Past, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedActiveUserRequests.TotalRecords} active user app requests.").ConfigureAwait(false);
		}
	}
}
