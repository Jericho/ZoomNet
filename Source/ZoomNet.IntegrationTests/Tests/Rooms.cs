using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Rooms : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ROOMS *****\n").ConfigureAwait(false);

			// GET ALL THE ROOMS
			var paginatedRooms = await client.Rooms.GetAllAsync(null, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedRooms.Records.Length} rooms").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedRooms.Records
				.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldRoom =>
				{
					//await client.Rooms.DeleteAsync(oldRoom.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Rom {oldRoom.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
