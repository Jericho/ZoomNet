using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class MeetingsTests
	{
		private const string MEETINGS_LIST_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""meetings"": [
				{
					""uuid"": ""meeting1_uuid=="",
					""id"": 1234567890,
					""host_id"": ""host123"",
					""topic"": ""Team Sync"",
					""type"": 2,
					""start_time"": ""2023-06-01T10:00:00Z"",
					""duration"": 60,
					""timezone"": ""UTC"",
					""created_at"": ""2023-05-20T08:00:00Z"",
					""join_url"": ""https://zoom.us/j/1234567890"",
					""agenda"": ""Weekly team synchronization meeting""
				},
				{
					""uuid"": ""meeting2_uuid=="",
					""id"": 9876543210,
					""host_id"": ""host456"",
					""topic"": ""Project Review"",
					""type"": 8,
					""start_time"": ""2023-06-05T14:00:00Z"",
					""duration"": 90,
					""timezone"": ""UTC"",
					""created_at"": ""2023-05-22T09:00:00Z"",
					""join_url"": ""https://zoom.us/j/9876543210"",
					""agenda"": ""Monthly project review""
				}
			]
		}";

		private const string INSTANT_MEETING_JSON = @"{
			""uuid"": ""instant_meeting_uuid=="",
			""id"": 1111111111,
			""host_id"": ""host789"",
			""topic"": ""Instant Discussion"",
			""type"": 1,
			""join_url"": ""https://zoom.us/j/1111111111"",
			""password"": ""secret123"",
			""h323_password"": ""654321"",
			""pstn_password"": ""789012"",
			""encrypted_password"": ""encryptedSecret123""
		}";

		private const string SCHEDULED_MEETING_JSON = @"{
			""uuid"": ""scheduled_meeting_uuid=="",
			""id"": 2222222222,
			""host_id"": ""host101"",
			""topic"": ""Scheduled Meeting"",
			""type"": 2,
			""start_time"": ""2023-07-01T15:00:00Z"",
			""duration"": 45,
			""timezone"": ""America/New_York"",
			""join_url"": ""https://zoom.us/j/2222222222"",
			""password"": ""pass456"",
			""agenda"": ""Quarterly planning session""
		}";

		private const string RECURRING_MEETING_JSON = @"{
			""uuid"": ""recurring_meeting_uuid=="",
			""id"": 3333333333,
			""host_id"": ""host202"",
			""topic"": ""Daily Standup"",
			""type"": 8,
			""start_time"": ""2023-06-01T09:00:00Z"",
			""duration"": 15,
			""timezone"": ""UTC"",
			""join_url"": ""https://zoom.us/j/3333333333"",
			""recurrence"": {
				""type"": 1,
				""repeat_interval"": 1
			}
		}";

		private const string MEETING_REGISTRANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""regtoken123"",
			""registrants"": [
				{
					""id"": ""reg001"",
					""email"": ""user1@example.com"",
					""first_name"": ""John"",
					""last_name"": ""Doe"",
					""status"": ""approved"",
					""create_time"": ""2023-05-25T10:00:00Z"",
					""join_url"": ""https://zoom.us/w/reg001""
				},
				{
					""id"": ""reg002"",
					""email"": ""user2@example.com"",
					""first_name"": ""Jane"",
					""last_name"": ""Smith"",
					""status"": ""pending"",
					""create_time"": ""2023-05-26T11:00:00Z"",
					""join_url"": ""https://zoom.us/w/reg002""
				}
			]
		}";

		private const string REGISTRANT_INFO_JSON = @"{
			""id"": 85746065,
			""join_url"": ""https://example.com/j/11111"",
			""registrant_id"": ""fdgsfh2ey82fuh"",
			""start_time"": ""2021-07-13T21:44:51Z"",
			""topic"": ""My Meeting"",
			""occurrences"": [
				{
					""duration"": 60,
					""occurrence_id"": ""1648194360000"",
					""start_time"": ""2022-03-25T07:46:00Z"",
					""status"": ""available""
				}
			],
			""participant_pin_code"": 380303
		}";

		private const string BATCH_REGISTRANTS_JSON = @"{
			""registrants"": [
				{
					""id"": ""batch_reg_001"",
					""registrant_id"": ""batch_reg_001"",
					""email"": ""batch1@example.com"",
					""join_url"": ""https://zoom.us/w/batch_reg_001""
				},
				{
					""id"": ""batch_reg_002"",
					""registrant_id"": ""batch_reg_002"",
					""email"": ""batch2@example.com"",
					""join_url"": ""https://zoom.us/w/batch_reg_002""
				}
			]
		}";

		private const string POLLS_JSON = @"{
			""polls"": [
				{
					""id"": ""poll001"",
					""title"": ""Meeting Feedback"",
					""status"": ""notstart"",
					""questions"": [
						{
							""name"": ""question1"",
							""type"": ""single"",
							""answer_required"": true,
							""answers"": [""Excellent"", ""Good"", ""Average"", ""Poor""]
						}
					]
				}
			]
		}";

		private const string POLL_JSON = @"{
			""id"": ""poll001"",
			""title"": ""Meeting Feedback"",
			""status"": ""notstart"",
			""questions"": [
				{
					""name"": ""question1"",
					""type"": ""single"",
					""answer_required"": true,
					""answers"": [""Excellent"", ""Good"", ""Average"", ""Poor""]
				}
			]
		}";

		private const string INVITATION_JSON = @"{
			""invitation"": ""You are invited to join the meeting.\n\nJoin URL: https://zoom.us/j/1234567890\nMeeting ID: 1234567890\nPassword: secret123""
		}";

		private const string LIVE_STREAM_SETTINGS_JSON = @"{
			""stream_url"": ""https://stream.example.com/live"",
			""stream_key"": ""streamkey123"",
			""page_url"": ""https://example.com/meeting""
		}";

		private const string INVITE_LINKS_JSON = @"{
			""attendees"": [
				{
					""name"": ""Attendee One"",
					""join_url"": ""https://zoom.us/j/invite123""
				},
				{
					""name"": ""Attendee Two"",
					""join_url"": ""https://zoom.us/j/invite456""
				}
			]
		}";

		private const string SURVEY_JSON = @"{
			""id"": ""survey123"",
			""status"": ""notstart"",
			""show_in_the_browser"": true,
			""custom_survey"": {
				""anonymous"": true,
				""questions"": [
					{
						""name"": ""How satisfied are you?"",
						""type"": ""single"",
						""answer_required"": true
					}
				]
			}
		}";

		private const string TEMPLATES_JSON = @"{
			""templates"": [
				{
					""id"": ""template001"",
					""name"": ""Standard Meeting Template"",
					""type"": 2
				},
				{
					""id"": ""template002"",
					""name"": ""Webinar Template"",
					""type"": 5
				}
			]
		}";

		private const string TOKEN_JSON = @"{
			""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.test.token""
		}";

		private const string TEMPLATE_ID_JSON = @"{
			""id"": ""newtemplate123""
		}";

		private readonly ITestOutputHelper _outputHelper;

		public MeetingsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync_WithPaginationToken_ReturnsMeetings()
		{
			// Arrange
			var userId = "user123";
			var type = MeetingListType.Scheduled;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "meetings"))
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", MEETINGS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await meetings.GetAllAsync(userId, type, cancellationToken: TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe(1234567890);
			result.Records[0].Topic.ShouldBe("Team Sync");
			result.Records[1].Id.ShouldBe(9876543210);
		}

		[Fact]
		public async Task GetAllAsync_WithDateRange_ReturnsMeetings()
		{
			// Arrange
			var userId = "user456";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var timeZone = TimeZones.America_New_York;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "meetings"))
				.WithQueryString("from", from.ToZoomFormat(timeZone))
				.WithQueryString("to", to.ToZoomFormat(timeZone))
				.WithQueryString("timezone", timeZone.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", MEETINGS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetAllAsync(userId, from: from, to: to, timeZone: timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		#endregion

		#region Create Meeting Tests

		[Fact]
		public async Task CreateInstantMeetingAsync_WithValidParameters_ReturnsMeeting()
		{
			// Arrange
			var userId = "user123";
			var topic = "Instant Discussion";
			var agenda = "Quick sync meeting";
			var password = "secret123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "meetings"))
				.Respond("application/json", INSTANT_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateInstantMeetingAsync(userId, topic, agenda, password, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(1111111111);
			result.Topic.ShouldBe("Instant Discussion");
			result.Type.ShouldBe(MeetingType.Instant);
		}

		[Fact]
		public async Task CreateScheduledMeetingAsync_WithValidParameters_ReturnsMeeting()
		{
			// Arrange
			var userId = "user123";
			var topic = "Scheduled Meeting";
			var agenda = "Quarterly planning";
			var start = new DateTime(2023, 7, 1, 15, 0, 0);
			var duration = 45;
			var timeZone = TimeZones.America_New_York;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "meetings"))
				.Respond("application/json", SCHEDULED_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateScheduledMeetingAsync(userId, topic, agenda, start, duration, timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(2222222222);
			result.Topic.ShouldBe("Scheduled Meeting");
			result.Type.ShouldBe(MeetingType.Scheduled);
			result.Duration.ShouldBe(45);
		}

		[Fact]
		public async Task CreateRecurringMeetingAsync_WithRecurrence_ReturnsMeeting()
		{
			// Arrange
			var userId = "user123";
			var topic = "Daily Standup";
			var agenda = "Daily team standup";
			var start = new DateTime(2023, 6, 1, 9, 0, 0);
			var duration = 15;
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Daily,
				RepeatInterval = 1
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "meetings"))
				.Respond("application/json", RECURRING_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateRecurringMeetingAsync(userId, topic, agenda, start, duration, recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(3333333333);
			result.Topic.ShouldBe("Daily Standup");
			result.Type.ShouldBe(MeetingType.RecurringFixedTime);
		}

		[Fact]
		public async Task CreateInstantMeetingAsync_WithPasswordAndGeneratePassword_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var topic = "Test Meeting";
			var agenda = "Test Agenda";
			var password = "test123";
			var generatePassword = true;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act & Assert
			Should.Throw<ArgumentException>(async () =>
			{
				await meetings.CreateInstantMeetingAsync(userId, topic, agenda, password, generatePassword: generatePassword, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		#endregion

		#region GetAsync Test

		[Fact]
		public async Task GetAsync_WithMeetingId_ReturnsMeeting()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString()))
				.WithQueryString("show_previous_occurrences", "false")
				.Respond("application/json", SCHEDULED_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(2222222222);
			result.Topic.ShouldBe("Scheduled Meeting");
		}

		#endregion

		#region Update Meeting Tests

		[Fact]
		public async Task UpdateScheduledMeetingAsync_WithValidParameters_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var topic = "Updated Meeting Topic";
			var agenda = "Updated agenda";
			var duration = 60;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateScheduledMeetingAsync(meetingId, topic: topic, agenda: agenda, duration: duration, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRecurringMeetingAsync_WithRecurrence_Succeeds()
		{
			// Arrange
			var meetingId = 3333333333L;
			var topic = "Updated Recurring Meeting";
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateRecurringMeetingAsync(meetingId, topic: topic, recurrence: recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateMeetingOccurrenceAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var meetingId = 3333333333L;
			var occurrenceId = "occurrence123";
			var agenda = "Updated occurrence agenda";
			var start = new DateTime(2023, 6, 15, 10, 0, 0);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString()))
				.WithQueryString("occurrence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateMeetingOccurrenceAsync(meetingId, occurrenceId, agenda, start, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Delete and Status Tests

		[Fact]
		public async Task DeleteAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId.ToString()))
				.WithQueryString("schedule_for_reminder", "true")
				.WithQueryString("cancel_meeting_reminder", "false")
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.DeleteAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task EndAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.EndAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RecoverAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.RecoverAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Registrant Tests

		[Fact]
		public async Task GetRegistrantsAsync_WithStatus_ReturnsRegistrants()
		{
			// Arrange
			var meetingId = 1234567890L;
			var status = RegistrantStatus.Approved;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants"))
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", MEETING_REGISTRANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetRegistrantsAsync(meetingId, status, recordsPerPage: 30, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("reg001");
			result.Records[0].Email.ShouldBe("user1@example.com");
			result.Records[0].Status.ShouldBe(RegistrantStatus.Approved);
		}

		[Fact]
		public async Task AddRegistrantAsync_WithRequiredFields_ReturnsRegistrantInfo()
		{
			// Arrange
			var meetingId = 1234567890L;
			var email = "newuser@example.com";
			var firstName = "New";
			var lastName = "User";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants"))
				.Respond("application/json", REGISTRANT_INFO_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.AddRegistrantAsync(meetingId, email, firstName, lastName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task PerformBatchRegistrationAsync_WithMultipleRegistrants_ReturnsRegistrantInfos()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrants = new[]
			{
				new BatchRegistrant { Email = "batch1@example.com", FirstName = "Batch", LastName = "One" },
				new BatchRegistrant { Email = "batch2@example.com", FirstName = "Batch", LastName = "Two" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "batch_registrants"))
				.Respond("application/json", BATCH_REGISTRANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.PerformBatchRegistrationAsync(meetingId, registrants, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("batch_reg_001");
			result[1].Id.ShouldBe("batch_reg_002");
		}

		[Fact]
		public async Task ApproveRegistrantAsync_WithRegistrantInfo_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantId = "reg001";
			var registrantEmail = "user1@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.ApproveRegistrantAsync(meetingId, registrantId, registrantEmail, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteRegistrantAsync_WithRegistrantId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantId = "reg001";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", registrantId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.DeleteRegistrantAsync(meetingId, registrantId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantAsync_WithRegistrantInfo_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantId = "reg001";
			var registrantEmail = "user1@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.RejectRegistrantAsync(meetingId, registrantId, registrantEmail, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantsAsync_WithMultipleRegistrants_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantsInfo = new[]
			{
				("reg001", "user1@example.com"),
				("reg002", "user2@example.com")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.RejectRegistrantsAsync(meetingId, registrantsInfo, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelRegistrantAsync_WithRegistrantInfo_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantId = "reg001";
			var registrantEmail = "user1@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.CancelRegistrantAsync(meetingId, registrantId, registrantEmail, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelRegistrantsAsync_WithMultipleRegistrants_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantsInfo = new[]
			{
				("reg001", "user1@example.com"),
				("reg002", "user2@example.com"),
				("reg003", "user3@example.com")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.CancelRegistrantsAsync(meetingId, registrantsInfo, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Poll Tests

		[Fact]
		public async Task GetPollsAsync_WithMeetingId_ReturnsPolls()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", POLLS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetPollsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Title.ShouldBe("Meeting Feedback");
		}

		[Fact]
		public async Task CreatePollAsync_WithTitleAndQuestions_ReturnsPoll()
		{
			// Arrange
			var meetingId = 1234567890L;
			var title = "Meeting Feedback";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "How was the meeting?",
					Type = PollQuestionType.SingleChoice,
					IsRequired = true,
					Answers = new[] { "Excellent", "Good", "Average", "Poor" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", POLL_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreatePollAsync(meetingId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Title.ShouldBe("Meeting Feedback");
			result.Questions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task DeletePollAsync_WithPollId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var pollId = 1001L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.DeletePollAsync(meetingId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetPollAsync_WithPollId_ReturnsPoll()
		{
			// Arrange
			var meetingId = 1234567890L;
			var pollId = 1001L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "polls", pollId.ToString()))
				.Respond("application/json", POLL_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetPollAsync(meetingId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("poll001");
			result.Title.ShouldBe("Meeting Feedback");
		}

		[Fact]
		public async Task UpdatePollAsync_WithTitleAndQuestions_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var pollId = 1001L;
			var title = "Updated Poll Title";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Updated question?",
					Type = PollQuestionType.SingleChoice,
					IsRequired = true,
					Answers = new[] { "Yes", "No" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdatePollAsync(meetingId, pollId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Live Streaming Tests

		[Fact]
		public async Task GetLiveStreamSettingsAsync_WithMeetingId_ReturnsSettings()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "livestream"))
				.Respond("application/json", LIVE_STREAM_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetLiveStreamSettingsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Url.ShouldBe("https://stream.example.com/live");
			result.Key.ShouldBe("streamkey123");
		}

		[Fact]
		public async Task UpdateLiveStreamAsync_WithStreamInfo_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var streamUrl = "https://stream.example.com/live";
			var streamKey = "streamkey123";
			var pageUrl = "https://example.com/meeting";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString(), "livestream"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateLiveStreamAsync(meetingId, streamUrl, streamKey, pageUrl, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StartLiveStreamAsync_WithDisplayName_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var displaySpeakerName = true;
			var speakerName = "John Doe";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString(), "livestream", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StartLiveStreamAsync(meetingId, displaySpeakerName, speakerName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Cloud Recording Tests

		[Fact]
		public async Task StartCloudRecordingAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StartCloudRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task PauseCloudRecordingAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.PauseCloudRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StopCloudRecordingAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StopCloudRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task ResumeCloudRecordingAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.ResumeCloudRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Additional Tests

		[Fact]
		public async Task GetInvitationAsync_WithMeetingId_ReturnsInvitation()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "invitation"))
				.Respond("application/json", INVITATION_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetInvitationAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldContain("You are invited");
			result.ShouldContain("https://zoom.us/j/1234567890");
		}

		[Fact]
		public async Task CreateInviteLinksAsync_WithNames_ReturnsInviteLinks()
		{
			// Arrange
			var meetingId = 1234567890L;
			var names = new[] { "Attendee One", "Attendee Two" };
			var timeToLive = 3600L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "invite_links"))
				.Respond("application/json", INVITE_LINKS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateInviteLinksAsync(meetingId, names, timeToLive, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Name.ShouldBe("Attendee One");
			result[1].Name.ShouldBe("Attendee Two");
		}

		[Fact]
		public async Task GetSurveyAsync_WithMeetingId_ReturnsSurvey()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "survey"))
				.Respond("application/json", SURVEY_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetSurveyAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShowInBrowser.ShouldBeTrue();
		}

		[Fact]
		public async Task GetTemplatesAsync_WithUserId_ReturnsTemplates()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "meeting_templates"))
				.Respond("application/json", TEMPLATES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTemplatesAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Name.ShouldBe("Standard Meeting Template");
			result[1].Name.ShouldBe("Webinar Template");
		}

		[Fact]
		public async Task CreateTemplateFromExistingMeetingAsync_WithMeetingId_ReturnsTemplateId()
		{
			// Arrange
			var userId = "user123";
			var meetingId = 1234567890L;
			var templateName = "My Custom Template";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "meeting_templates"))
				.Respond("application/json", TEMPLATE_ID_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateTemplateFromExistingMeetingAsync(userId, meetingId, templateName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("newtemplate123");
		}

		[Fact]
		public async Task GetTokenForClosedCaptioningAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", $"{meetingId}/token?type=closed_caption_token"))
				.Respond("application/json", TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForClosedCaptioningAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldStartWith("eyJ");
		}

		[Fact]
		public async Task GetTokenForLocalRecordingAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "jointoken", "local_recording"))
				.Respond("application/json", TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLocalRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldStartWith("eyJ");
		}

		[Fact]
		public async Task InviteParticipantsByEmailAsync_WithEmailAddresses_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var emailAddresses = new[] { "user1@example.com", "user2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantsByEmailAsync(meetingId, emailAddresses, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Registration Questions Tests

		[Fact]
		public async Task GetRegistrationQuestionsAsync_WithMeetingId_ReturnsQuestions()
		{
			// Arrange
			var meetingId = 1234567890L;

			var registrationQuestionsJson = @"{
				""custom_questions"": [
					{
						""answers"": [ ""Good"" ],
						""required"": true,
						""title"": ""How are you?"",
						""type"": ""short""
					}
				],
				""questions"": [
					{
						""field_name"": ""last_name"",
						""required"": true
					},
					{
						""field_name"": ""city"",
						""required"": false
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "questions"))
				.Respond("application/json", registrationQuestionsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetRegistrationQuestionsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequiredFields.ShouldNotBeNull();
			result.RequiredFields.Length.ShouldBe(1);
			result.OptionalFields.ShouldNotBeNull();
			result.OptionalFields.Length.ShouldBe(1);
			result.Questions.ShouldNotBeNull();
			result.Questions.Length.ShouldBe(1);
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsAsync_WithFields_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var requiredFields = new[] { RegistrationField.LastName };
			var optionalFields = new[] { RegistrationField.Organization, RegistrationField.City };
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForMeeting
				{
					Title = "Custom Question",
					Type = RegistrationCustomQuestionTypeForMeeting.Short,
					IsRequired = false
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "questions"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateRegistrationQuestionsAsync(meetingId, requiredFields, optionalFields, customQuestions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Survey Tests

		[Fact]
		public async Task UpdateSurveyAsync_WithQuestions_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var questions = new[]
			{
				new SurveyQuestion
				{
					Name = "How satisfied are you?",
					Type = SurveyQuestionType.Rating_Scale,
					IsRequired = true
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateSurveyAsync(meetingId, questions, true, true, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSurveyAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.DeleteSurveyAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Participant Invitation Tests

		[Fact]
		public async Task InviteParticipantsByIdAsync_WithUserIds_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var userIds = new[] { "user1", "user2", "user3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantsByIdAsync(meetingId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task InviteParticipantByPhoneAsync_WithPhoneNumber_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var phoneNumber = "+1234567890";
			var inviteeName = "John Doe";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantByPhoneAsync(meetingId, phoneNumber, inviteeName, false, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task InviteParticipantByPhoneAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var phoneNumber = "+1234567890-123";
			var inviteeName = "Jane Smith";
			var requireGreeting = true;
			var requirePressingOne = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantByPhoneAsync(meetingId, phoneNumber, inviteeName, requireGreeting, requirePressingOne, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task InviteParticipantByRoomSystemH323Async_WithDeviceAddress_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var deviceAddress = "192.168.1.100";
			var fromDisplayName = "Conference Room A";
			var toDisplayName = "Meeting Host";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantByRoomSystemH323Async(meetingId, deviceAddress, fromDisplayName, toDisplayName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task InviteParticipantByRoomSystemSipAsync_WithDeviceAddress_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var deviceAddress = "sip.example.com";
			var fromUri = "sip:sender@example.com";
			var fromDisplayName = "Conference Room B";
			var toDisplayName = "Meeting Attendee";
			var customHeaders = new[]
			{
				new KeyValuePair<string, string>("X-Custom-Header", "CustomValue")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.InviteParticipantByRoomSystemSipAsync(meetingId, deviceAddress, fromUri, fromDisplayName, toDisplayName, customHeaders, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Waiting Room Tests

		[Fact]
		public async Task UpdateWaitingRoomAsync_WithTitle_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var title = "Updated Waiting Room Title";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateWaitingRoomAsync(meetingId, title, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateWaitingRoomAsync_WithTitleAndDescription_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var title = "Waiting Room";
			var description = "Please wait while the host admits you to the meeting.";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.UpdateWaitingRoomAsync(meetingId, title, description, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region AI Companion Tests

		[Fact]
		public async Task StartAiCompanionAsync_WithDefaultMode_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StartAiCompanionAsync(meetingId, AiCompanionMode.All, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StopAiCompanionAsync_WithDefaultMode_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StopAiCompanionAsync(meetingId, AiCompanionMode.All, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StopAiCompanionAsync_WithDeleteAssets_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StopAiCompanionAsync(meetingId, AiCompanionMode.All, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DisableAiCompanionAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.DisableAiCompanionAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Token Tests

		[Fact]
		public async Task GetTokenForLocalArchivingAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "jointoken", "local_archiving"))
				.Respond("application/json", TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLocalArchivingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldStartWith("eyJ");
		}

		[Fact]
		public async Task GetTokenForLiveStreamingAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "jointoken", "live_streaming"))
				.Respond("application/json", TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLiveStreamingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldStartWith("eyJ");
		}

		#endregion

		#region Obsolete Method Tests

		[Fact]
		public async Task InviteParticipantsAsync_WithEmailAddresses_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;
			var emailAddresses = new[] { "user1@example.com", "user2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("live_meetings", meetingId.ToString(), "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			await meetings.InviteParticipantsAsync(meetingId, emailAddresses, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAllAsync_WithObsoletePageNumber_ReturnsObsoleteResponse()
		{
			// Arrange
			var userId = "user123";
			var type = MeetingListType.Live;
			var recordsPerPage = 30;
			var page = 2;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "meetings"))
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", page.ToString())
				.Respond("application/json", MEETINGS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await meetings.GetAllAsync(userId, type, recordsPerPage, page, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRegistrantsAsync_WithObsoletePageNumber_ReturnsObsoleteResponse()
		{
			// Arrange
			var meetingId = 1234567890L;
			var status = RegistrantStatus.Pending;
			var recordsPerPage = 30;
			var page = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants"))
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", page.ToString())
				.Respond("application/json", MEETING_REGISTRANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await meetings.GetRegistrantsAsync(meetingId, status, null, recordsPerPage, page, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task StopLiveStreamAsync_WithMeetingId_Succeeds()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("meetings", meetingId.ToString(), "livestream", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			await meetings.StopLiveStreamAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
