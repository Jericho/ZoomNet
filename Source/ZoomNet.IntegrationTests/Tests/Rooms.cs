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

			// GET ALL THE TAGS
			var paginatedTags = await client.Rooms.GetAllTagsAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedTags.Records.Length} tags").ConfigureAwait(false);

			// GET THE LOCATION STRUCTURE
			var currentStructure = await client.Rooms.GetLocationStructureAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Current location structure: {string.Join(", ", currentStructure.Select(s => s.ToEnumString()))}").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedTags.Records
				.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldTag =>
				{
					await client.Rooms.DeleteTagAsync(oldTag.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Tag {oldTag.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// Clean up rooms before locations is important because Zoom won't let you delete a location if there are rooms assigned to it.
			cleanUpTasks = paginatedRooms.Records
				.Where(r => r.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldRoom =>
				{
					await client.Rooms.DeleteAsync(oldRoom.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Room {oldRoom.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// Order is important: you must delete floors before buildings, buildings before campuses, campuses before cities, and so on.
			if (currentStructure.Any(l => l == RoomLocationType.Floor)) await CleanUpLocations(RoomLocationType.Floor, client, log, cancellationToken).ConfigureAwait(false);
			if (currentStructure.Any(l => l == RoomLocationType.Building)) await CleanUpLocations(RoomLocationType.Building, client, log, cancellationToken).ConfigureAwait(false);
			if (currentStructure.Any(l => l == RoomLocationType.Campus)) await CleanUpLocations(RoomLocationType.Campus, client, log, cancellationToken).ConfigureAwait(false);
			if (currentStructure.Any(l => l == RoomLocationType.City)) await CleanUpLocations(RoomLocationType.City, client, log, cancellationToken).ConfigureAwait(false);
			if (currentStructure.Any(l => l == RoomLocationType.State)) await CleanUpLocations(RoomLocationType.State, client, log, cancellationToken).ConfigureAwait(false);
			if (currentStructure.Any(l => l == RoomLocationType.Country)) await CleanUpLocations(RoomLocationType.Country, client, log, cancellationToken).ConfigureAwait(false);

			// CREATE A LOCATION HIERARCHY
			var desiredStructure = currentStructure
				.Union(new[] { RoomLocationType.Building, RoomLocationType.Floor })
				.Distinct()
				.ToArray();

			if (currentStructure.Length != desiredStructure.Length)
			{
				await client.Rooms.UpdateLocationStructureAsync(desiredStructure, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Added 'Building' and/or 'Floor' to the location structure").ConfigureAwait(false);

				currentStructure = await client.Rooms.GetLocationStructureAsync(cancellationToken).ConfigureAwait(false);
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
			await log.WriteLineAsync("Added a building to the location structure").ConfigureAwait(false);

			var buildingB = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Building B", parentId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Added a building to the location structure").ConfigureAwait(false);

			var floorA1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor A-1", buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Added a floor to the location structure").ConfigureAwait(false);

			var floorA2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor A-2", buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Added a floor to the location structure").ConfigureAwait(false);

			var floorB1 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor B-1", buildingB.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Added a floor to the location structure").ConfigureAwait(false);

			var floorB2 = await client.Rooms.CreateLocationAsync("ZoomNet Integration Testing: Floor B-2", buildingB.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Added a floor to the location structure").ConfigureAwait(false);

			await client.Rooms.MoveLocationASync(floorA2.Id, buildingB.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Floor A2 has been moved to building B").ConfigureAwait(false);

			var locationSettings = await client.Rooms.GetLocationSettingsAsync(buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("General settings for a location have been retrieved").ConfigureAwait(false);

			var alertSettings = await client.Rooms.GetLocationAlertSettingsAsync(buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Alert settings for a location have been retrieved").ConfigureAwait(false);

			var signageSettings = await client.Rooms.GetLocationSignageSettingsAsync(buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Signage settings for a location have been retrieved").ConfigureAwait(false);

			var schedulingDisplaySettings = await client.Rooms.GetLocationSchedulingDisplaySettingsAsync(buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Scheduling display settings for a location have been retrieved").ConfigureAwait(false);

			var tagId = await client.Rooms.CreateTagAsync("ZoomNet Integration Testing: Tag", "This is a test", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("A new room tag was created").ConfigureAwait(false);

			await client.Rooms.UpdateTagAsync(tagId, "ZoomNet Integration Testing: UPDATED Tag", "This is still a test", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Tag was updated").ConfigureAwait(false);

			await client.Rooms.UpdateLocationProfileAsync(buildingA.Id, description: "This is a test", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Profile for a location has been updated").ConfigureAwait(false);

			var profile = await client.Rooms.GetLocationProfileAsync(buildingA.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Profile for a location has been retrieved").ConfigureAwait(false);

			var room = await client.Rooms.CreateAsync("ZoomNet Integration Testing: Room", RoomType.Room, floorA1.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("A new room was created").ConfigureAwait(false);

			await client.Rooms.MoveAsync(room.Id, floorB1.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Room has been moved to a different location").ConfigureAwait(false);

			var devices = await client.Rooms.GetAllDevicesAsync(room.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {devices.Length} devices in the room").ConfigureAwait(false);

			await client.Rooms.GetDevicesInformationAsync(room.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved devices information").ConfigureAwait(false);

			await client.Rooms.CreateDeviceProfileAsync(room.Id, true, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("A new device profile was created").ConfigureAwait(false);

			//await client.Rooms.UpdateAsync(room.Id, "ZoomNet Integration Testing: UPDATED Room", floorA2.Id, cancellationToken).ConfigureAwait(false);
			//await log.WriteLineAsync("Room was updated").ConfigureAwait(false);

			await client.Rooms.AssignTagToRoom(room.Id, tagId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Room tag has been assigned").ConfigureAwait(false);

			await client.Rooms.UnAssignTagFromRoom(room.Id, tagId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Room tag has been removed from room").ConfigureAwait(false);

			await client.Rooms.DeleteTagAsync(tagId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Room tag has been deleted").ConfigureAwait(false);

			await client.Rooms.DeleteAsync(room.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Room has been deleted").ConfigureAwait(false);
		}

		private static async Task CleanUpLocations(RoomLocationType locationType, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			var paginatedLocations = await client.Rooms.GetAllLocationsAsync(null, locationType, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedLocations.TotalRecords} locations of type '{locationType.ToEnumString()}'").ConfigureAwait(false);

			var cleanUpTasks = paginatedLocations.Records
				.Where(l => l.Name.StartsWith("ZoomNet Integration Testing:", StringComparison.OrdinalIgnoreCase))
				.Select(async oldLocation =>
				{
					await client.Rooms.DeleteLocationAsync(oldLocation.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Location {oldLocation.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
