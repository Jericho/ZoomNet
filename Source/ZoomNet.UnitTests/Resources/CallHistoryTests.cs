using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class CallHistoryTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public CallHistoryTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public async Task GetAccountCallHistoryAsync()
		{
			// Arrange
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.TotalRecords.ShouldBeGreaterThanOrEqualTo(0);
			result.Records.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallHistoryAsync_WithCallType()
		{
			// Arrange
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
			var callType = "inbound";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("type", callType)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				type: callType,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallHistoryAsync_WithPathFilter()
		{
			// Arrange
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
			var path = "inbound";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("path", path)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				path: path,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallHistoryAsync_WithTimeType()
		{
			// Arrange
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
			var timeType = "startTime";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("time_type", timeType)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				timeType: timeType,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAccountCallHistoryAsync_WithAllFilters()
		{
			// Arrange
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
			var callType = "inbound";
			var path = "inbound";
			var timeType = "startTime";
			var recordsPerPage = 100;
			var pagingToken = "token_xyz";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("type", callType)
				.WithQueryString("path", path)
				.WithQueryString("time_type", timeType)
				.WithQueryString("page_size", "100")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				type: callType,
				path: path,
				timeType: timeType,
				recordsPerPage: recordsPerPage,
				pagingToken: pagingToken,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(100);
		}

		[Fact]
		public async Task GetCallElementAsync()
		{
			// Arrange
			var callId = "call123456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history", callId))
				.Respond("application/json", EndpointsResource.phone_call_history__callId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetCallElementAsync(callId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(callId);
			result.CallerName.ShouldNotBeNullOrEmpty();
			result.CalleeName.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public async Task GetUserCallHistoryAsync()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetUserCallHistoryAsync(
				userId,
				from,
				to,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.TotalRecords.ShouldBeGreaterThanOrEqualTo(0);
			result.Records.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetUserCallHistoryAsync_WithFilters()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
			var callType = "outbound";
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("type", callType)
				.WithQueryString("page_size", "50")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetUserCallHistoryAsync(
				userId,
				from,
				to,
				type: callType,
				recordsPerPage: recordsPerPage,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(50);
		}

		[Fact]
		public async Task GetCallLogAsync()
		{
			// Arrange
			var callLogId = "log123456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs", callLogId))
				.Respond("application/json", EndpointsResource.phone_call_logs__callLogId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new CallHistory(client);

			// Act
			var result = await callHistory.GetCallLogAsync(callLogId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(callLogId);
		}
	}
}
