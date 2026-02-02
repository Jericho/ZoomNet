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
	public class SmsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public SmsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetSmsSessionDetailsAsync Tests

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithValidSessionId_ReturnsSmsMessages()
		{
			// Arrange
			var sessionId = "session123";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId))
				.WithQueryString("page_size", "30")
				.WithQueryString("sort", "1")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(sessionId, null, null, true, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].MessageId.ShouldBe("msg001");
			result.Records[0].Direction.ShouldBe(SmsDirection.Inbound);
			result.Records[0].Sender.PhoneNumber.ShouldBe("+12223334444");
			result.Records[1].MessageId.ShouldBe("msg002");
			result.Records[1].Direction.ShouldBe(SmsDirection.Outbound);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithDateRange_ReturnsSmsMessages()
		{
			// Arrange
			var sessionId = "session123";
			var from = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2023, 7, 31, 23, 59, 59, DateTimeKind.Utc);
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId))
				.WithQueryString("from", "2023-07-01T00:00:00Z")
				.WithQueryString("to", "2023-07-31T23:59:59Z")
				.WithQueryString("page_size", "30")
				.WithQueryString("sort", "1")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(sessionId, from, to, true, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithDescendingOrder_ReturnsSmsMessages()
		{
			// Arrange
			var sessionId = "session123";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId))
				.WithQueryString("page_size", "30")
				.WithQueryString("sort", "2")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(sessionId, null, null, false, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithNullSortOrder_ReturnsSmsMessages()
		{
			// Arrange
			var sessionId = "session123";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(sessionId, null, null, null, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithPagingToken_ReturnsSmsMessages()
		{
			// Arrange
			var sessionId = "session123";
			var recordsPerPage = 30;
			var pagingToken = "nextPageToken123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId))
				.WithQueryString("page_size", "30")
				.WithQueryString("next_page_token", pagingToken)
				.WithQueryString("sort", "1")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(sessionId, null, null, true, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		#endregion

		#region GetSmsByMessageIdAsync Tests

		[Fact]
		public async Task GetSmsByMessageIdAsync_WithValidIds_ReturnsMessage()
		{
			// Arrange
			var sessionId = "session123";
			var messageId = "msg001";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "sms", "sessions", sessionId, "messages", messageId))
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__messages__messageId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsByMessageIdAsync(sessionId, messageId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.MessageId.ShouldBe("msg001");
			result.Direction.ShouldBe(SmsDirection.Inbound);
			result.Sender.ShouldNotBeNull();
			result.Sender.PhoneNumber.ShouldBe("+12223334444");
			result.Sender.DisplayName.ShouldBe("John Doe");
			result.Recipients.ShouldNotBeNull();
			result.Recipients.Length.ShouldBe(1);
			result.Recipients[0].PhoneNumber.ShouldBe("+15556667777");
			result.Recipients[0].DisplayName.ShouldBe("Jane Smith");
		}

		#endregion
	}
}
