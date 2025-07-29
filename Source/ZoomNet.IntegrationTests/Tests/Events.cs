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

			// SIMPLE EVENT
			var eventStart = DateTime.UtcNow.AddDays(1);
			var eventEnd = eventStart.AddMinutes(30);
			var attendanceType = EventAttendanceType.Virtual;

			var newSimpleEvent = await client.Events.CreateSimpleEventAsync(
				"ZoomNet Integration Testing: simple event",
				"The description",
				eventStart,
				eventEnd,
				TimeZones.America_New_York,
				EventMeetingType.Meeting,
				hub.Id,
				true,
				attendanceType,
				new[] { EventCategory.CommunityAndSpirituality },
				new[] { "category1", "cat2" },
				"Rembrandt van Rijn",
				eventStart.AddMinutes(-15), // Lobby opens 15 minutes before the event starts
				eventEnd.AddMinutes(15), // Lobby closes 15 minutes after the event ends
				new[] { Country.France, Country.Germany },
				"The best conference ever !!!",
				cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Simple event {newSimpleEvent.Id} created").ConfigureAwait(false);

			await client.Events.UpdateSimpleEventAsync(newSimpleEvent.Id, description: "This description has been updated", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Simple event updated").ConfigureAwait(false);

			newSimpleEvent = (SimpleEvent)await client.Events.GetAsync(newSimpleEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Simple event retrieved").ConfigureAwait(false);

			var duplicatedSimpleEventId = await client.Events.DuplicateEventAsync(newSimpleEvent.Id, "ZoomNet Integration Testing: duplicated simple event", eventStart.AddDays(1), TimeZones.America_New_York, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Simple event duplicated").ConfigureAwait(false);

			await client.Events.PublishEventAsync(newSimpleEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The simple event has been published").ConfigureAwait(false);

			await client.Events.CancelEventAsync(newSimpleEvent.Id, "Cancelled for testing purposes", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The simple event has been cancelled").ConfigureAwait(false);

			await client.Events.DeleteEventAsync(newSimpleEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The simple event has been deleted").ConfigureAwait(false);

			await client.Events.DeleteEventAsync(duplicatedSimpleEventId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The duplicated simple event has been deleted").ConfigureAwait(false);

			// RECURRING EVENT
			var recurrenceInfo = new EventRecurrenceInfo()
			{
				EndDateTime = DateTime.Now.AddMonths(2),
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Friday },
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				Duration = 45
			};
			eventStart = DateTime.UtcNow.AddDays(2);
			eventEnd = eventStart.AddHours(2);
			attendanceType = EventAttendanceType.Hybrid;

			var newRecurringEvent = await client.Events.CreateRecurringEventAsync(
				"ZoomNet Integration Testing: recurring event",
				"The description",
				eventStart,
				eventEnd,
				recurrenceInfo,
				TimeZones.America_New_York,
				hub.Id,
				true,
				attendanceType,
				new[] { EventCategory.BusinessAndNetworking },
				new[] { "cat2", "cat3" },
				"Jan Vermeer",
				eventStart.AddMinutes(-15), // Lobby opens 15 minutes before the event starts
				eventEnd.AddMinutes(15), // Lobby closes 15 minutes after the event ends
				new[] { Country.Chad, Country.Norway },
				"The best recurring event ever !!!",
				cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring event {newRecurringEvent.Id} created").ConfigureAwait(false);

			await client.Events.UpdateRecurringEventAsync(newRecurringEvent.Id, description: "This description has been updated", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Recurring event updated").ConfigureAwait(false);

			await client.Events.DeleteEventAsync(newRecurringEvent.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The recurring event has been deleted").ConfigureAwait(false);

			// CONFERENCE
			var conferenceCalendar = new[] // The conference is a two-day event
			{
				(Start: DateTime.UtcNow.AddDays(7), End: DateTime.UtcNow.AddDays(7).AddHours(2)),
				(Start: DateTime.UtcNow.AddDays(8), End: DateTime.UtcNow.AddDays(8).AddHours(3))
			};
			attendanceType = EventAttendanceType.InPerson;

			var newConference = await client.Events.CreateConferenceAsync(
				"ZoomNet Integration Testing: conference",
				"The description",
				conferenceCalendar,
				TimeZones.America_New_York,
				hub.Id,
				true,
				attendanceType,
				new[] { EventCategory.BusinessAndNetworking },
				new[] { "cat2", "cat3" },
				"Frans Hals",
				conferenceCalendar.OrderBy(c => c.Start).First().Start.AddMinutes(-15), // Lobby opens 15 minutes before the first session
				conferenceCalendar.OrderBy(c => c.End).Last().End.AddMinutes(15), // Lobby closes 15 minutes after the last session
				new[] { Country.Chad, Country.Norway, Country.India },
				"The best conference ever !!!",
				cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Conference {newConference.Id} created").ConfigureAwait(false);

			await client.Events.UpdateConferenceAsync(newConference.Id, description: "This description has been updated", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Conference updated").ConfigureAwait(false);

			newConference = (Conference)await client.Events.GetAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Conference retrieved").ConfigureAwait(false);

			var sponsorTiers = await client.Events.GetAllSponsorTiersAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {sponsorTiers.Length} sponsor tiers").ConfigureAwait(false);

			var newExhibitor = await client.Events.CreateExhibitorAsync(newConference.Id, "ZoomNet Integration Testing: Exhibitor", "John Doe", "john@example.com", true, sponsorTiers.First().Id, "This is the description", new[] { "QnjbUW7ORu2sjvjNfjf_zQ", "iERy5vUPRW259kk9l0zNbQ" }, "https://mywebsite.com/example", "https://mywebsite.com/example", "https://linkedin.com/example", "https://twitter.com/example", "https://youtube.com/example", "https://instagram.com/profile", "https://facebook.com/profile", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Exhibitor created").ConfigureAwait(false);

			var newSpeaker = await client.Events.CreateSpeakerAsync(newConference.Id, "ZoomNet Integration Testing: Speaker", "joe@example.com", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Speaker created").ConfigureAwait(false);

			await client.Events.UpdateSpeakerAsync(newConference.Id, newSpeaker.Id, name: "ZoomNet Integration Testing: updated speaker name", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Speaker updated").ConfigureAwait(false);

			newSpeaker = await client.Events.GetSpeakerAsync(newConference.Id, newSpeaker.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Speaker {newSpeaker.Id} retrieved").ConfigureAwait(false);

			var newSession = await client.Events.CreateSessionAsync(
				newConference.Id,
				"ZoomNet Integration Testing: First day session",
				conferenceCalendar.OrderBy(c => c.Start).First().Start,
				conferenceCalendar.OrderBy(c => c.Start).First().End,
				TimeZones.America_New_York,
				"Session that takes place on the first day of the conference",
				EventSessionType.Webinar,
				attendanceType: attendanceType,
				allowReservations: true,
				maxCapacity: 100,
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Session {newSession.Id} created").ConfigureAwait(false);

			await client.Events.UpdateSessionAsync(
				newConference.Id,
				newSession.Id,
				name: "ZoomNet Integration Testing: updated session name",
				description: "Updated session description",
				speakers: new[] { (newSpeaker.Id, false, true, true) },
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Session {newSession.Id} updated").ConfigureAwait(false);

			newSession = await client.Events.GetSessionAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Session {newSession.Id} retrieved").ConfigureAwait(false);

			var allSessions = await client.Events.GetAllSessionsAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"All sessions retrieved. There are {allSessions.Length} sessions.").ConfigureAwait(false);

			var ticketTypes = await client.Events.GetAllTicketTypesAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {ticketTypes.Length} ticket types").ConfigureAwait(false);

			foreach (var ticketType in ticketTypes)
			{
				await client.Events.DeleteTicketTypeAsync(newConference.Id, ticketType.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Ticket type {ticketType.Name} deleted").ConfigureAwait(false);
			}

			var newTicketTypeId = await client.Events.CreateTicketTypeAsync(
				newConference.Id,
				"Backstage pass",
				DateTime.Now, // Can start selling these tickets right away
				conferenceCalendar.OrderBy(c => c.Start).First().Start, // Stop selling these tickets when the conference starts
				"CAD",
				null,
				50,
				"Allows backstage access during the conference",
				0,
				null,
				cancellationToken
			).ConfigureAwait(false);
			await log.WriteLineAsync($"Ticket type {newTicketTypeId} created").ConfigureAwait(false);

			await client.Events.UpdateTicketTypeAsync(newConference.Id, newTicketTypeId, name: "VIP pass", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Ticket type {newTicketTypeId} updated").ConfigureAwait(false);

			var registrationQuestions = new[]
			{
				new EventRegistrationQuestion
				{
					FieldName = EventRegistrationField.City,
					IsRequired = true,
					Title = "What city do you live in?",
				},
				new EventRegistrationQuestion
				{
					FieldName = EventRegistrationField.JobTitle,
					IsRequired = true,
					Title = "What is your job title?",
				},
			};

			var customQuestions = new[]
			{
				new EventRegistrationCustomQuestion
				{
					FieldName = "favorite_color",
					IsRequired = false,
					Title = "What is your favorite color?",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText
				},
				new EventRegistrationCustomQuestion
				{
					FieldName = "favorite_food",
					IsRequired = false,
					Title = "What is your favorite food?",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText
				}
			};

			await client.Events.UpdateRegistrationQuestionsForTicketTypeAsync(newConference.Id, newTicketTypeId, registrationQuestions, customQuestions, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Registration questions for the ticket type updated").ConfigureAwait(false);

			var conferenceRegistrationQuestions = await client.Events.GetRegistrationQuestionsForTicketTypeAsync(newConference.Id, newTicketTypeId, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registration contains {conferenceRegistrationQuestions.StandardQuestions.Length} standard questions and {conferenceRegistrationQuestions.CustomQuestions.Length} custom questions").ConfigureAwait(false);

			await client.Events.PublishEventAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The conference has been published").ConfigureAwait(false);

			await Task.Delay(500, cancellationToken).ConfigureAwait(false); // Wait a bit to ensure the event is published

			var accessLink = await client.Events.CreateEventAccessLinkAsync(newConference.Id,
				"ZoomNet Integration Testing: Access Link",
				authenticationMethod: EventAuthenticationMethod.ZoomAccount,
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Access link created: {accessLink.Url}").ConfigureAwait(false);

			await client.Events.UpdateEventAccessLinkAsync(newConference.Id, accessLink.Id,
				name: "ZoomNet Integration Testing: updated name",
				ticketTypeId: newTicketTypeId,
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Access link updated").ConfigureAwait(false);

			accessLink = await client.Events.GetEventAccessLinkAsync(newConference.Id, accessLink.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Access link retrieved").ConfigureAwait(false);

			var accessLinks = await client.Events.GetAllEventAccessLinksAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Access links retrieved. There are {accessLinks.Length} links.").ConfigureAwait(false);

			await client.Events.DeleteEventAccessLinkAsync(newConference.Id, accessLink.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Access link deleted").ConfigureAwait(false);

			await client.Events.UpsertSessionInterpretersAsync(
				newConference.Id,
				newSession.Id,
				new[]
				{
					("languageInterpreter1@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.Portuguese),
					("languageInterpreter2@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.German),
					("languageInterpreter3@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.Spanish),
				},
				new[]
				{
					("signInterpreter1@example.com", InterpretationSignLanguage.French),
					("signInterpreter2@example.com", InterpretationSignLanguage.British),
				},
				cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session language interpreters added").ConfigureAwait(false);

			var interpreters = await client.Events.GetAllSessionInterpretersAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved all interpreters. There are {interpreters.Length} interpreters").ConfigureAwait(false);

			await client.Events.AddCoEditorsAsync(
				newConference.Id,
				new[]
				{
					("coeditor1@example.com", new EventCoEditorPermissionGroup[] { EventCoEditorPermissionGroup.Venue }),
					("anothereditor@example.com", new[] { EventCoEditorPermissionGroup.EventPlanning, EventCoEditorPermissionGroup.EventConfiguration }),
				},
				cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Co-editors added").ConfigureAwait(false);

			var coeditors = await client.Events.GetAllCoEditorsAsync(newConference.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved all co-editors. There are {coeditors.Length} co-editors").ConfigureAwait(false);

			var tickets = new[]
			{
				new EventTicket
				{
					TypeId = newTicketTypeId,
					SendNotifications = false,
					FastJoin = true,
					RegistrationNeeded = false,
					FirstName = "Bob",
					LastName = "Smith",
					City = "Paris",
					JobTitle = "Software Engineer",
					Email = "bob@example.com"
				},
				new EventTicket
				{
					TypeId = newTicketTypeId,
					SendNotifications = false,
					FastJoin = true,
					RegistrationNeeded = false,
					FirstName = "John",
					LastName = "Doe",
					City = "Berlin",
					JobTitle = "Product Manager",
					Email = "john@example.com"
				}
			};

			var createTicketsResult = await client.Events.CreateTicketsAsync(newConference.Id, tickets, "integration_test", cancellationToken).ConfigureAwait(false);
			if (createTicketsResult.Errors.Length > 0)
			{
				await log.WriteLineAsync($"There were {createTicketsResult.Errors.Length} errors when creating tickets:").ConfigureAwait(false);
				foreach (var error in createTicketsResult.Errors)
				{
					await log.WriteLineAsync($"- {error.Email}: ({error.ErrorCode}) {error.Message}").ConfigureAwait(false);
				}
			}
			else
			{
				await log.WriteLineAsync("All tickets created successfully").ConfigureAwait(false);
			}

			var bobTicket = createTicketsResult.Tickets.FirstOrDefault(t => t.Email == "bob@example.com");
			if (bobTicket != null)
			{
				await client.Events.DeleteTicketAsync(newConference.Id, bobTicket.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Ticket {bobTicket.Id} deleted").ConfigureAwait(false);
			}

			var johnTicket = createTicketsResult.Tickets.FirstOrDefault(t => t.Email == "john@example.com");
			if (johnTicket != null)
			{
				johnTicket = await client.Events.GetTicketAsync(newConference.Id, johnTicket.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Ticket {johnTicket.Id} retrieved").ConfigureAwait(false);
			}

			// We previously deleted Bob's ticket, so we expect an error when trying check in this attendee
			var checkInErrors = await client.Events.CheckInAttendeesAsync(newConference.Id, new[] { "bob@example.com", "john@example.com" }, "integration_tests", cancellationToken).ConfigureAwait(false);
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
			await log.WriteLineAsync($"Retrieved {actions.Records.Length} attendee actions for John Doe").ConfigureAwait(false);

			await client.Events.AddSessionReservationAsync(newConference.Id, newSession.Id, johnTicket.Email, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Reserved a spot for {johnTicket.Email} in session {newSession.Id}").ConfigureAwait(false);

			var reservations = await client.Events.GetAllSessionReservationsAsync(newConference.Id, newSession.Id, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {reservations.Records.Length} reservations for session {newSession.Id}").ConfigureAwait(false);

			await client.Events.UpdateSessionLivestreamConfigurationAsync(newConference.Id, newSession.Id, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session livestream enabled").ConfigureAwait(false);

			var livestreamConfig = await client.Events.GetSessionLivestreamConfgurationAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session livestream configuration retrieved").ConfigureAwait(false);

			await client.Events.UpdateSessionLivestreamConfigurationAsync(newConference.Id, newSession.Id, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session livestream disabled").ConfigureAwait(false);

			var joinToken = await client.Events.GetSessionJoinTokenAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Session join token: {joinToken}").ConfigureAwait(false);

			var poll1 = new PollForEventSession
			{
				Title = "ZoomNet Integration Testing: Basic Poll",
				Type = PollType.Basic,
				Status = PollStatusForEventSession.Active,
				AllowAnonymous = false,
				Questions = new[]
				{
					new PollQuestionForEventSession
					{
						Question = "What is your favorite programming language?",
						Type = PollQuestionType.SingleChoice,
						Answers = new[] { "C#", "JavaScript", "Python" },
					},
					new PollQuestionForEventSession
					{
						Question = "What is your favorite IDE?",
						Type = PollQuestionType.SingleChoice,
						Answers = new[] { "Visual Studio", "VS Code", "IntelliJ IDEA" },
					},
				}
			};

			var poll2 = new PollForEventSession
			{
				Title = "ZoomNet Integration Testing: Advanced Poll",
				Type = PollType.Basic, // I am deliberatly setting the poll type to a value that I know is not valid (due to the fact the poll contains 'Advanced' questions) to verify that this value gets overridden in CreateSessionPollAsync
				Status = PollStatusForEventSession.Active,
				AllowAnonymous = false,
				Questions = new[]
				{
					new PollQuestionForEventSession
					{
						Question = "What is your pet's name?",
						Type = PollQuestionType.Short,
					},
					new PollQuestionForEventSession
					{
						Question = "Tell us about yourself",
						Type = PollQuestionType.Long,
					},
					new PollQuestionForEventSession
					{
						Question = "Which colors do you like?",
						Type = PollQuestionType.MultipleChoice,
						Answers = new[] { "Red", "Green", "Blue", "Yellow" },
					},
					new PollQuestionForEventSession
					{
						Question = "Rank these movies",
						Type = PollQuestionType.RankOrder,
						Prompts = new[] { "Zoolander", "Dodge Ball", "Old School", "Elf", "Anchorman" },
						PromptCorrectAnswers = new[] { "Funny", "Neutral", "Boring" },
					},
					new PollQuestionForEventSession
					{
						Question = "The quick brown <blank1 /> jumps over the lazy <blank2 />",
						Answers = new[] { "Answer 1", "Answer 2" },
						Type = PollQuestionType.FillInTheBlanks,
					},
					new PollQuestionForEventSession
					{
						Question = "Match the cars to their style",
						Prompts = new[] { "Ford F150", "Tesla Model 3", "Ford Explorer" },
						PromptCorrectAnswers = new[] { "Pickup truck", "Electric Vehicule", "SUV" },
						Type = PollQuestionType.Matching,
					},
					new PollQuestionForEventSession
					{
						Question = "How likely are you to recommend us?",
						RatingLowScoreLabel = "Not likely",
						RatingHighScoreLabel = "Very likely",
						RatingMinimumValue = 1,
						RatingMaximumValue = 10,
						Type = PollQuestionType.RatingScale,
					}
				}
			};

			await client.Events.UpsertSessionPollsAsync(
				newConference.Id,
				newSession.Id,
				new[] { poll1, poll2 },
				cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session poll created").ConfigureAwait(false);

			var polls = await client.Events.GetAllSessionPollsAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {polls.Length} polls for session {newSession.Id}").ConfigureAwait(false);

			await client.Events.DeleteSessionReservationAsync(newConference.Id, newSession.Id, johnTicket.Email, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Deleted reservation for {johnTicket.Email} in session {newSession.Id}").ConfigureAwait(false);

			await client.Events.DeleteSpeakerAsync(newConference.Id, newSpeaker.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Speaker deleted").ConfigureAwait(false);

			await client.Events.DeleteExhibitorAsync(newConference.Id, newExhibitor.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Exhibitor deleted").ConfigureAwait(false);

			await client.Events.DeleteSessionAsync(newConference.Id, newSession.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Session deleted").ConfigureAwait(false);

			await client.Events.CancelEventAsync(newConference.Id, "Cancelled for testing purposes", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("The conference has been cancelled").ConfigureAwait(false);
		}
	}
}
