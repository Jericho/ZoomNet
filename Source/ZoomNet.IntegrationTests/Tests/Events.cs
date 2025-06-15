using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Events : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** EVENTS *****\n").ConfigureAwait(false);

			// GET ALL THE HUBS
			var hubs = await client.Events.GetAllHubsAsync(UserRoleType.Host, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {hubs.Length} hubs").ConfigureAwait(false);

			var hub = hubs.First(h => h.IsActive);

			// GET ALL THE HUB HOSTS
			var hubHosts = await client.Events.GetAllHubHostsAsync(hub.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {hubHosts.TotalRecords} hosts for hub {hub.Id}").ConfigureAwait(false);

			// GET ALL THE HUB VIDEOS
			var hubVideos = await client.Events.GetAllHubVideosAsync(hub.Id, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {hubVideos.TotalRecords} videos for hub {hub.Id}").ConfigureAwait(false);

			// GET ALL THE EVENTS
			var events = await client.Events.GetAllAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {events.TotalRecords} events").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = events.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldEvent =>
				{
					//await client.Events.DeleteAsync(oldEvent.Id, null, false, false, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Event {oldEvent.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// SIMPLE EVENT
			var start = DateTime.UtcNow.AddMonths(1);
			var end = start.AddMinutes(35);
			var newSimpleEvent = await client.Events.CreateSimpleAsync("ZoomNet Integration Testing: simple event", "The description", start, end, TimeZones.America_New_York, EventMeetingType.Meeting, hub.Id, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Simple event {newSimpleEvent.Id} created").ConfigureAwait(false);

		}
	}
}
