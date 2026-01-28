using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class MeetingsTests
	{
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_GET);

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
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe(97763643886);
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetAllAsync(userId, from: from, to: to, timeZone: timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateInstantMeetingAsync(userId, topic, agenda, password, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(92674392836);
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateScheduledMeetingAsync(userId, topic, agenda, start, duration, timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(92674392836);
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateRecurringMeetingAsync(userId, topic, agenda, start, duration, recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(92674392836);
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
			await Should.ThrowAsync<ArgumentException>(() => meetings.CreateInstantMeetingAsync(userId, topic, agenda, password, generatePassword: generatePassword, cancellationToken: TestContext.Current.CancellationToken));
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(97763643886);
			result.Topic.ShouldBe("My Meeting");
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
		public async Task GetRegistrantAsync_ReturnsRegistrant()
		{
			// Arrange
			var meetingId = 1234567890L;
			var registrantId = "registrant_id";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", registrantId))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__registrants__registrantId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetRegistrantAsync(meetingId, registrantId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Address.ShouldBe("1800 Amphibious Blvd.");
		}

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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetRegistrantsAsync(meetingId, status, recordsPerPage: 30, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("9tboDiHUQAeOnbmudzWa5g");
			result.Records[0].Email.ShouldBe("jchill@example.com");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.AddRegistrantAsync(meetingId, email, firstName, lastName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("fdgsfh2ey82fuh");
			result.EventId.ShouldBe(85746065);
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__batch_registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.PerformBatchRegistrationAsync(meetingId, registrants, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("9tboDiHUQAeOnbmudzWa5g");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__polls_GET);

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
			result[0].Title.ShouldBe("Learn something new");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__polls_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreatePollAsync(meetingId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Title.ShouldBe("Learn something new");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__polls__pollId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetPollAsync(meetingId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("QalIoKWLTJehBJ8e1xRrbQ");
			result.Title.ShouldBe("Learn something new");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__livestream_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetLiveStreamSettingsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Url.ShouldBe("https://example.com/livestream");
			result.Key.ShouldBe("contact-ic@example.com");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__invitation_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetInvitationAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldStartWith("Jill Chill is inviting you to a scheduled Zoom meeting");
			result.ShouldContain("https://zoom.us/j/55544443210?pwd=8pEkRweVXPV3Ob2KJYgFTRlDtl1gSn.1");
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__invite_links_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateInviteLinksAsync(meetingId, names, timeToLive, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Name.ShouldBe("Jill Chill");
		}

		[Fact]
		public async Task GetSurveyAsync_WithMeetingId_ReturnsSurvey()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "survey"))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__survey_GET);

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
				.Respond("application/json", EndpointsResponseResource.users__userId__meeting_templates_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTemplatesAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Name.ShouldBe("My meeting template");
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meeting_templates_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.CreateTemplateFromExistingMeetingAsync(userId, meetingId, templateName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("AdxbhxCzKgSiWAw");
		}

		[Fact]
		public async Task GetTokenForClosedCaptioningAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", $"{meetingId}/token?type=closed_caption_token"))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__token_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForClosedCaptioningAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("https://example.com/closedcaption?id=200610693&ns=GZHkEA==&expire=86400&spparams=id%2Cns%2Cexpire&signature=nYtXJqRKCW");
		}

		[Fact]
		public async Task GetTokenForLocalRecordingAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "jointoken", "local_recording"))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__jointoken_local_recording_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLocalRecordingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("2njt50mj");
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

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "registrants", "questions"))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__registrants_questions_GET);

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
			result.OptionalFields.Length.ShouldBe(0);
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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__jointoken_local_archiving_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLocalArchivingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("2njt50mj");
		}

		[Fact]
		public async Task GetTokenForLiveStreamingAsync_WithMeetingId_ReturnsToken()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId.ToString(), "jointoken", "live_streaming"))
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__jointoken_live_streaming_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var meetings = new Meetings(client);

			// Act
			var result = await meetings.GetTokenForLiveStreamingAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("2njt50mj");
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
				.Respond("application/json", EndpointsResponseResource.users__userId__meetings_GET);

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
				.Respond("application/json", EndpointsResponseResource.meetings__meetingId__registrants_GET);

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
