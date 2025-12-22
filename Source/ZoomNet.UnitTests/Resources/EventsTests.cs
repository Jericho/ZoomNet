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

namespace ZoomNet.UnitTests.Resources
{
	public class EventsTests
	{
		private const string EVENTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""events"": [
				{
					""id"": ""event123"",
					""name"": ""Test Event"",
					""description"": ""Test Description"",
					""start_time"": ""2023-06-01T10:00:00Z"",
					""end_time"": ""2023-06-01T11:00:00Z"",
					""timezone"": ""America/New_York"",
					""event_type"": ""simple""
				}
			]
		}";

		private const string SINGLE_EVENT_JSON = @"{
			""id"": ""event123"",
			""name"": ""Test Event"",
			""description"": ""Test Description"",
			""start_time"": ""2023-06-01T10:00:00Z"",
			""end_time"": ""2023-06-01T11:00:00Z"",
			""timezone"": ""America/New_York"",
			""event_type"": ""simple"",
			""hub_id"": ""hub123""
		}";

		private const string HUBS_JSON = @"{
			""hubs"": [
				{
					""id"": ""hub123"",
					""name"": ""Test Hub"",
					""description"": ""Hub Description""
				}
			]
		}";

		private const string SPEAKERS_JSON = @"{
			""speakers"": [
				{
					""id"": ""speaker123"",
					""name"": ""John Doe"",
					""email"": ""john@example.com"",
					""job_title"": ""CEO""
				}
			]
		}";

		private const string SESSIONS_JSON = @"{
			""sessions"": [
				{
					""id"": ""session123"",
					""name"": ""Session 1"",
					""start_time"": ""2023-06-01T10:00:00Z"",
					""end_time"": ""2023-06-01T11:00:00Z""
				}
			]
		}";

		private const string REGISTRANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""reg_token"",
			""registrants"": [
				{
					""id"": ""reg123"",
					""email"": ""attendee@example.com"",
					""first_name"": ""Jane"",
					""last_name"": ""Smith""
				}
			]
		}";

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
			var actionsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""attendees"": [
					{
						""email"": ""attendee@example.com"",
						""action"": ""check-in""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "attendee_action"))
				.WithQueryString("email", attendeeEmail)
				.WithQueryString("page_size", "30")
				.Respond("application/json", actionsJson);

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
			var errorsJson = @"{""errors"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("zoom_events", "events", eventId, "attendee_action"))
				.Respond("application/json", errorsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CheckInAttendeesAsync(eventId, attendeeEmails, source, TestContext.Current.CancellationToken);

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
			var coEditorsJson = @"{
				""coeditors"": [
					{
						""email"": ""coeditor1@example.com"",
						""permission_groups"": [ ""Publish"", ""EventConfiguration"" ]
					},
					{
						""email"": ""coeditor2@example.com"",
						""permission_groups"": [ ""EventBranding"", ""Registration & Join"", ""Venue"", ""EventExperience"", ""EventPlanning"" ]
					}
				],
				""total_records"": 2
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "coeditors"))
				.Respond("application/json", coEditorsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllCoEditorsAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
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
			var accessLinkJson = @"{
				""access_link_id"": ""link123"",
				""name"": ""Registration Link"",
				""type"": ""registration"",
				""is_default"": false,
				""url"": ""https://zoom.us/events/example"",
				""authentication_method"": ""zoom_account"",
				""allow_domain_list"": [ ""example.com"" ],
				""email_restrict_list"": [ ""user@example.com"" ],
				""security_at_join"": {
					""email_authentication"": true,
					""security_code_verification"": true
				},
				""ticket_type_id"": ""ticket123"",
				""recurring_registration_option"": ""all_sessions""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", accessLinkJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateEventAccessLinkAsync(eventId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("link123");
			result.Name.ShouldBe("Registration Link");
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

			var accessLinkJson = @"{
				""access_link_id"": ""234kjhg23kl4jhlaksjdh3"",
				""name"": ""Event Access Link 1"",
				""type"": ""registration"",
				""is_default"": true,
				""authentication_method"": ""bypass_auth"",
				""idp_name"": ""okta"",
				""allow_domain_list"": [ ""zoom.us"" ],
				""email_restrict_list"": [ ""example1@example.com"" ],
				""security_at_join"": {
					""email_authentication"": true,
					""security_code_verification"": true
				},
				""ticket_type_id"": ""PTYwAknYQXaDStOP7O3ExA"",
				""recurring_registration_option"": ""all_sessions""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", accessLinkJson);

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
			var accessLinkJson = @"{
				""access_link_id"": ""link123"",
				""name"": ""My Access Link"",
				""type"": ""registration"",
				""is_default"": false,
				""url"": ""https://zoom.us/events/mylink"",
				""authentication_method"": ""zoom_account""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links", accessLinkId))
				.Respond("application/json", accessLinkJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetEventAccessLinkAsync(eventId, accessLinkId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("link123");
			result.Name.ShouldBe("My Access Link");
			result.Url.ShouldBe("https://zoom.us/events/mylink");
		}

		[Fact]
		public async Task GetAllEventAccessLinksAsync()
		{
			// Arrange
			var eventId = "event123";
			var accessLinksJson = @"{
				""access_links"": [
					{
						""access_link_id"": ""link1"",
						""name"": ""Default Link"",
						""type"": ""registration"",
						""is_default"": true,
						""url"": ""https://zoom.us/events/link1""
					},
					{
						""access_link_id"": ""link2"",
						""name"": ""Group Join Link"",
						""type"": ""group-join"",
						""is_default"": false,
						""url"": ""https://zoom.us/events/link2""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "access_links"))
				.Respond("application/json", accessLinksJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllEventAccessLinksAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("link1");
			result[0].IsDefault.ShouldBeTrue();
			result[1].Id.ShouldBe("link2");
			result[1].Type.ShouldBe(EventAccessLinkType.GroupJoin);
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
				.Respond("application/json", EVENTS_JSON);

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
		public async Task GetAllAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => events.GetAllAsync(recordsPerPage: 500, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var eventId = "event123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId))
				.Respond("application/json", SINGLE_EVENT_JSON);

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

			var simpleEventJson = @"{
				""event_id"": ""234kj2h34kljgh23lkhj3"",
				""name"": ""Simple Event Name"",
				""description"": ""This is a simple event"",
				""timezone"": ""America/Indianapolis"",
				""event_type"": ""SIMPLE_EVENT"",
				""access_level"": ""PRIVATE_RESTRICTED"",
				""meeting_type"": ""MEETING"",
				""categories"": [ ""Food and Drinks"" ],
				""tags"": [ ""Event tag2"" ],
				""calendar"": [
					{
						""start_time"": ""2024-07-28T13:00:00Z"",
						""end_time"": ""2024-07-30T13:00:00Z""
					}
				],
				""status"": ""PUBLISHED"",
				""hub_id"": ""23asdfasdf3asdf"",
				""start_time"": ""2022-06-03T20:51:00Z"",
				""end_time"": ""2022-06-03T20:51:00Z"",
				""contact_name"": ""user contact name"",
				""lobby_start_time"": ""2022-06-03T20:51:00Z"",
				""lobby_end_time"": ""2022-06-03T20:51:00Z"",
				""event_url"": ""www.example.com/zoomEvents"",
				""blocked_countries"": [ ""US"" ],
				""attendance_type"": ""hybrid"",
				""tagline"": ""Unlocking Innovation: Join Us for the Day of Insipiration and Insight!""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events"))
				.Respond("application/json", simpleEventJson);

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

			var conferenceJson = @"{
				""event_id"": ""234kj2h34kljgh23lkhj3"",
				""name"": ""OpenAPI Conference Name"",
				""description"": ""This event was created with the OpenAPI"",
				""timezone"": ""America/Indianapolis"",
				""event_type"": ""CONFERENCE"",
				""recurrence"": {
					""type"": 1,
					""repeat_interval"": 1,
					""weekly_days"": [ 1 ],
					""monthly_days"": [ 1 ],
					""monthly_week_day"": 1,
					""end_times"": 1,
					""end_date_time"": ""2025-12-21T17:38:29.698Z"",
					""monthly_week"": -1,
					""duration"": 1
				},
				""access_level"": ""PRIVATE_RESTRICTED"",
				""meeting_type"": ""MEETING"",
				""categories"": [ ""Food and Drinks"" ],
				""tags"": [ ""Event tag1"" ],
				""calendar"": [
					{
						""start_time"": ""2024-07-28T13:00:00Z"",
						""end_time"": ""2024-07-30T13:00:00Z""
					}
				],
				""status"": ""PUBLISHED"",
				""hub_id"": ""23asdfasdf3asdf"",
				""start_time"": ""2022-06-03T20:51:00Z"",
				""end_time"": ""2022-06-03T20:51:00Z"",
				""contact_name"": ""user contact name"",
				""lobby_start_time"": ""2022-06-03T20:51:00Z"",
				""lobby_end_time"": ""2022-06-03T20:51:00Z"",
				""event_url"": ""www.example.com/zoomEvents"",
				""blocked_countries"": [ ""US"" ],
				""attendance_type"": ""hybrid"",
				""tagline"": ""Unlocking Innovation: Join Us for the Day of Insipiration and Insight!""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events"))
				.Respond("application/json", conferenceJson);

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
			var duplicateJson = @"{""event_id"": ""event_duplicate""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "event_actions"))
				.Respond("application/json", duplicateJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.DuplicateEventAsync(eventId, name, start, timeZone, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("event_duplicate");
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
			var exhibitorJson = @"{
				""exhibitor_id"": ""exhibitor123"",
				""name"": ""Acme Corporation"",
				""contact_name"": ""John Doe"",
				""contact_email"": ""john@acme.com"",
				""is_sponsor"": true,
				""tier_id"": ""tier123"",
				""description"": ""Leading technology provider"",
				""associated_sessions"": [ ""session1"", ""session2"" ],
				""website"": ""https://www.acme.com"",
				""privacy_policy"": ""https://www.acme.com/privacy"",
				""linkedin_url"": ""https://linkedin.com/company/acme"",
				""twitter_url"": ""https://twitter.com/acme"",
				""youtube_url"": ""https://youtube.com/acme"",
				""instagram_url"": ""https://instagram.com/acme"",
				""facebook_url"": ""https://facebook.com/acme""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", exhibitorJson);

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
				new[] { "session1", "session2" },
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
			result.Id.ShouldBe("exhibitor123");
			result.Name.ShouldBe("Acme Corporation");
			result.ContactName.ShouldBe("John Doe");
			result.ContactEmailAddress.ShouldBe("john@acme.com");
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
			var exhibitorJson = @"{
				""exhibitor_id"": ""exhibitor456"",
				""name"": ""Regular Exhibitor"",
				""contact_name"": ""Jane Smith"",
				""contact_email"": ""jane@regular.com"",
				""is_sponsor"": false,
				""description"": ""Regular exhibitor booth"",
				""website"": ""https://www.regular.com"",
				""privacy_policy"": ""https://www.regular.com/privacy""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", exhibitorJson);

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
			result.Id.ShouldBe("exhibitor456");
			result.IsSponsor.ShouldBeFalse();
		}

		[Fact]
		public async Task GetExhibitorAsync()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorId = "exhibitor123";
			var exhibitorJson = @"{
				""exhibitor_id"": ""exhibitor123"",
				""name"": ""Test Exhibitor"",
				""contact_name"": ""Contact Person"",
				""contact_email"": ""contact@test.com"",
				""is_sponsor"": true,
				""tier_id"": ""tier456"",
				""description"": ""Test description"",
				""website"": ""https://www.test.com""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors", exhibitorId))
				.Respond("application/json", exhibitorJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetExhibitorAsync(eventId, exhibitorId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("exhibitor123");
			result.Name.ShouldBe("Test Exhibitor");
		}

		[Fact]
		public async Task GetAllExhibitorsAsync()
		{
			// Arrange
			var eventId = "event123";
			var exhibitorsJson = @"{
				""exhibitors"": [
					{
						""exhibitor_id"": ""exhibitor1"",
						""name"": ""Exhibitor One"",
						""contact_name"": ""Contact One"",
						""contact_email"": ""one@example.com"",
						""is_sponsor"": true,
						""tier_id"": ""tier1""
					},
					{
						""exhibitor_id"": ""exhibitor2"",
						""name"": ""Exhibitor Two"",
						""contact_name"": ""Contact Two"",
						""contact_email"": ""two@example.com"",
						""is_sponsor"": false
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "exhibitors"))
				.Respond("application/json", exhibitorsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllExhibitorsAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("exhibitor1");
			result[0].IsSponsor.ShouldBeTrue();
			result[1].Id.ShouldBe("exhibitor2");
			result[1].IsSponsor.ShouldBeFalse();
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
			var sponsorTiersJson = @"{
				""sponsor_tiers"": [
					{
						""tier_id"": ""tier1"",
						""name"": ""Platinum"",
						""description"": ""Platinum sponsor tier"",
						""order"": 1
					},
					{
						""tier_id"": ""tier2"",
						""name"": ""Gold"",
						""description"": ""Gold sponsor tier"",
						""order"": 2
					},
					{
						""tier_id"": ""tier3"",
						""name"": ""Silver"",
						""description"": ""Silver sponsor tier"",
						""order"": 3
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sponsor_tiers"))
				.Respond("application/json", sponsorTiersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllSponsorTiersAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
		}

		#endregion

		#region Hub Tests

		[Fact]
		public async Task GetAllHubsAsync()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs"))
				.WithQueryString("role_type", "host")
				.Respond("application/json", HUBS_JSON);

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
		public async Task CreateHubHostAsync()
		{
			// Arrange
			var hubId = "hub123";
			var emailAddress = "host@example.com";
			var responseJson = @"{""host_user_id"": ""host456""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "hosts"))
				.Respond("application/json", responseJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateHubHostAsync(hubId, emailAddress, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("host456");
		}

		[Fact]
		public async Task GetAllHubHostsAsync()
		{
			// Arrange
			var hubId = "hub123";
			var hostsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""hosts"": [
					{
						""user_id"": ""host123"",
						""email"": ""host@example.com"",
						""first_name"": ""John"",
						""last_name"": ""Doe""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "hubs", hubId, "hosts"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", hostsJson);

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
				.Respond("application/json", REGISTRANTS_JSON);

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
			var attendeesJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""attendees"": [
					{
						""id"": ""att123"",
						""email"": ""attendee@example.com""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "attendees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", attendeesJson);

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
			var sessionJson = @"{
				""timezone"": ""America/New_York"",
				""type"": 2,
				""session_id"": ""4uzfv3JwTeyR5QpC3PXwMg"",
				""name"": ""Session 1"",
				""description"": ""Educational Session on ZoomEvents"",
				""start_time"": ""2022-05-31T13:00:00Z"",
				""end_time"": ""2022-05-31T13:00:00Z"",
				""session_speakers"": [
					{
						""speaker_id"": ""4uzfv3JwTeyR5QpC3PXwMg"",
						""access_to_edit_session"": true,
						""show_in_session_detail"": true,
						""has_alternative_host_permission"": true,
						""meeting_role"": 2,
						""name"": ""Speaker-1"",
						""company"": ""Zoom"",
						""title"": ""Product Lead""
					}
				],
				""featured"": true,
				""visible_in_landing_page"": true,
				""featured_in_lobby"": false,
				""visible_in_lobby"": true,
				""is_simulive"": true,
				""record_file_id"": ""f09340e1-cdc3-4eae-9a74-98f9777ed908"",
				""chat_channel"": true,
				""led_by_sponsor"": false,
				""track_labels"": [ ""Technical Track"" ],
				""audience_labels"": [ ""Family"" ],
				""product_labels"": [ ""zoomMeeting"" ],
				""level"": [ ""Level-1"" ],
				""alternative_host"": [ ""abc.cd@email.com"" ],
				""panelist"": [ ""abc.cd@email.com"" ],
				""attendance_type"": ""hybrid"",
				""physical_location"": ""801 Mt Vernon Pl NW, Washington, DC 20001"",
				""session_reservation"": {
					""allow_reservations"": true,
					""max_capacity"": 20
				}
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions"))
				.Respond("application/json", sessionJson);

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
				.Respond("application/json", SESSIONS_JSON);

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
			var sessionJson = @"{
				""id"": ""session789"",
				""name"": ""Session 1""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId))
				.Respond("application/json", sessionJson);

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
			var tokenJson = @"{""join_token"": ""token_abc123""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "sessions", sessionId, "join_token"))
				.Respond("application/json", tokenJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetSessionJoinTokenAsync(eventId, sessionId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("token_abc123");
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
			var speakerJson = @"{
				""speaker_id"": ""3935Ug73Sp6S-7K1BHk7qw"",
				""name"": ""John Joseph Dev"",
				""email"": ""email@example.com"",
				""job_title"": ""Product Manager"",
				""biography"": ""Provide a brief introduction of the speaker."",
				""company_name"": ""zoom"",
				""company_website"": ""https://www.example.com"",
				""linkedin_url"": ""https://linkedin.com/example"",
				""twitter_url"": ""https://twitter.com/example"",
				""youtube_url"": ""https://youtube.com/example"",
				""featured_in_event_detail_page"": true,
				""visible_in_event_detail_page"": true,
				""featured_in_lobby"": false,
				""visible_in_lobby"": true
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers"))
				.Respond("application/json", speakerJson);

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
				.Respond("application/json", SPEAKERS_JSON);

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
			var speakerJson = @"{
				""id"": ""speaker456"",
				""name"": ""John Doe""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "speakers", speakerId))
				.Respond("application/json", speakerJson);

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
			var ticketTypeJson = @"{""ticket_type_id"": ""ticket_123""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", ticketTypeJson);

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
			result.ShouldBe("ticket_123");
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
			var ticketTypeJson = @"{""ticket_type_id"": ""ticket_free_456""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", ticketTypeJson);

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
			result.ShouldBe("ticket_free_456");
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
			var ticketTypeJson = @"{""ticket_type_id"": ""ticket_vip_789""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", ticketTypeJson);

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
			result.ShouldBe("ticket_vip_789");
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
			var ticketTypeJson = @"{""ticket_type_id"": ""ticket_early_999""}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", ticketTypeJson);

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
			result.ShouldBe("ticket_early_999");
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
			var ticketTypesJson = @"{
				""ticket_types"": [
					{
						""ticket_type_id"": ""ticket1"",
						""name"": ""General Admission"",
						""start_time"": ""2023-06-01T00:00:00Z"",
						""end_time"": ""2023-06-30T23:59:59Z"",
						""currency"": ""USD"",
						""price"": ""50.00"",
						""free"": false,
						""quantity"": 100,
						""sold_quantity"": 25,
						""description"": ""Standard ticket""
					},
					{
						""ticket_type_id"": ""ticket2"",
						""name"": ""VIP"",
						""start_time"": ""2023-06-01T00:00:00Z"",
						""end_time"": ""2023-06-30T23:59:59Z"",
						""currency"": ""USD"",
						""price"": ""150.00"",
						""free"": false,
						""quantity"": 20,
						""sold_quantity"": 5,
						""description"": ""VIP access ticket""
					},
					{
						""ticket_type_id"": ""ticket3"",
						""name"": ""Free Entry"",
						""start_time"": ""2023-06-01T00:00:00Z"",
						""end_time"": ""2023-06-30T23:59:59Z"",
						""currency"": ""USD"",
						""free"": true,
						""quantity"": 200,
						""sold_quantity"": 50
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types"))
				.Respond("application/json", ticketTypesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketTypesAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
			result[0].Id.ShouldBe("ticket1");
			result[0].Name.ShouldBe("General Admission");
			result[1].Id.ShouldBe("ticket2");
			result[1].Name.ShouldBe("VIP");
			result[2].Id.ShouldBe("ticket3");
			result[2].IsFree.ShouldBeTrue();
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
			var questionsJson = @"{
				""questions"": [
					{
						""field_name"": ""first_name"",
						""required"": true
					},
					{
						""field_name"": ""last_name"",
						""required"": true
					},
					{
						""field_name"": ""email"",
						""required"": true
					}
				],
				""custom_questions"": [
					{
						""title"": ""Dietary Restrictions"",
						""type"": ""short"",
						""required"": false
					},
					{
						""title"": ""T-Shirt Size"",
						""type"": ""single"",
						""required"": true,
						""options"": [ ""S"", ""M"", ""L"", ""XL"" ]
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond("application/json", questionsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(3);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetRegistrationQuestionsForEventAsync_EmptyQuestions()
		{
			// Arrange
			var eventId = "event123";
			var questionsJson = @"{
				""questions"": [],
				""custom_questions"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "questions"))
				.Respond("application/json", questionsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForEventAsync(eventId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(0);
			result.CustomQuestions.ShouldNotBeNull();
			result.CustomQuestions.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetRegistrationQuestionsForTicketTypeAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "ticket_456";
			var questionsJson = @"{
				""questions"": [
					{
						""field_name"": ""first_name"",
						""required"": true
					},
					{
						""field_name"": ""email"",
						""required"": true
					}
				],
				""custom_questions"": [
					{
						""title"": ""Company Name"",
						""type"": ""short"",
						""required"": true
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "ticket_types", ticketTypeId, "questions"))
				.Respond("application/json", questionsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetRegistrationQuestionsForTicketTypeAsync(eventId, ticketTypeId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.StandardQuestions.ShouldNotBeNull();
			result.StandardQuestions.Length.ShouldBe(2);
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
			var responseJson = @"{
				""tickets"": [
					{
						""ticket_id"": ""ticket1"",
						""email"": ""attendee1@example.com"",
						""ticket_type_id"": ""ticket_type_1"",
						""first_name"": ""John"",
						""last_name"": ""Doe"",
						""city"": ""New York""
					},
					{
						""ticket_id"": ""ticket2"",
						""email"": ""attendee2@example.com"",
						""ticket_type_id"": ""ticket_type_1"",
						""first_name"": ""Jane"",
						""last_name"": ""Smith"",
						""city"": ""Los Angeles""
					}
				],
				""errors"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", responseJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.ShouldNotBeNull();
			result.Tickets.Length.ShouldBe(2);
			result.Errors.ShouldNotBeNull();
			result.Errors.Length.ShouldBe(0);
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
			var responseJson = @"{
				""tickets"": [
					{
						""ticket_id"": ""ticket_abc"",
						""email"": ""test@example.com"",
						""ticket_type_id"": ""ticket_type_1""
					}
				],
				""errors"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.Respond("application/json", responseJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, source, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.Length.ShouldBe(1);
			result.Errors.Length.ShouldBe(0);
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
			var responseJson = @"{
				""tickets"": [
					{
						""ticket_id"": ""ticket1"",
						""email"": ""valid@example.com"",
						""ticket_type_id"": ""ticket_type_1""
					}
				],
				""errors"": [
					{
						""email"": ""invalid@example.com"",
						""error_code"": ""INVALID_TICKET_TYPE"",
						""message"": ""Invalid ticket type ID""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", responseJson);

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
			result.Errors[0].Email.ShouldBe("invalid@example.com");
			result.Errors[0].ErrorCode.ShouldBe("INVALID_TICKET_TYPE");
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
			var responseJson = @"{
				""tickets"": [
					{
						""ticket_id"": ""ticket_complete"",
						""email"": ""complete@example.com"",
						""ticket_type_id"": ""ticket_type_1"",
						""external_ticket_id"": ""ext_123""
					}
				],
				""errors"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("validation_level", "standard")
				.Respond("application/json", responseJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.CreateTicketsAsync(eventId, tickets, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Tickets.Length.ShouldBe(1);
			result.Tickets[0].Id.ShouldBe("ticket_complete");
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
			var ticketJson = @"{
				""ticket_id"": ""ticket456"",
				""email"": ""attendee@example.com"",
				""ticket_type_id"": ""type123"",
				""first_name"": ""John"",
				""last_name"": ""Doe"",
				""city"": ""Seattle"",
				""state"": ""WA"",
				""country"": ""US"",
				""phone"": ""+1-206-555-0100"",
				""job_title"": ""Software Engineer"",
				""organization"": ""Tech Company"",
				""event_join_link"": ""https://zoom.us/events/join/abc123""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets", ticketId))
				.Respond("application/json", ticketJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetTicketAsync(eventId, ticketId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("ticket456");
			result.Email.ShouldBe("attendee@example.com");
			result.FirstName.ShouldBe("John");
			result.LastName.ShouldBe("Doe");
		}

		[Fact]
		public async Task GetAllTicketsAsync()
		{
			// Arrange
			var eventId = "event123";
			var ticketsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""tickets"": [
					{
						""ticket_id"": ""ticket1"",
						""email"": ""user1@example.com"",
						""ticket_type_id"": ""type1"",
						""first_name"": ""Alice"",
						""last_name"": ""Johnson""
					},
					{
						""ticket_id"": ""ticket2"",
						""email"": ""user2@example.com"",
						""ticket_type_id"": ""type1"",
						""first_name"": ""Bob"",
						""last_name"": ""Williams""
					},
					{
						""ticket_id"": ""ticket3"",
						""email"": ""user3@example.com"",
						""ticket_type_id"": ""type2"",
						""first_name"": ""Carol"",
						""last_name"": ""Brown""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", ticketsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketsAsync(eventId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(3);
			result.Records[0].Id.ShouldBe("ticket1");
			result.Records[1].Id.ShouldBe("ticket2");
			result.Records[2].Id.ShouldBe("ticket3");
		}

		[Fact]
		public async Task GetAllTicketsAsync_WithTicketTypeFilter()
		{
			// Arrange
			var eventId = "event123";
			var ticketTypeId = "type_vip";
			var ticketsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""tickets"": [
					{
						""ticket_id"": ""ticket_vip_1"",
						""email"": ""vip1@example.com"",
						""ticket_type_id"": ""type_vip""
					},
					{
						""ticket_id"": ""ticket_vip_2"",
						""email"": ""vip2@example.com"",
						""ticket_type_id"": ""type_vip""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("ticket_type_id", ticketTypeId)
				.WithQueryString("page_size", "30")
				.Respond("application/json", ticketsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketsAsync(eventId, ticketTypeId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllTicketsAsync_WithPagination()
		{
			// Arrange
			var eventId = "event123";
			var recordsPerPage = 50;
			var pagingToken = "next_page_token_123";
			var ticketsJson = @"{
				""page_size"": 50,
				""next_page_token"": ""next_token_456"",
				""tickets"": [
					{
						""ticket_id"": ""ticket50"",
						""email"": ""user50@example.com"",
						""ticket_type_id"": ""type1""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("zoom_events", "events", eventId, "tickets"))
				.WithQueryString("page_size", "50")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", ticketsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var events = new Events(client);

			// Act
			var result = await events.GetAllTicketsAsync(eventId, recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.PageSize.ShouldBe(50);
			result.NextPageToken.ShouldBe("next_token_456");
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
	}
}
