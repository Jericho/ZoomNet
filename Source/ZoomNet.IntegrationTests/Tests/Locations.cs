using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Locations : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** LOCATIONS *****\n").ConfigureAwait(false);

			// GET ALL THE LOCATIONS
			var paginatedLocations = await client.Locations.GetAllAsync(null, null, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedLocations.Records.Length} locations").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedLocations.Records
				.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldLocation =>
				{
					//await client.Locations.DeleteAsync(oldLocation.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Location {oldLocation.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
