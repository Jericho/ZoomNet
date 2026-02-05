using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class EventsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public EventsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region Attendee Actions Tests

		[Fact]
		public async Task GetAllAttendeeActionsAsync()
		{
			// Arrange
			var eventId = "event123";
			var attendeeEmail = "attendee@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "attendee_action"))
				.WithQueryString("email", attendeeEmail)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__attendee_action_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllAttendeeActionsAsync(eventId, attendeeEmail, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CheckInAttendeesAsync()
		{
			// Arrange
			var eventId = "event123";
			var attendeeEmails = new[] { "attendee1@example.com", "attendee2@example.com" };
			var source = "manual";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "attendee_action"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__attendee_action_PATCH);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CheckInAttendeesAsync(eventId, attendeeEmails, source, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CheckInAttendeesAsync_ForSession()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";
			var attendeeEmails = new[] { "attendee1@example.com", "attendee2@example.com" };
			var source = "manual";
			var errorsJson = @"{""errors"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "attendee_action"))
				.Respond("application/json", errorsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CheckInAttendeesAsync(eventId, sessionId, attendeeEmails, source, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		#endregion

		#region Co-Editors Tests

		[Fact]
		public async Task GetAllCoEditorsAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__coeditors_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllCoEditorsAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		[Fact]
		public async Task AddCoEditorsAsync()
		{
			// Arrange
			var eventId = "event123";
			var coeditors = new[]
			{
				(EmailAddress: "coeditor@example.com", Permissions: new[] { EventCoEditorPermissionGroup.Publish })
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.AddCoEditorsAsync(eventId, coeditors, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddCoEditorsAsync_MultipleCoEditorsWithMultiplePermissions()
		{
			// Arrange
			var eventId = "event123";
			var coeditors = new[]
			{
				(EmailAddress: "coeditor1@example.com", Permissions: new[] { EventCoEditorPermissionGroup.Publish, EventCoEditorPermissionGroup.EventConfiguration }),
				(EmailAddress: "coeditor2@example.com", Permissions: new[] { EventCoEditorPermissionGroup.EventBranding, EventCoEditorPermissionGroup.RegistrationAndJoin })
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.AddCoEditorsAsync(eventId, coeditors, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateCoEditorsAsync()
		{
			// Arrange
			var eventId = "event123";
			var coeditors = new List<(string EmailAddress, IEnumerable<EventCoEditorPermissionGroup> Permissions)>()
			{
				(EmailAddress: "coeditor@example.com", Permissions: new[] { EventCoEditorPermissionGroup.EventBranding, EventCoEditorPermissionGroup.Venue })
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateCoEditorsAsync(eventId, coeditors, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateCoEditorsAsync_MultipleCoEditors()
		{
			// Arrange
			var eventId = "event123";
			var coeditors = new[]
			{
				(EmailAddress: "coeditor1@example.com", Permissions: (IEnumerable<EventCoEditorPermissionGroup>)new[] { EventCoEditorPermissionGroup.Publish }),
				(EmailAddress: "coeditor2@example.com", Permissions: (IEnumerable<EventCoEditorPermissionGroup>)new[] { EventCoEditorPermissionGroup.EventConfiguration, EventCoEditorPermissionGroup.EventPlanning })
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateCoEditorsAsync(eventId, coeditors, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteCoEditorsAsync()
		{
			// Arrange
			var eventId = "event123";
			var emailAddresses = new[] { "coeditor@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteCoEditorsAsync(eventId, emailAddresses, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteCoEditorsAsync_MultipleCoEditors()
		{
			// Arrange
			var eventId = "event123";
			var emailAddresses = new[] { "coeditor1@example.com", "coeditor2@example.com", "coeditor3@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteCoEditorsAsync(eventId, emailAddresses, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Event Access Tests

		[Fact]
		public async Task CreateEventAccessLinkAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Registration Link";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__access_links_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateEventAccessLinkAsync(eventId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("234kjhg23kl4jhlaksjdh3");
			result.Name.ShouldBe("Event Access Link 1");
		}

		[Fact]
		public async Task CreateEventAccessLinkAsync_WithAllParameters()
		{
			// Arrange
			var eventId = "event123";
			var name = "Group Join Link";
			var type = EventAccessLinkType.GroupJoin;
			var authMethod = EventAuthenticationMethod.EmailOpt;
			var isDefault = true;
			var allowDomains = new[] { "example.com", "test.com" };
			var emailRestrict = new[] { "user@example.com" };
			var emailAuth = false;
			var securityCodeVerification = false;
			var ticketTypeId = "ticket456";
			var recurringType = RecurringEventRegistrationType.SingleSession;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__access_links_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateEventAccessLinkAsync(
				eventId,
				name,
				type,
				authMethod,
				isDefault,
				allowDomains,
				emailRestrict,
				emailAuth,
				securityCodeVerification,
				ticketTypeId,
				recurringType,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteEventAccessLinkAsync()
		{
			// Arrange
			var eventId = "event123";
			var accessLinkId = "link123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteEventAccessLinkAsync(eventId, accessLinkId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetEventAccessLinkAsync()
		{
			// Arrange
			var eventId = "event123";
			var accessLinkId = "link123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__access_links__accessLinkId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetEventAccessLinkAsync(eventId, accessLinkId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("234kjhg23kl4jhlaksjdh3");
			result.Name.ShouldBe("Event Access Link 1");
			result.Url.ShouldBeNull();
		}

		[Fact]
		public async Task GetAllEventAccessLinksAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__access_links_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllEventAccessLinksAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("234kjhg23kl4jhlaksjdh3");
			result[0].IsDefault.ShouldBeTrue();
		}

		[Fact]
		public async Task UpdateEventAccessLinkAsync()
		{
			// Arrange
			var eventId = "event123";
			var accessLinkId = "link123";
			var name = "Updated Link Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateEventAccessLinkAsync(eventId, accessLinkId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateEventAccessLinkAsync_WithMultipleParameters()
		{
			// Arrange
			var eventId = "event123";
			var accessLinkId = "link123";
			var name = "Updated Link";
			var isDefault = true;
			var authMethod = EventAuthenticationMethod.ZoomAccount;
			var allowDomains = new[] { "newdomain.com" };
			var emailRestrict = new[] { "admin@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateEventAccessLinkAsync(
				eventId,
				accessLinkId,
				name,
				isDefault,
				authMethod,
				allowDomains,
				emailRestrict,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateEventAccessLinkAsync_WithSecurityOptions()
		{
			// Arrange
			var eventId = "event123";
			var accessLinkId = "link123";
			var emailAuth = false;
			var securityCode = true;
			var ticketTypeId = "ticket789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateEventAccessLinkAsync(
				eventId,
				accessLinkId,
				emailAuthentication: emailAuth,
				securityCodeVerification: securityCode,
				ticketTypeId: ticketTypeId,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Event Tests

		[Fact]
		public async Task GetAllAsync()
		{
			// Arrange
			var recordsPerPage = 30;
			var pagingToken = "token123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events"))
				.WithQueryString("role_type", "host")
				.WithQueryString("event_status_type", "upcoming")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.zoom_events_events_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllAsync(UserRoleType.Host, EventListStatus.Upcoming, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateSimpleEventAsync()
		{
			// Arrange
			var name = "New Simple Event";
			var description = "Event Description";
			var start = new DateTime(2023, 6, 1, 10, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 1, 11, 0, 0, DateTimeKind.Utc);
			var timeZone = TimeZones.America_New_York;
			var meetingType = EventMeetingType.Meeting;
			var hubId = "hub123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events"))
				.Respond("application/json", EndpointsResource.zoom_events_events_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateSimpleEventAsync(name, description, start, end, timeZone, meetingType, hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateConferenceAsync()
		{
			// Arrange
			var name = "Conference Event";
			var description = "Conference Description";
			var calendar = new[]
			{
				(Start: new DateTime(2023, 6, 1, 10, 0, 0, DateTimeKind.Utc), End: new DateTime(2023, 6, 1, 11, 0, 0, DateTimeKind.Utc)),
				(Start: new DateTime(2023, 6, 2, 10, 0, 0, DateTimeKind.Utc), End: new DateTime(2023, 6, 2, 11, 0, 0, DateTimeKind.Utc))
			};
			var timeZone = TimeZones.America_New_York;
			var hubId = "hub123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events"))
				.Respond("application/json", EndpointsResource.zoom_events_events_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateConferenceAsync(name, description, calendar, timeZone, hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateRecurringEventAsync()
		{
			// Arrange
			var name = "Recurring Event";
			var description = "Recurring Event Description";
			var start = new DateTime(2023, 6, 1, 10, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 1, 11, 0, 0, DateTimeKind.Utc);
			var timeZone = TimeZones.America_New_York;
			var hubId = "hub123";
			var recurrence = new EventRecurrenceInfo
			{
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday },
				EndDateTime = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events"))
				.Respond("application/json", EndpointsResource.zoom_events_events_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateRecurringEventAsync(name, description, start, end, recurrence, timeZone, hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("234kj2h34kljgh23lkhj3");
		}

		[Fact]
		public async Task UpdateSimpleEventAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Updated Event Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateSimpleEventAsync(eventId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateConferenceAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Updated Conference Name";
			var description = "Updated conference description";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateConferenceAsync(eventId, name, description, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateConferenceAsync_WithCalendar()
		{
			// Arrange
			var eventId = "event123";
			var name = "Updated Conference";
			var calendar = new[]
			{
				(Start: (DateTime?)new DateTime(2023, 7, 1, 10, 0, 0, DateTimeKind.Utc), End: (DateTime?)new DateTime(2023, 7, 1, 12, 0, 0, DateTimeKind.Utc)),
				(Start: (DateTime?)new DateTime(2023, 7, 2, 10, 0, 0, DateTimeKind.Utc), End: (DateTime?)new DateTime(2023, 7, 2, 12, 0, 0, DateTimeKind.Utc))
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateConferenceAsync(eventId, name, calendar: calendar, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRecurringEventAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Updated Recurring Event";
			var description = "Updated description";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRecurringEventAsync(eventId, name, description, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRecurringEventAsync_WithFullRecurrence()
		{
			// Arrange
			var eventId = "event123";
			var name = "Updated Event with Recurrence";
			var recurrence = new EventRecurrenceInfo
			{
				Type = RecurrenceType.Daily,
				RepeatInterval = 2,
				EndDateTime = new DateTime(2024, 6, 30, 23, 59, 59, DateTimeKind.Utc)
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRecurringEventAsync(eventId, name, recurrence: recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task PublishEventAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "event_actions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.PublishEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelEventAsync()
		{
			// Arrange
			var eventId = "event123";
			var cancellationMessage = "Event cancelled due to weather";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "event_actions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.CancelEventAsync(eventId, cancellationMessage, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteEventAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DuplicateEventAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Duplicated Event";
			var start = new DateTime(2023, 7, 1, 10, 0, 0, DateTimeKind.Utc);
			var timeZone = TimeZones.America_New_York;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "event_actions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__event_actions_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.DuplicateEventAsync(eventId, name, start, timeZone, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("kNqLPC6hSFiZ9NpgjA549w");
		}

		#endregion

		#region Exhibitor Tests

		[Fact]
		public async Task CreateExhibitorAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Acme Corporation";
			var contactName = "John Doe";
			var contactEmail = "john@acme.com";
			var isSponsor = true;
			var sponsorTierId = "tier123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__exhibitors_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateExhibitorAsync(
				eventId,
				name,
				contactName,
				contactEmail,
				isSponsor,
				sponsorTierId,
				"Leading technology provider",
				["session1", "session2"],
				"https://www.acme.com",
				"https://www.acme.com/privacy",
				"https://linkedin.com/company/acme",
				"https://twitter.com/acme",
				"https://youtube.com/acme",
				"https://instagram.com/acme",
				"https://facebook.com/acme",
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("kNqLPC6hSFiZ9NpgjA549w");
			result.Name.ShouldBe("John Doe");
			result.ContactName.ShouldBe("John Doe");
			result.ContactEmailAddress.ShouldBe("abc.def@email.com");
			result.IsSponsor.ShouldBeTrue();
		}

		[Fact]
		public async Task CreateExhibitorAsync_NonSponsor()
		{
			// Arrange
			var eventId = "event123";
			var name = "Regular Exhibitor";
			var contactName = "Jane Smith";
			var contactEmail = "jane@regular.com";
			var isSponsor = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__exhibitors_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateExhibitorAsync(
				eventId,
				name,
				contactName,
				contactEmail,
				isSponsor,
				null,
				"Regular exhibitor booth",
				null,
				"https://www.mycompanywebsite.com",
				"https://www.mycompanywebsite.com/privacy",
				null,
				null,
				null,
				null,
				null,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("kNqLPC6hSFiZ9NpgjA549w");
			result.IsSponsor.ShouldBeTrue();
		}

		[Fact]
		public async Task GetExhibitorAsync()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__exhibitors__exhibitorId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetExhibitorAsync(eventId, exhibitorId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("kNqLPC6hSFiZ9NpgjA549w");
			result.Name.ShouldBe("Fletchie Doe");
		}

		[Fact]
		public async Task GetAllExhibitorsAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__exhibitors_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllExhibitorsAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("kNqLPC6hSFiZ9NpgjA549w");
			result[0].IsSponsor.ShouldBeTrue();
		}

		[Fact]
		public async Task UpdateExhibitorAsync()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";
			var name = "Updated Exhibitor Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateExhibitorAsync(eventId, exhibitorId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateExhibitorAsync_WithAllParameters()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";
			var name = "Updated Name";
			var contactName = "Updated Contact";
			var contactEmail = "updated@example.com";
			var isSponsor = true;
			var sponsorTierId = "newTier123";
			var description = "Updated description";
			var sessionIds = new[] { "session1", "session2", "session3" };
			var website = "https://updated.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateExhibitorAsync(
				eventId,
				exhibitorId,
				name,
				contactName,
				contactEmail,
				isSponsor,
				sponsorTierId,
				description,
				sessionIds,
				website,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateExhibitorAsync_WithSocialMediaLinks()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";
			var linkedInUrl = "https://linkedin.com/updated";
			var twitterUrl = "https://twitter.com/updated";
			var youtubeUrl = "https://youtube.com/updated";
			var instagramUrl = "https://instagram.com/updated";
			var facebookUrl = "https://facebook.com/updated";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateExhibitorAsync(
				eventId,
				exhibitorId,
				linkedInUrl: linkedInUrl,
				twitterUrl: twitterUrl,
				youtubeUrl: youtubeUrl,
				instagramUrl: instagramUrl,
				facebookUrl: facebookUrl,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteExhibitorAsync()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteExhibitorAsync(eventId, exhibitorId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllSponsorTiersAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sponsor_tiers"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sponsor_tiers_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSponsorTiersAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		#endregion

		#region Hub Tests

		[Fact]
		public async Task CreateHubHostAsync()
		{
			// Arrange
			var hubId = "hub123";
			var emailAddress = "host@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "hosts"))
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__hosts_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateHubHostAsync(hubId, emailAddress, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("di78aUD6RwmuRQY3WlK6VA");
		}

		[Fact]
		public async Task GetAllHubsAsync()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs"))
				.WithQueryString("role_type", "host")
				.Respond("application/json", EndpointsResource.zoom_events_hubs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllHubsAsync(UserRoleType.Host, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetAllHubHostsAsync()
		{
			// Arrange
			var hubId = "hub123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "hosts"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__hosts_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllHubHostsAsync(hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllHubVideosAsync()
		{
			// Arrange
			var hubId = "hub123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "videos"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__videos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllHubVideosAsync(hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetAllHubVideosAsync_WithFolder()
		{
			// Arrange
			var hubId = "hub123";
			var folderId = "folder456";
			var recordsPerPage = 50;
			var pagingToken = "token_abc";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "videos"))
				.WithQueryString("folder_id", folderId)
				.WithQueryString("page_size", "50")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__videos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllHubVideosAsync(hubId, folderId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.TotalRecords.ShouldBe(1);
			result.Records[0].Id.ShouldBe("iso9Dllddp39dagjLj9j");
			result.Records[0].Name.ShouldBe("My Recording1");
			result.Records[0].SourceType.ShouldBe(HubVideoSourceType.Recording);
			result.Records[0].Status.ShouldBe(HubVideoStatus.Processing);
		}

		[Fact]
		public async Task RemoveHostFromHubAsync()
		{
			// Arrange
			var hubId = "hub123";
			var userId = "user456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "hosts", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.RemoveHostFromHubAsync(hubId, userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Registrant Tests

		[Fact]
		public async Task GetAllRegistrantsAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "registrants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllRegistrantsAsync(eventId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllSessionAttendeesAsync()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "attendees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__attendees_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSessionAttendeesAsync(eventId, sessionId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Session Tests

		[Fact]
		public async Task CreateSessionAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "Session 1";
			var start = new DateTime(2023, 6, 1, 10, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 1, 11, 0, 0, DateTimeKind.Utc);
			var timeZone = TimeZones.America_New_York;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateSessionAsync(eventId, name, start, end, timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("4uzfv3JwTeyR5QpC3PXwMg");
		}

		[Fact]
		public async Task GetAllSessionsAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSessionsAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetSessionAsync()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetSessionAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateSessionAsync()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";
			var name = "Updated Session";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateSessionAsync(eventId, sessionId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSessionAsync()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteSessionAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetSessionJoinTokenAsync()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "join_token"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__join_token_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetSessionJoinTokenAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("OIgzlUv99nloE1vFMFssJVZuhFSstmSNd_Pi4WEBUNy0hWK2L6TtrRWscn");
		}

		#endregion

		#region Speaker Tests

		[Fact]
		public async Task CreateSpeakerAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "John Doe";
			var emailAddress = "john@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__speakers_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateSpeakerAsync(eventId, name, emailAddress, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllSpeakersAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__speakers_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSpeakersAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetSpeakerAsync()
		{
			// Arrange
			var eventId = "event123";
			var speakerId = "speaker456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers", speakerId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__speakers__speakerId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetSpeakerAsync(eventId, speakerId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateSpeakerAsync()
		{
			// Arrange
			var eventId = "event123";
			var speakerId = "speaker456";
			var name = "Jane Doe";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers", speakerId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateSpeakerAsync(eventId, speakerId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSpeakerAsync()
		{
			// Arrange
			var eventId = "event123";
			var speakerId = "speaker456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers", speakerId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteSpeakerAsync(eventId, speakerId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Ticket Type Tests

		[Fact]
		public async Task CreateTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var name = "General Admission";
			var start = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 30, 23, 59, 59, DateTimeKind.Utc);
			var currencyCode = "USD";
			var price = 50.00;
			var quantity = 100;
			var description = "General admission ticket";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketTypeAsync(
				eventId,
				name,
				start,
				end,
				currencyCode,
				price,
				quantity,
				description,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("pfalaof9s83lsLJD9u2d");
		}

		[Fact]
		public async Task CreateTicketTypeAsync_FreeTicket()
		{
			// Arrange
			var eventId = "event123";
			var name = "Free Admission";
			var start = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 30, 23, 59, 59, DateTimeKind.Utc);
			var currencyCode = "USD";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketTypeAsync(
				eventId,
				name,
				start,
				end,
				currencyCode,
				price: null,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("pfalaof9s83lsLJD9u2d");
		}

		[Fact]
		public async Task CreateTicketTypeAsync_WithSessionIds()
		{
			// Arrange
			var eventId = "event123";
			var name = "VIP Pass";
			var start = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 30, 23, 59, 59, DateTimeKind.Utc);
			var currencyCode = "USD";
			var price = 150.00;
			var quantity = 50;
			var description = "VIP access ticket";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketTypeAsync(
				eventId,
				name,
				start,
				end,
				currencyCode,
				price,
				quantity,
				description,
				sessionIds: new[] { "session1", "session2", "session3" },
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("pfalaof9s83lsLJD9u2d");
		}

		[Fact]
		public async Task CreateTicketTypeAsync_WithAllParameters()
		{
			// Arrange
			var eventId = "event123";
			var name = "Early Bird Special";
			var start = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 6, 15, 23, 59, 59, DateTimeKind.Utc);
			var currencyCode = "USD";
			var price = 35.00;
			var quantity = 50;
			var description = "Early bird discount ticket";
			var quantitySold = 10;
			var sessionIds = new[] { "session1" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketTypeAsync(
				eventId,
				name,
				start,
				end,
				currencyCode,
				price,
				quantity,
				description,
				quantitySold,
				sessionIds,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("pfalaof9s83lsLJD9u2d");
		}

		[Fact]
		public async Task DeleteTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteTicketTypeAsync(eventId, ticketTypeId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllTicketTypesAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketTypesAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("234kjhg23kl4jhlaksjdh3");
			result[0].Name.ShouldBe("General Admission Ticket");
		}

		[Fact]
		public async Task UpdateTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var name = "Updated Ticket Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketTypeAsync(eventId, ticketTypeId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketTypeAsync_WithPrice()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var name = "Updated Pricing";
			var price = 75.00;
			var quantity = 150;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketTypeAsync(
				eventId,
				ticketTypeId,
				name,
				price: price,
				quantity: quantity,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketTypeAsync_WithDatesAndSessions()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var start = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 7, 31, 23, 59, 59, DateTimeKind.Utc);
			var sessionIds = new[] { "session1", "session2" };
			var description = "Updated description";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketTypeAsync(
				eventId,
				ticketTypeId,
				start: start,
				end: end,
				description: description,
				sessionIds: sessionIds,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketTypeAsync_WithAllParameters()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var name = "Complete Update";
			var start = new DateTime(2023, 8, 1, 0, 0, 0, DateTimeKind.Utc);
			var end = new DateTime(2023, 8, 31, 23, 59, 59, DateTimeKind.Utc);
			var currencyCode = "EUR";
			var price = 100.00;
			var quantity = 200;
			var description = "Fully updated ticket";
			var quantitySold = 50;
			var sessionIds = new[] { "session1", "session2", "session3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketTypeAsync(
				eventId,
				ticketTypeId,
				name,
				start,
				end,
				currencyCode,
				price,
				quantity,
				description,
				quantitySold,
				sessionIds,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetRegistrationQuestionsForEventAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__questions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(1);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetRegistrationQuestionsForEventAsync_EmptyQuestions()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__questions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(1);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetRegistrationQuestionsForTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId, "questions"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__ticket_types__ticketTypeId__questions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForTicketTypeAsync(eventId, ticketTypeId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(1);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsForEventAsync()
		{
			// Arrange
			var eventId = "event123";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.LastName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.City, IsRequired = true }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRegistrationQuestionsForEventAsync(
				eventId,
				standardQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsForEventAsync_WithCustomQuestions()
		{
			// Arrange
			var eventId = "event123";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.JobTitle, IsRequired = true }
			};
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForEvent
				{
					Title = "Dietary Preferences",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText,
					IsRequired = false
				},
				new RegistrationCustomQuestionForEvent
				{
					Title = "Industry",
					Type = RegistrationCustomQuestionTypeForEvent.SingleRadio,
					IsRequired = true,
					Options = new[] { "Technology", "Healthcare", "Finance", "Other" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRegistrationQuestionsForEventAsync(
				eventId,
				standardQuestions,
				customQuestions,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsForEventAsync_OnlyCustomQuestions()
		{
			// Arrange
			var eventId = "event123";
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForEvent
				{
					Title = "How did you hear about us?",
					Type = RegistrationCustomQuestionTypeForEvent.MultipleChoices,
					IsRequired = false,
					Options = new[] { "Social Media", "Email", "Friend", "Advertisement" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRegistrationQuestionsForEventAsync(
				eventId,
				customQuestions: customQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsForTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.LastName, IsRequired = true }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId, "questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRegistrationQuestionsForTicketTypeAsync(
				eventId,
				ticketTypeId,
				standardQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsForTicketTypeAsync_WithBothQuestionTypes()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.Phone, IsRequired = true }
			};
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForEvent
				{
					Title = "VIP Preferences",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText,
					IsRequired = false
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId, "questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateRegistrationQuestionsForTicketTypeAsync(
				eventId,
				ticketTypeId,
				standardQuestions,
				customQuestions,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Ticket Tests

		[Fact]
		public async Task CreateTicketsAsync()
		{
			// Arrange
			var eventId = "event123";
			var tickets = new[]
			{
				new EventTicket
				{
					Email = "attendee1@example.com",
					TypeId = "ticket_type_1",
					FirstName = "John",
					LastName = "Doe",
					City = "New York"
				},
				new EventTicket
				{
					Email = "attendee2@example.com",
					TypeId = "ticket_type_1",
					FirstName = "Jane",
					LastName = "Smith",
					City = "Los Angeles"
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.ShouldNotBeNull();
			result.Tickets.Length.ShouldBe(1);
			result.Errors.ShouldNotBeNull();
			result.Errors.Length.ShouldBe(1);
		}

		[Fact]
		public async Task CreateTicketsAsync_WithSource()
		{
			// Arrange
			var eventId = "event123";
			var source = "integration_test";
			var tickets = new[]
			{
				new EventTicket
				{
					Email = "test@example.com",
					TypeId = "ticket_type_1",
					FirstName = "Test",
					LastName = "User"
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, source, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.Length.ShouldBe(1);
			result.Errors.Length.ShouldBe(1);
		}

		[Fact]
		public async Task CreateTicketsAsync_WithErrors()
		{
			// Arrange
			var eventId = "event123";
			var tickets = new[]
			{
				new EventTicket { Email = "valid@example.com", TypeId = "ticket_type_1" },
				new EventTicket { Email = "invalid@example.com", TypeId = "invalid_type" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.Length.ShouldBe(1);
			result.Errors.Length.ShouldBe(1);
			result.Errors[0].Email.ShouldBe("email@zoom.us");
			result.Errors[0].ErrorCode.ShouldBe("fast_join_with_registrationNeeded");
		}

		[Fact]
		public async Task CreateTicketsAsync_WithAllTicketProperties()
		{
			// Arrange
			var eventId = "event123";
			var tickets = new[]
			{
				new EventTicket
				{
					Email = "complete@example.com",
					TypeId = "ticket_type_1",
					ExternalTicketId = "ext_123",
					SendNotifications = true,
					FastJoin = false,
					RegistrationNeeded = true,
					SessionIds = new[] { "session1", "session2" },
					FirstName = "Complete",
					LastName = "User",
					Address = "123 Main St",
					City = "Boston",
					State = "MA",
					Country = "US",
					Zip = "02101",
					Phone = "+1-555-0100",
					Industry = "Finance",
					JobTitle = "CFO",
					Organization = "Finance Corp",
					Comments = "VIP guest",
					CustomQuestions = new[]
					{
						new KeyValuePair<string, string>("Dietary Preference", "Vegetarian"),
						new KeyValuePair<string, string>("T-Shirt Size", "L")
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.Length.ShouldBe(1);
			result.Tickets[0].Id.ShouldBe("iso9Dllddp39dagjLj9j");
		}

		[Fact]
		public async Task DeleteTicketAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteTicketAsync(eventId, ticketId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetTicketAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets__ticketId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetTicketAsync(eventId, ticketId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("iso9Dllddp39dagjLj9j");
			result.Email.ShouldBe("email@zoom.us");
			result.FirstName.ShouldBe("Jill");
			result.LastName.ShouldBe("Chill");
		}

		[Fact]
		public async Task GetAllTicketsAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketsAsync(eventId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("iso9Dllddp39dagjLj9j");
		}

		[Fact]
		public async Task GetAllTicketsAsync_WithTicketTypeFilter()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "type_vip";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("ticket_type_id", ticketTypeId)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__tickets_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketsAsync(eventId, ticketTypeId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task UpdateTicketAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";
			var firstName = "UpdatedFirstName";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketAsync(eventId, ticketId, firstName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketAsync_WithMultipleFields()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";
			var firstName = "Jane";
			var lastName = "Updated";
			var city = "San Francisco";
			var phone = "+1-415-555-0100";
			var jobTitle = "Senior Developer";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketAsync(
				eventId,
				ticketId,
				firstName,
				lastName,
				city: city,
				phone: phone,
				jobTitle: jobTitle,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketAsync_WithAllFields()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";
			var firstName = "Complete";
			var lastName = "Update";
			var address = "456 Updated St";
			var city = "Austin";
			var state = "TX";
			var zip = "78701";
			var country = "US";
			var phone = "+1-512-555-0100";
			var industry = "Finance";
			var jobTitle = "CFO";
			var organization = "Finance Corp";
			var comments = "VIP guest";
			var externalTicketId = "ext_updated_123";
			var customQuestions = new[]
			{
				new KeyValuePair<string, string>("Dietary Preference", "Vegetarian"),
				new KeyValuePair<string, string>("T-Shirt Size", "L")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketAsync(
				eventId,
				ticketId,
				firstName,
				lastName,
				address,
				city,
				state,
				zip,
				country,
				phone,
				industry,
				jobTitle,
				organization,
				comments,
				externalTicketId,
				customQuestions,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketAsync_WithCustomQuestionsOnly()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";
			var customQuestions = new[]
			{
				new KeyValuePair<string, string>("Dietary Restriction", "Gluten-free"),
				new KeyValuePair<string, string>("Accessibility Needs", "Wheelchair access"),
				new KeyValuePair<string, string>("Preferred Session Track", "Technical")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketAsync(
				eventId,
				ticketId,
				customQuestions: customQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateTicketAsync_WithExternalTicketId()
		{
			// Arrange
			var eventId = "event123";
			var ticketId = "ticket456";
			var externalTicketId = "external_system_ref_789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateTicketAsync(
				eventId,
				ticketId,
				externalTicketId: externalTicketId,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Video On Demand Tests

		[Fact]
		public async Task PublishVideoOnDemandChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "actions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.PublishVideoOnDemandChannelAsync(hubId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateVideoOnDemandChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var name = "Tech Talks Channel";
			var description = "Technical presentations and webinars";
			var type = VideoOnDemandChannelType.VideoListHub;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels"))
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateVideoOnDemandChannelAsync(hubId, name, description, type, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("iso9Dllddp39dagjLj9j");
			result.Name.ShouldBe("FoodieFlicks");
		}

		[Fact]
		public async Task CreateVideoOnDemandChannelAsync_MultiVideoEmbedded()
		{
			// Arrange
			var hubId = "hub123";
			var name = "Embedded Training Videos";
			var description = "Training videos for embedding";
			var type = VideoOnDemandChannelType.MultiVideoEmbedded;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels"))
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateVideoOnDemandChannelAsync(hubId, name, description, type, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("iso9Dllddp39dagjLj9j");
		}

		[Fact]
		public async Task UpdateVideoOnDemandChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var name = "Updated Channel Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateVideoOnDemandChannelAsync(hubId, channelId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateVideoOnDemandChannelAsync_WithDescription()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var name = "Updated Channel";
			var description = "Updated description with more details";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateVideoOnDemandChannelAsync(hubId, channelId, name, description, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetVideoOnDemandChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels__channelId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetVideoOnDemandChannelAsync(hubId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("iso9Dllddp39dagjLj9j");
			result.Name.ShouldBe("FoodieFlicks");
		}

		[Fact]
		public async Task DeleteVideoOnDemandChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteVideoOnDemandChannelAsync(hubId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllVidoOnDemandChannelsAsync()
		{
			// Arrange
			var hubId = "hub123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllVidoOnDemandChannelsAsync(hubId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("iso9Dllddp39dagjLj9j");
		}

		[Fact]
		public async Task AddVideosToChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var videoIds = new[] { "video1", "video2", "video3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.AddVideosToChannelAsync(hubId, channelId, videoIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddVideosToChannelAsync_SingleVideo()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var videoIds = new[] { "video_single_123" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.AddVideosToChannelAsync(hubId, channelId, videoIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveVideoFromChannelAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var videoId = "video789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "videos", videoId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.RemoveVideoFromChannelAsync(hubId, channelId, videoId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllVideosAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "videos"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels__channelId__videos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllVideosAsync(hubId, channelId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		#endregion

		#region Video On Demand Registration Tests

		[Fact]
		public async Task GetAllVideoOnDemandRegistrationQuestionsAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "registration_questions"))
				.Respond("application/json", EndpointsResource.zoom_events_hubs__hubId__vod_channels__channelId__registration_questions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllVideoOnDemandRegistrationQuestionsAsync(hubId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(1);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task UpdateChannelRegistrationQuestionsAsync()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.LastName, IsRequired = true }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "registration_questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateChannelRegistrationQuestionsAsync(
				hubId,
				channelId,
				standardQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateChannelRegistrationQuestionsAsync_WithCustomQuestions()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true }
			};
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Company Name",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText,
					IsRequired = true
				},
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Industry",
					Type = RegistrationCustomQuestionTypeForEvent.SingleDropdown,
					IsRequired = true,
					Answers = new[] { "Technology", "Healthcare", "Finance", "Education", "Other" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "registration_questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateChannelRegistrationQuestionsAsync(
				hubId,
				channelId,
				standardQuestions,
				customQuestions,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateChannelRegistrationQuestionsAsync_OnlyCustomQuestions()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "How did you find this channel?",
					Type = RegistrationCustomQuestionTypeForEvent.MultipleChoices,
					IsRequired = false,
					Answers = new[] { "Search", "Recommendation", "Social Media", "Email" }
				},
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Additional Comments",
					Type = RegistrationCustomQuestionTypeForEvent.LongText,
					IsRequired = false,
					MinimumLength = 10,
					MaximumLength = 2000
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "registration_questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateChannelRegistrationQuestionsAsync(
				hubId,
				channelId,
				customQuestions: customQuestions,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateChannelRegistrationQuestionsAsync_WithAllQuestionTypes()
		{
			// Arrange
			var hubId = "hub123";
			var channelId = "channel456";
			var standardQuestions = new[]
			{
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.FirstName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.LastName, IsRequired = true },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.Organization, IsRequired = false },
				new RegistrationStandardQuestion { FieldName = EventRegistrationField.JobTitle, IsRequired = false }
			};
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Experience Level",
					Type = RegistrationCustomQuestionTypeForEvent.SingleRadio,
					IsRequired = true,
					Answers = new[] { "Beginner", "Intermediate", "Advanced", "Expert" }
				},
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Areas of Interest",
					Type = RegistrationCustomQuestionTypeForEvent.MultipleChoices,
					IsRequired = false,
					Answers = new[] { "Product Updates", "Technical Deep Dives", "Best Practices", "Case Studies" }
				},
				new RegistrationCustomQuestionForVodChannel
				{
					Title = "Brief Introduction",
					Type = RegistrationCustomQuestionTypeForEvent.ShortText,
					IsRequired = false,
					MinimumLength = 20,
					MaximumLength = 500
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "vod_channels", channelId, "registration_questions"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateChannelRegistrationQuestionsAsync(
				hubId,
				channelId,
				standardQuestions,
				customQuestions,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Session Reservation Tests

		[Fact]
		public async Task AddSessionReservationAsync_WithEmailAddress_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var emailAddress = "attendee@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "reservations"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.AddSessionReservationAsync(eventId, sessionId, emailAddress, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSessionReservationAsync_WithEmailAddress_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var emailAddress = "attendee@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "reservations"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.DeleteSessionReservationAsync(eventId, sessionId, emailAddress, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllSessionReservationsAsync_WithDefaultParameters_ReturnsReservations()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "reservations"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__reservations_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSessionReservationsAsync(eventId, sessionId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		#endregion

		#region Session Interpreter Tests

		[Fact]
		public async Task UpsertSessionInterpretersAsync_WithLanguageInterpreters_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var languageInterpreters = new[]
			{
				("interpreter1@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.Spanish),
				("interpreter2@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.French)
			};
			var signLanguageInterpreters = Array.Empty<(string, InterpretationSignLanguage)>();

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "interpreters"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionInterpretersAsync(eventId, sessionId, languageInterpreters, signLanguageInterpreters, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpsertSessionInterpretersAsync_WithSignLanguageInterpreters_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var languageInterpreters = Array.Empty<(string, InterpretationLanguageForEventSession, InterpretationLanguageForEventSession)>();
			var signLanguageInterpreters = new[]
			{
				("signinterpreter1@example.com", InterpretationSignLanguage.American),
				("signinterpreter2@example.com", InterpretationSignLanguage.British)
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "interpreters"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionInterpretersAsync(eventId, sessionId, languageInterpreters, signLanguageInterpreters, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpsertSessionInterpretersAsync_WithBothTypes_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var languageInterpreters = new[]
			{
				("interpreter1@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.German),
				("interpreter2@example.com", InterpretationLanguageForEventSession.English, InterpretationLanguageForEventSession.Portuguese)
			};
			var signLanguageInterpreters = new[]
			{
				("signinterpreter1@example.com", InterpretationSignLanguage.French),
				("signinterpreter2@example.com", InterpretationSignLanguage.Japanese)
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "interpreters"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionInterpretersAsync(eventId, sessionId, languageInterpreters, signLanguageInterpreters, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllSessionInterpretersAsync_ReturnsInterpreters()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "interpreters"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__interpreters_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSessionInterpretersAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		#endregion

		#region Session Poll Tests

		[Fact]
		public async Task UpsertSessionPollsAsync_WithBasicPoll_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var polls = new[]
			{
				new PollForEventSession
				{
					Title = "Basic Poll",
					Type = PollType.Basic,
					Status = PollStatusForEventSession.Active,
					AllowAnonymous = false,
					Questions = new[]
					{
						new PollQuestionForEventSession
						{
							Question = "What is your favorite color?",
							Type = PollQuestionType.SingleChoice,
							Answers = new[] { "Red", "Blue", "Green", "Yellow" }
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "polls"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionPollsAsync(eventId, sessionId, polls, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpsertSessionPollsAsync_WithAdvancedPoll_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var polls = new[]
			{
				new PollForEventSession
				{
					Title = "Advanced Poll",
					Type = PollType.Advanced,
					Status = PollStatusForEventSession.Active,
					AllowAnonymous = true,
					Questions = new[]
					{
						new PollQuestionForEventSession
						{
							Question = "What is your pet's name?",
							Type = PollQuestionType.Short,
							MinimumNumberOfCharacters = 1,
							MaximumNumberOfCharacters = 500
						},
						new PollQuestionForEventSession
						{
							Question = "Tell us about yourself",
							Type = PollQuestionType.Long,
							MinimumNumberOfCharacters = 1,
							MaximumNumberOfCharacters = 2000
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "polls"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionPollsAsync(eventId, sessionId, polls, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpsertSessionPollsAsync_WithMultiplePolls_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var polls = new[]
			{
				new PollForEventSession
				{
					Title = "Poll 1",
					Type = PollType.Basic,
					Status = PollStatusForEventSession.Active,
					AllowAnonymous = false,
					Questions = new[]
					{
						new PollQuestionForEventSession
						{
							Question = "Question 1",
							Type = PollQuestionType.SingleChoice,
							Answers = new[] { "Answer 1", "Answer 2" }
						}
					}
				},
				new PollForEventSession
				{
					Title = "Poll 2",
					Type = PollType.Advanced,
					Status = PollStatusForEventSession.Active,
					AllowAnonymous = true,
					Questions = new[]
					{
						new PollQuestionForEventSession
						{
							Question = "Question 2",
							Type = PollQuestionType.MultipleChoice,
							Answers = new[] { "Option A", "Option B", "Option C" }
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "polls"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpsertSessionPollsAsync(eventId, sessionId, polls, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllSessionPollsAsync_ReturnsPolls()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "polls"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__polls_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSessionPollsAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		#endregion

		#region Session Livestream Tests

		[Fact]
		public async Task UpdateSessionLivestreamConfigurationAsync_EnableIncoming_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var incomingEnabled = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "livestream"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateSessionLivestreamConfigurationAsync(eventId, sessionId, incomingEnabled, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateSessionLivestreamConfigurationAsync_DisableIncoming_Succeeds()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";
			var incomingEnabled = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "livestream"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			await events.UpdateSessionLivestreamConfigurationAsync(eventId, sessionId, incomingEnabled, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetSessionLivestreamConfgurationAsync_ReturnsConfiguration()
		{
			// Arrange
			var eventId = "event123";
			var sessionId = "session456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "livestream"))
				.Respond("application/json", EndpointsResource.zoom_events_events__eventId__sessions__sessionId__livestream_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetSessionLivestreamConfgurationAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.StreamUrl.ShouldBe("rtmp://10.100.125.159:1956/live");
			result.StreamKey.ShouldBe("ZAEUMvRcI1eq_SRMg3iQBz-U");
		}

		#endregion
	}
}
