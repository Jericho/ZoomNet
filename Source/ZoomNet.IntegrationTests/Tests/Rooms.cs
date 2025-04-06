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
					await client.Rooms.DeleteAsync(oldRoom.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Room {oldRoom.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			await CleanUpLocations(RoomLocationType.Floor, client, log, cancellationToken).ConfigureAwait(false);
			await CleanUpLocations(RoomLocationType.Building, client, log, cancellationToken).ConfigureAwait(false);
			await CleanUpLocations(RoomLocationType.Campus, client, log, cancellationToken).ConfigureAwait(false);
			await CleanUpLocations(RoomLocationType.City, client, log, cancellationToken).ConfigureAwait(false);
			await CleanUpLocations(RoomLocationType.State, client, log, cancellationToken).ConfigureAwait(false);
			await CleanUpLocations(RoomLocationType.Country, client, log, cancellationToken).ConfigureAwait(false);

			// CREATE A LOCATION HIERARCHY
			var currentStructure = await client.Rooms.GetRoomLocationStructureAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Current location structure: {string.Join(", ", currentStructure.Select(s => s.ToEnumString()))}").ConfigureAwait(false);

			var desiredStructure = currentStructure
				.Union(new[] { RoomLocationType.Building, RoomLocationType.Floor })
				.Distinct()
				.ToArray();

			if (currentStructure.Length != desiredStructure.Length)
			{
				await client.Rooms.UpdateRoomLocationStructureAsync(desiredStructure, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Added 'Building' and/or 'Floor' to the location structure").ConfigureAwait(false);

				currentStructure = await client.Rooms.GetRoomLocationStructureAsync(cancellationToken).ConfigureAwait(false);
			}

			var parentId = myUser.AccountId;
			if (currentStructure.Contains(RoomLocationType.Country))
			{
				var country = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Country", parentId, cancellationToken).ConfigureAwait(false);
				parentId = country.Id;
				await log.WriteLineAsync("Added a country to the location structure").ConfigureAwait(false);
			}

			if (currentStructure.Contains(RoomLocationType.State))
			{
				var state = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: State", parentId, cancellationToken).ConfigureAwait(false);
				parentId = state.Id;
				await log.WriteLineAsync("Added a state to the location structure").ConfigureAwait(false);
			}

			if (currentStructure.Contains(RoomLocationType.Campus))
			{
				var state = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Campus", parentId, cancellationToken).ConfigureAwait(false);
				parentId = state.Id;
				await log.WriteLineAsync("Added a campus to the location structure").ConfigureAwait(false);
			}

			var buildingA = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building A", parentId, cancellationToken).ConfigureAwait(false);
			var buildingB = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building B", parentId, cancellationToken).ConfigureAwait(false);
			var floorA1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor A-1", buildingA.Id, cancellationToken).ConfigureAwait(false);
			var floorA2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor A-2", buildingA.Id, cancellationToken).ConfigureAwait(false);
			var floorB1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor B-1", buildingB.Id, cancellationToken).ConfigureAwait(false);
			var floorB2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor B-2", buildingB.Id, cancellationToken).ConfigureAwait(false);
		}

		private async Task CleanUpLocations(RoomLocationType locationType, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			var paginatedLocations = await client.Rooms.GetAllLocationsAsync(null, locationType, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedLocations.TotalRecords} room locations of type '{locationType.ToEnumString()}'").ConfigureAwait(false);

			var cleanUpTasks = paginatedLocations.Records
				.Where(l => l.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldLocation =>
				{
					await client.Rooms.DeleteLocationAsync(oldLocation.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Room {oldLocation.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
