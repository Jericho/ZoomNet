using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class CallLogsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public CallLogsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<AccountCallLog>(EndpointsResource.phone_call_logs__callLogId__GET, JsonFormatter.DefaultDeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("a55d94f7-27ea-4dbb-ab25-028f2c8d55bd");
			result.UserId.ShouldBeNull();
			result.CallType.ShouldBe(CallLogCallType.Voip);
			result.CallerDidNumber.ShouldBe("+12055432724");
			result.CallerCountryCode.ShouldBe("1");
			result.CallerCountryIsoCode.ShouldBe(Country.United_States_of_America);
			result.CallerNumber.ShouldBe("+16622001852");
			result.CallerNumberType.ShouldBe(CallLogCallerNumberType.Extension);
			result.CallerName.ShouldBe("Caller name");
			result.CalleeCountryCode.ShouldBe("1");
			result.CalleeCountryIsoCode.ShouldBe(Country.United_States_of_America);
			result.CalleeNumber.ShouldBe("1018");
			result.CalleeNumberType.ShouldBe(CallLogCalleeNumberType.Extension);
			result.CalleeName.ShouldBe("Callee name");
			result.CalleeNumberSource.ShouldBe(CallLogNumberSource.Internal);
			result.Direction.ShouldBe(CallLogDirection.Inbound);
			result.Duration.ShouldBe(20);
			result.Result.ShouldBe(CallLogResult.CallConnected);
			result.StartedTime.ShouldBe(new DateTime(2021, 10, 12, 22, 54, 31, DateTimeKind.Utc));
			result.Path.ShouldBe("callQueue");
			result.RecordingId.ShouldBeNull();
			result.RecordingType.ShouldBeNull();
			result.CallId.ShouldBe("7018317023722949162");
			result.Owner.ShouldBeNull();
			result.CallEndTime.ShouldBeNull();
			result.UserDepartment.ShouldBe("web-api1");
			result.UserCostCenter.ShouldBe("cost-center1");
		}

		[Fact]
		public async Task GetCallLogsForAccountAsync()
		{
			// Arrange
			var from = new DateOnly(2021, 10, 1);
			var to = new DateOnly(2021, 10, 12);
			var recordsPerPage = 5;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(from, to, CallLogType.All, null, CallLogTimeType.StartTime, null, false, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldNotBeNullOrEmpty();
			result.MoreRecordsAvailable.ShouldBeTrue();
			result.TotalRecords.ShouldBe(54);
			result.From.ShouldBe(from);
			result.To.ShouldBe(to);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		#region User Call Logs Tests

		[Fact]
		public async Task GetUserCallLogsAsync_WithMinimalParameters()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.ShouldNotBeEmpty();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithDateRange()
		{
			// Arrange
			var userId = "user123";
			var from = new DateOnly(2023, 12, 1);
			var to = new DateOnly(2023, 12, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("from", "2023-12-01")
				.WithQueryString("to", "2023-12-31")
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithCallType()
		{
			// Arrange
			var userId = "user123";
			var type = CallLogType.Missed;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "missed")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, type: type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithPhoneNumber()
		{
			// Arrange
			var userId = "user123";
			var phoneNumber = "+1234567890";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("phone_number", phoneNumber)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, phoneNumber: phoneNumber, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithPagination()
		{
			// Arrange
			var userId = "user123";
			var recordsPerPage = 50;
			var pagingToken = "token123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "50")
				.WithQueryString("next_page_token", "token123")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithAllParameters()
		{
			// Arrange
			var userId = "user123";
			var from = new DateOnly(2023, 11, 1);
			var to = new DateOnly(2023, 11, 30);
			var type = CallLogType.Missed;
			var phoneNumber = "+9876543210";
			var recordsPerPage = 100;
			var pagingToken = "full_token";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("from", "2023-11-01")
				.WithQueryString("to", "2023-11-30")
				.WithQueryString("type", "missed")
				.WithQueryString("phone_number", phoneNumber)
				.WithQueryString("page_size", "100")
				.WithQueryString("next_page_token", "full_token")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, from, to, type, phoneNumber, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithDifferentCallTypes()
		{
			// Arrange
			var userId = "user123";
			var type = CallLogType.All;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, type: type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallLogsAsync_MissedType()
		{
			// Arrange
			var userId = "user123";
			var type = CallLogType.Missed;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "missed")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, type: type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Account Call Logs Tests

		[Fact]
		public async Task GetAccountCallLogsAsync_WithMinimalParameters()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("timeType", "startTime")
				.WithQueryString("charged_call_logs", false.ToString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithPathType()
		{
			// Arrange
			var pathType = CallLogPathType.Pstn;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("path", "pstn")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(pathType: pathType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithTimeType()
		{
			// Arrange
			var timeType = CallLogTimeType.EndTime;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("timeType", "endTime")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(timeType: timeType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithSiteId()
		{
			// Arrange
			var siteId = "site123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("site_id", siteId)
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(siteId: siteId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithChargedCallLogs()
		{
			// Arrange
			var chargedCallLogs = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("charged_call_logs", true.ToString())
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(chargedCallLogs: chargedCallLogs, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithAllParameters()
		{
			// Arrange
			var from = new DateOnly(2023, 10, 1);
			var to = new DateOnly(2023, 10, 31);
			var type = CallLogType.Missed;
			var pathType = CallLogPathType.Extension;
			var timeType = CallLogTimeType.EndTime;
			var siteId = "site456";
			var chargedCallLogs = true;
			var recordsPerPage = 75;
			var pagingToken = "complete_token";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("from", "2023-10-01")
				.WithQueryString("to", "2023-10-31")
				.WithQueryString("type", "missed")
				.WithQueryString("path", "extension")
				.WithQueryString("timeType", "endTime")
				.WithQueryString("site_id", "site456")
				.WithQueryString("charged_call_logs", true.ToString())
				.WithQueryString("page_size", "75")
				.WithQueryString("next_page_token", "complete_token")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(from, to, type, pathType, timeType, siteId, chargedCallLogs, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithDifferentPathTypes()
		{
			// Arrange
			var pathType = CallLogPathType.AutoReceptionist;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("path", "autoReceptionist")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(pathType: pathType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithCallQueuePathType()
		{
			// Arrange
			var pathType = CallLogPathType.CallQueue;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("path", "callQueue")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(pathType: pathType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithCustomPageSize()
		{
			// Arrange
			var recordsPerPage = 200;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("page_size", "200")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithPagingToken()
		{
			// Arrange
			var pagingToken = "next_page_xyz";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithOnlyFromDate()
		{
			// Arrange
			var from = new DateOnly(2023, 9, 1);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("from", "2023-09-01")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(from: from, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallLogsAsync_WithOnlyToDate()
		{
			// Arrange
			var to = new DateOnly(2023, 9, 30);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("to", "2023-09-30")
				.Respond("application/json", EndpointsResource.phone_call_logs_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(to: to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
