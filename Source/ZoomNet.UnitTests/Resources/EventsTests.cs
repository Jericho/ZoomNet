using RichardSzalay.MockHttp;
using Shouldly;
using System;
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

		#region Event CRUD Tests

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

		#endregion

		#region Validation Tests

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

		#endregion
	}
}
