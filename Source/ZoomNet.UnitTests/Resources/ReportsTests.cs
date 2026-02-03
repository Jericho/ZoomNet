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
			result.NextPageToken.ShouldBe("Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3");
			result.Records.Length.ShouldBe(1);
			result.Records[0].DisplayName.ShouldBe("example");
			result.Records[0].Email.ShouldBe("jchill@example.com");
			result.Records[0].CustomerKey.ShouldBe("349589LkJyeW");
			result.Records[0].Duration.ShouldBe(259);
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
			result.NextPageToken.ShouldBe("w7587w4eiyfsudgk");
			result.Records.Length.ShouldBe(1);
			result.Records[0].Topic.ShouldBe("My Meeting");
			result.Records[0].Duration.ShouldBe(6);
			result.Records[0].ParticipantsCount.ShouldBe(2);
			result.Records[0].Department.ShouldBeNull();
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
			result.NextPageToken.ShouldBe("Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3");
			result.Records.Length.ShouldBe(1);
			result.Records[0].DisplayName.ShouldBe("jchill");
			result.Records[0].Email.ShouldBe("jchill@example.com");
			result.Records[0].CustomerKey.ShouldBe("349589LkJyeW");
			result.Records[0].Duration.ShouldBe(20);
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
			result.NextPageToken.ShouldBe("b43YBRLJFg3V4vsSpxvGdKIGtNbxn9h9If2");
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("2pyjK5VNQHadb2BY6Z4GbA");
			result.Records[0].Email.ShouldBe("jchill@example.com");
			result.Records[0].DisplayName.ShouldBe("Jill Chill");
			result.Records[0].Department.ShouldBe("HR");
			result.Records[0].TotalMeetings.ShouldBe(45);
			result.Records[0].TotalParticipants.ShouldBe(56);
			result.Records[0].TotalMeetingMinutes.ShouldBe(342);
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
			result.Year.ShouldBe(2022);
			result.Month.ShouldBe(3);
			result.DailyUsageSummaries.Length.ShouldBe(1);
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
			result.Year.ShouldBe(2022);
			result.Month.ShouldBe(3);
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
