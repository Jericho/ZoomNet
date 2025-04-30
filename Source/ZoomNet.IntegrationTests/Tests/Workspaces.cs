using System.IO;
using System.Linq;
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

			var paginatedLocations = await client.Rooms.GetAllLocationsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
			if (paginatedLocations.TotalRecords == 0)
			{
				await log.WriteLineAsync("No locations found. Skipping workspaces test.").ConfigureAwait(false);
				return;
			}

			var location = paginatedLocations.Records.First(l => l.Type == RoomLocationType.Floor);

			var workspaceId = await client.Workspaces.CreateAsync("ZoomNet Integration Testing 1", WorkspaceType.Desk, location.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Workspace {workspaceId} created").ConfigureAwait(false);

			// GET ALL THE WORKSPACES
			var paginatedWorkspaces = await client.Workspaces.GetAllAsync(location.Id, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedWorkspaces.TotalRecords} workspaces for location {location.Id}").ConfigureAwait(false);
		}
	}
}
