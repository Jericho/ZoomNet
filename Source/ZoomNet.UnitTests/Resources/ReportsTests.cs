using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class ReportsTests
	{
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
				.Respond("application/json", EndpointsResource.report_meetings__meetingId__participants_GET);

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
				.Respond("application/json", EndpointsResource.report_users__userId__meetings_GET);

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
				.Respond("application/json", EndpointsResource.report_webinars__webinarId__participants_GET);

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
				.Respond("application/json", EndpointsResource.report_users_GET);

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
				.Respond("application/json", EndpointsResource.report_daily_GET);

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
				.Respond("application/json", EndpointsResource.report_daily_GET);

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
				.Respond("application/json", EndpointsResource.report_users__userId__meetings_GET);

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
				.Respond("application/json", EndpointsResource.report_users_GET);

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
				.Respond("application/json", EndpointsResource.report_users__userId__meetings_GET);

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
