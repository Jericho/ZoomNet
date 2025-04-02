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

			var paginatedRoomLocations = await client.Rooms.GetAllLocationsAsync(null, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedRoomLocations.Records.Length} room locations").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedRooms.Records
				.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldRoom =>
				{
					//await client.Rooms.DeleteAsync(oldRoom.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Room {oldRoom.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				})
				.Union(paginatedRoomLocations.Records
					.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
					.Select(async oldLocation =>
					{
						//await client.Rooms.DeleteLocationAsync(oldRoom.Id, cancellationToken).ConfigureAwait(false);
						await log.WriteLineAsync($"Room location {oldLocation.Id} deleted").ConfigureAwait(false);
						await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
					}));
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// CREATE A LOCATION HIERARCHY
			//var structure = new[] { RoomType.Country, RoomType.State, RoomType.City, RoomType.Building };
			//await client.Rooms.UpdateRoomsStructureAsync(structure, cancellationToken).ConfigureAwait(false);

			//paginatedRoomLocations = await client.Rooms.GetAllLocationsAsync(null, null, 100, null, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync($"There are {paginatedRoomLocations.Records.Length} room locations").ConfigureAwait(false);

			var country1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Country 1", string.Empty, cancellationToken).ConfigureAwait(false);
			var province1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Province 1", country1.Id, cancellationToken).ConfigureAwait(false);
			var province2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Province 2", country1.Id, cancellationToken).ConfigureAwait(false);
			var city1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: City 1", province1.Id, cancellationToken).ConfigureAwait(false);
			var city2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: City 2", province1.Id, cancellationToken).ConfigureAwait(false);
			var city3 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: City 3", province1.Id, cancellationToken).ConfigureAwait(false);
			var city4 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: City 4", province2.Id, cancellationToken).ConfigureAwait(false);
			var city5 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: City 5", province2.Id, cancellationToken).ConfigureAwait(false);
			var building1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 1", city1.Id, cancellationToken).ConfigureAwait(false);
			var building2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 2", city1.Id, cancellationToken).ConfigureAwait(false);
			var building3 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 3", city2.Id, cancellationToken).ConfigureAwait(false);
			var building4 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 4", city2.Id, cancellationToken).ConfigureAwait(false);
			var building5 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 5", city3.Id, cancellationToken).ConfigureAwait(false);
			var building6 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building 6", city4.Id, cancellationToken).ConfigureAwait(false);

		}
	}
}
