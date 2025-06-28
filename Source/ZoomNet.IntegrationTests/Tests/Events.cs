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

			// CANCEL UPCOMING EVENTS
			var upComingEvents = await client.Events.GetAllAsync(UserRoleType.Host, EventListStatus.Upcoming, 100, null, cancellationToken).ConfigureAwait(false);
			var cleanUpTasks = upComingEvents.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldEvent =>
				{
					await client.Events.CancelEventAsync(oldEvent.Id, "Cleaning up upcoming evnts", cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Event {oldEvent.Id} cancelled").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// GET THE DRAFT EVENTS
			var draftEvents = await client.Events.GetAllAsync(UserRoleType.Host, EventListStatus.Draft, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {draftEvents.TotalRecords} draft events").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			cleanUpTasks = draftEvents.Records
				.Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldEvent =>
				{
					await client.Events.DeleteEventAsync(oldEvent.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Event {oldEvent.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// SIMPLE EVENT
			var start = DateTime.UtcNow.AddDays(1);
			var end = start.AddMinutes(30);
			var attendanceType = EventAttendanceType.Virtual;

			var newSimpleEvent = await client.Events.CreateSimpleEventAsync("ZoomNet Integration Testing: simple event", "The description", start, end, TimeZones.America_New_York, EventMeetingType.Meeting, hub.Id, true, attendanceType, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Simple event {newSimpleEvent.Id} created").ConfigureAwait(false);

			newSimpleEvent = (SimpleEvent)await client.Events.GetAsync(newSimpleEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Simple event retrieved").ConfigureAwait(false);

			await client.Events.PublishEventAsync(newSimpleEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The simple event has been published").ConfigureAwait(false);

			await client.Events.CancelEventAsync(newSimpleEvent.Id, "Cancelled for testing purposes", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The simple event has been cancelled").ConfigureAwait(false);

			// CONFERENCE
			start = DateTime.UtcNow.AddDays(14);
			end = start.AddMinutes(90);
			attendanceType = EventAttendanceType.InPerson;

			var newConference = await client.Events.CreateConferenceAsync("ZoomNet Integration Testing: conference", "The description", start, end, TimeZones.America_New_York, hub.Id, true, attendanceType, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Conference {newConference.Id} created").ConfigureAwait(false);

			newConference = (Conference)await client.Events.GetAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Conference retrieved").ConfigureAwait(false);

			var sponsorTiers = await client.Events.GetAllSponsorTiersAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {sponsorTiers.Length} sponsor tiers").ConfigureAwait(false);

			var newExhibitor = await client.Events.CreateExhibitorAsync(newConference.Id, "ZoomNet Integration Testing: Exhibitor", "John Doe", "john@example.com", true, sponsorTiers.First().Id, "This is the description", new[] { "QnjbUW7ORu2sjvjNfjf_zQ", "iERy5vUPRW259kk9l0zNbQ" }, "https://mywebsite.com/example", "https://mywebsite.com/example", "https://linkedin.com/example", "https://twitter.com/example", "https://youtube.com/example", "https://instagram.com/profile", "https://facebook.com/profile", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Exhibitor created").ConfigureAwait(false);

			var newSession = await client.Events.CreateSessionAsync(newConference.Id, "ZoomNet Integration Testing: Session", start, end, TimeZones.America_New_York, "This is the desciption", EventSessionType.Webinar, true, attendanceType: attendanceType, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Session {newSession.Id} created").ConfigureAwait(false);

			var ticketTypes = await client.Events.GetAllTicketTypesAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {ticketTypes.Length} ticket types").ConfigureAwait(false);

			await client.Events.PublishEventAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The conference has been published").ConfigureAwait(false);

			var tickets = new[]
			{
				new EventTicket
				{
					TypeId = ticketTypes.First(t => t.IsFree).Id,
					SendNotifications = true,
					FastJoin = true,
					RegistrationNeeded = false,
					FirstName = "Bob",
					LastName = "Smith",
					Email = "bob@example.com"
				},
				new EventTicket
				{
					TypeId = ticketTypes.First(t => t.IsFree).Id,
					SendNotifications = true,
					FastJoin = false,
					RegistrationNeeded = true,
					FirstName = "John",
					LastName = "Doe",
					Email = "john@example.com"
				}
			};

			tickets = await client.Events.CreateTicketsAsync(newConference.Id, tickets, "integration_test", cancellationToken).ConfigureAwait(false);

			var checkInErrors = await client.Events.CheckInAttendeesAsync(newConference.Id, newSession.Id, new[] { "bob@example.com", "john@example.com" }, "integration_tests", cancellationToken).ConfigureAwait(false);
			if (checkInErrors.Length > 0)
			{
				await log.WriteLineAsync($"There were {checkInErrors.Length} errors while checking in attendees:").ConfigureAwait(false);
				foreach (var error in checkInErrors)
				{
					await log.WriteLineAsync($"- {error.Email}: {error.ErrorMessage}").ConfigureAwait(false);
				}
			}
			else
			{
				await log.WriteLineAsync("All attendees checked in successfully").ConfigureAwait(false);
			}

			var actions = await client.Events.GetAllAttendeeActionsAsync(newConference.Id, "bob@example.com", 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved {actions.Records.Length} attendee actions for Bob Smith").ConfigureAwait(false);

			actions = await client.Events.GetAllAttendeeActionsAsync(newConference.Id, "john@example.com", 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved {actions.Records.Length} attendee actions for John Doe Smith").ConfigureAwait(false);
		}
	}
}
