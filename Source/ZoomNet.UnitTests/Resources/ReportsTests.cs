using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class ReportsTests
	{
		private const string MEETING_PARTICIPANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""participants"": [
				{
					""id"": ""participant1"",
					""name"": ""Alice Johnson"",
					""user_email"": ""alice@example.com"",
					""join_time"": ""2023-06-01T10:05:00Z"",
					""leave_time"": ""2023-06-01T11:30:00Z"",
					""duration"": 85,
					""customer_key"": ""CUST001"",
					""failover"": false
				},
				{
					""id"": ""participant2"",
					""name"": ""Bob Smith"",
					""user_email"": ""bob@example.com"",
					""join_time"": ""2023-06-01T10:10:00Z"",
					""leave_time"": ""2023-06-01T11:25:00Z"",
					""duration"": 75,
					""customer_key"": ""CUST002"",
					""failover"": false
				}
			]
		}";

		private const string PAST_MEETINGS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token456"",
			""meetings"": [
				{
					""uuid"": ""meeting1_uuid=="",
					""id"": 1234567890,
					""host_id"": ""host123"",
					""topic"": ""Weekly Standup"",
					""start_time"": ""2023-06-01T10:00:00Z"",
					""end_time"": ""2023-06-01T11:00:00Z"",
					""duration"": 60,
					""total_minutes"": 180,
					""participants_count"": 3,
					""type"": 2,
					""user_name"": ""John Doe"",
					""user_email"": ""john@example.com"",
					""dept"": ""Engineering"",
					""source"": ""Zoom""
				},
				{
					""uuid"": ""meeting2_uuid=="",
					""id"": 9876543210,
					""host_id"": ""host456"",
					""topic"": ""Sprint Planning"",
					""start_time"": ""2023-06-05T14:00:00Z"",
					""end_time"": ""2023-06-05T16:00:00Z"",
					""duration"": 120,
					""total_minutes"": 360,
					""participants_count"": 5,
					""type"": 2,
					""user_name"": ""Jane Smith"",
					""user_email"": ""jane@example.com"",
					""dept"": ""Product"",
					""source"": ""API""
				}
			]
		}";

		private const string WEBINAR_PARTICIPANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token789"",
			""participants"": [
				{
					""id"": ""webinar_participant1"",
					""name"": ""Carol White"",
					""user_email"": ""carol@example.com"",
					""join_time"": ""2023-06-01T14:00:00Z"",
					""leave_time"": ""2023-06-01T15:30:00Z"",
					""duration"": 90,
					""customer_key"": ""CUST003"",
					""failover"": false
				},
				{
					""id"": ""webinar_participant2"",
					""name"": ""David Brown"",
					""user_email"": ""david@example.com"",
					""join_time"": ""2023-06-01T14:05:00Z"",
					""leave_time"": ""2023-06-01T15:25:00Z"",
					""duration"": 80,
					""customer_key"": ""CUST004"",
					""failover"": false
				}
			]
		}";

		private const string HOSTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token999"",
			""users"": [
				{
					""id"": ""user001"",
					""email"": ""host1@example.com"",
					""user_name"": ""Host One"",
					""type"": 2,
					""dept"": ""Sales"",
					""meetings"": 15,
					""participants"": 120,
					""meeting_minutes"": 900
				},
				{
					""id"": ""user002"",
					""email"": ""host2@example.com"",
					""user_name"": ""Host Two"",
					""type"": 2,
					""dept"": ""Marketing"",
					""meetings"": 20,
					""participants"": 180,
					""meeting_minutes"": 1200
				}
			]
		}";

		private const string DAILY_USAGE_REPORT_JSON = @"{
			""year"": 2023,
			""month"": 6,
			""dates"": [
				{
					""date"": ""2023-06-01"",
					""meetings"": 25,
					""participants"": 200,
					""meeting_minutes"": 1500,
					""new_users"": 5,
					""total_meetings"": 25,
					""total_participants"": 200,
					""total_meeting_minutes"": 1500
				},
				{
					""date"": ""2023-06-02"",
					""meetings"": 30,
					""participants"": 250,
					""meeting_minutes"": 1800,
					""new_users"": 3,
					""total_meetings"": 55,
					""total_participants"": 450,
					""total_meeting_minutes"": 3300
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public ReportsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetMeetingParticipantsAsync Tests

		[Fact]
		public async Task GetMeetingParticipantsAsync_DefaultParameters_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "meetings", meetingId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", "30")
				.Respond("application/json", MEETING_PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingParticipantsAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.Length.ShouldBe(2);
			result.Records[0].DisplayName.ShouldBe("Alice Johnson");
			result.Records[0].Email.ShouldBe("alice@example.com");
			result.Records[0].CustomerKey.ShouldBe("CUST001");
			result.Records[0].Duration.ShouldBe(85);
			result.Records[1].DisplayName.ShouldBe("Bob Smith");
			result.Records[1].CustomerKey.ShouldBe("CUST002");
		}

		[Fact]
		public async Task GetMeetingParticipantsAsync_WithPagination_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "meeting456";
			var recordsPerPage = 10;
			var pageToken = "customPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "meetings", meetingId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", MEETING_PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingParticipantsAsync(meetingId, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetMeetingParticipantsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var meetingId = "meeting789";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetMeetingParticipantsAsync(meetingId, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetMeetingParticipantsAsync_EmptyParticipants_ReturnsEmptyArray()
		{
			// Arrange
			var meetingId = "meeting000";
			var emptyParticipantsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "meetings", meetingId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyParticipantsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingParticipantsAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetMeetingsAsync Tests

		[Fact]
		public async Task GetMeetingsAsync_ValidDateRange_ReturnsMeetings()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users", userId, "meetings"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-30")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", PAST_MEETINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingsAsync(userId, from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token456");
			result.Records.Length.ShouldBe(2);
			result.Records[0].Topic.ShouldBe("Weekly Standup");
			result.Records[0].Duration.ShouldBe(60);
			result.Records[0].ParticipantsCount.ShouldBe(3);
			result.Records[0].Department.ShouldBe("Engineering");
			result.Records[1].Topic.ShouldBe("Sprint Planning");
			result.Records[1].Duration.ShouldBe(120);
		}

		[Fact]
		public async Task GetMeetingsAsync_WithPagination_ReturnsMeetings()
		{
			// Arrange
			var userId = "user456";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 15);
			var type = ReportMeetingType.Past;
			var recordsPerPage = 20;
			var pageToken = "nextToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users", userId, "meetings"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-15")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", PAST_MEETINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingsAsync(userId, from, to, type, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetMeetingsAsync_ToDateBeforeFromDate_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 6, 30);
			var to = new DateTime(2023, 6, 1);
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetMeetingsAsync(userId, from, to, type, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetMeetingsAsync_DateRangeExceedsOneMonth_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 7, 5); // More than 30 days
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetMeetingsAsync(userId, from, to, type, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetMeetingsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetMeetingsAsync(userId, from, to, type, recordsPerPage: -1, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetMeetingsAsync_EmptyMeetings_ReturnsEmptyArray()
		{
			// Arrange
			var userId = "user000";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportMeetingType.Past;
			var emptyMeetingsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""meetings"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users", userId, "meetings"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-30")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyMeetingsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingsAsync(userId, from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetWebinarParticipantsAsync Tests

		[Fact]
		public async Task GetWebinarParticipantsAsync_DefaultParameters_ReturnsParticipants()
		{
			// Arrange
			var webinarId = "webinar123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "webinars", webinarId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", "30")
				.Respond("application/json", WEBINAR_PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetWebinarParticipantsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token789");
			result.Records.Length.ShouldBe(2);
			result.Records[0].DisplayName.ShouldBe("Carol White");
			result.Records[0].Email.ShouldBe("carol@example.com");
			result.Records[0].CustomerKey.ShouldBe("CUST003");
			result.Records[0].Duration.ShouldBe(90);
			result.Records[1].DisplayName.ShouldBe("David Brown");
			result.Records[1].CustomerKey.ShouldBe("CUST004");
		}

		[Fact]
		public async Task GetWebinarParticipantsAsync_WithPagination_ReturnsParticipants()
		{
			// Arrange
			var webinarId = "webinar456";
			var recordsPerPage = 15;
			var pageToken = "webinarPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "webinars", webinarId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", WEBINAR_PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetWebinarParticipantsAsync(webinarId, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetWebinarParticipantsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var webinarId = "webinar789";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetWebinarParticipantsAsync(webinarId, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetWebinarParticipantsAsync_EmptyParticipants_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = "webinar000";
			var emptyParticipantsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "webinars", webinarId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyParticipantsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetWebinarParticipantsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetHostsAsync Tests

		[Fact]
		public async Task GetHostsAsync_ValidDateRange_ReturnsHosts()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportHostType.Active;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-30")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", HOSTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetHostsAsync(from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token999");
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("user001");
			result.Records[0].Email.ShouldBe("host1@example.com");
			result.Records[0].DisplayName.ShouldBe("Host One");
			result.Records[0].Department.ShouldBe("Sales");
			result.Records[0].TotalMeetings.ShouldBe(15);
			result.Records[0].TotalParticipants.ShouldBe(120);
			result.Records[0].TotalMeetingMinutes.ShouldBe(900);
			result.Records[1].Id.ShouldBe("user002");
			result.Records[1].TotalMeetings.ShouldBe(20);
		}

		[Fact]
		public async Task GetHostsAsync_WithPagination_ReturnsHosts()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportHostType.Inactive;
			var recordsPerPage = 50;
			var pageToken = "hostPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-30")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", HOSTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetHostsAsync(from, to, type, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetHostsAsync_ToDateBeforeFromDate_ThrowsException()
		{
			// Arrange
			var from = new DateTime(2023, 6, 30);
			var to = new DateTime(2023, 6, 1);
			var type = ReportHostType.Active;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetHostsAsync(from, to, type, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetHostsAsync_DateRangeExceedsOneMonth_ThrowsException()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 7, 10); // More than 30 days
			var type = ReportHostType.Active;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetHostsAsync(from, to, type, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetHostsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportHostType.Active;

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => reports.GetHostsAsync(from, to, type, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetHostsAsync_EmptyHosts_ReturnsEmptyArray()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 6, 30);
			var type = ReportHostType.Active;
			var emptyHostsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""users"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-06-30")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyHostsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetHostsAsync(from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetDailyUsageReportAsync Tests

		[Fact]
		public async Task GetDailyUsageReportAsync_WithYearAndMonth_ReturnsReport()
		{
			// Arrange
			var year = 2023;
			var month = 6;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "daily"))
				.WithQueryString("year", year.ToString())
				.WithQueryString("month", month.ToString())
				.Respond("application/json", DAILY_USAGE_REPORT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetDailyUsageReportAsync(year, month, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Year.ShouldBe(2023);
			result.Month.ShouldBe(6);
			result.DailyUsageSummaries.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetDailyUsageReportAsync_WithGroupId_ReturnsReport()
		{
			// Arrange
			var year = 2023;
			var month = 6;
			var groupId = "group123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "daily"))
				.WithQueryString("year", year.ToString())
				.WithQueryString("month", month.ToString())
				.WithQueryString("groupId", groupId)
				.Respond("application/json", DAILY_USAGE_REPORT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetDailyUsageReportAsync(year, month, groupId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Year.ShouldBe(2023);
			result.Month.ShouldBe(6);
		}

		[Fact]
		public async Task GetDailyUsageReportAsync_DifferentYearAndMonth_ReturnsCorrectReport()
		{
			// Arrange
			var year = 2024;
			var month = 12;
			var differentReportJson = @"{
				""year"": 2024,
				""month"": 12,
				""dates"": [
					{
						""date"": ""2024-12-01"",
						""meetings"": 50,
						""participants"": 400,
						""meeting_minutes"": 3000,
						""new_users"": 10,
						""total_meetings"": 50,
						""total_participants"": 400,
						""total_meeting_minutes"": 3000
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "daily"))
				.WithQueryString("year", year.ToString())
				.WithQueryString("month", month.ToString())
				.Respond("application/json", differentReportJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetDailyUsageReportAsync(year, month, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Year.ShouldBe(2024);
			result.Month.ShouldBe(12);
			result.DailyUsageSummaries.Length.ShouldBe(1);
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetMeetingsAsync_ExactlyThirtyDays_WorksCorrectly()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 7, 1); // Exactly 30 days
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users", userId, "meetings"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-07-01")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", PAST_MEETINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingsAsync(userId, from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetHostsAsync_ExactlyThirtyDays_WorksCorrectly()
		{
			// Arrange
			var from = new DateTime(2023, 6, 1);
			var to = new DateTime(2023, 7, 1); // Exactly 30 days
			var type = ReportHostType.Active;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users"))
				.WithQueryString("from", "2023-06-01")
				.WithQueryString("to", "2023-07-01")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", HOSTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetHostsAsync(from, to, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetMeetingParticipantsAsync_MaxRecordsPerPage_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "meetings", meetingId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", MEETING_PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingParticipantsAsync(meetingId, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetMeetingsAsync_SameDayDateRange_WorksCorrectly()
		{
			// Arrange
			var userId = "user123";
			var date = new DateTime(2023, 6, 15);
			var type = ReportMeetingType.Past;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("report", "users", userId, "meetings"))
				.WithQueryString("from", "2023-06-15")
				.WithQueryString("to", "2023-06-15")
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", PAST_MEETINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var reports = new Reports(client);

			// Act
			var result = await reports.GetMeetingsAsync(userId, date, date, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
