using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Workspaces : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** WORKSPACES *****\n").ConfigureAwait(false);

			// GET ALL THE WORKSPACES
			var paginatedWorkspaces = await client.Workspaces.GetAllAsync(locationId, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedWorkspaces.TotalRecords} workspaces for location {locationId}").ConfigureAwait(false);

			//// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			//var cleanUpTasks = paginatedWebinars.Records
			//	.Union(paginatedWebinars.Records)
			//	.Where(m => m.Topic.StartsWith("ZoomNet Integration Testing:"))
			//	.Select(async oldWebinar =>
			//	{
			//		await client.Webinars.DeleteAsync(oldWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
			//		await log.WriteLineAsync($"Webinar {oldWebinar.Id} deleted").ConfigureAwait(false);
			//		await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
			//	});
			//await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
