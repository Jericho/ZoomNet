using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
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
			var from = new DateOnly(2024, 1, 1);
			var to = new DateOnly(2024, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

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
			var from = new DateOnly(2024, 1, 1);
			var to = new DateOnly(2024, 1, 31);
			var callType = CallElementCallType.General;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("call_types", callType.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				callTypes: [callType],
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
			var from = new DateOnly(2024, 1, 1);
			var to = new DateOnly(2024, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
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
			var from = new DateOnly(2024, 1, 1);
			var to = new DateOnly(2024, 1, 31);
			var callType = CallElementCallType.Emergency;
			var timeType = CallLogTimeType.StartTime;
			var recordsPerPage = 100;
			var pagingToken = "token_xyz";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("call_types", callType.ToEnumString())
				.WithQueryString("time_type", timeType.ToEnumString())
				.WithQueryString("page_size", "100")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.phone_call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

			// Act
			var result = await callHistory.GetAccountCallHistoryAsync(
				from,
				to,
				callTypes: [callType],
				timeType: timeType,
				recordsPerPage: recordsPerPage,
				pagingToken: pagingToken,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
		}

		[Fact]
		public async Task GetCallElementAsync()
		{
			// Arrange
			var callElementId = "20211008-fe9c3900-5187-4254-9359-590afbc40bc9";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_element", callElementId))
				.Respond("application/json", EndpointsResource.phone_call_element__callElementId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

			// Act
			var result = await callHistory.GetCallElementAsync(callElementId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.CallElementId.ShouldBe(callElementId);
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
			var callHistory = new ZoomNet.Resources.CallHistory(client);

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
			var callType = CallElementCallType.General;
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_history"))
				.WithQueryString("from", from.ToString("yyyy-MM-dd"))
				.WithQueryString("to", to.ToString("yyyy-MM-dd"))
				.WithQueryString("call_types", callType.ToEnumString())
				.WithQueryString("page_size", "50")
				.Respond("application/json", EndpointsResource.phone_users__userId__call_history_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var callHistory = new ZoomNet.Resources.CallHistory(client);

			// Act
			var result = await callHistory.GetUserCallHistoryAsync(
				userId,
				from,
				to,
				callTypes: [callType],
				recordsPerPage: recordsPerPage,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
		}
	}
}
