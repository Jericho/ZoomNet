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
	public class SmsSessionTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public SmsSessionTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetSmsSessionDetailsAsync Tests

		[Fact]
		public async Task GetSmsSessionDetailsAsync_DefaultParameters()
		{
			// Arrange
			var sessionId = "session123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].MessageId.ShouldBe("IQ-cRH5P5EiTWCwpNzScnECJw");
			result.Records[0].Message.ShouldBe("welcome");
			result.Records[0].Direction.ShouldBe(SmsDirection.Inbound);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithDateRange()
		{
			// Arrange
			var sessionId = "session456";
			var from = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 3, 31, 23, 59, 59, DateTimeKind.Utc);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("from", "2024-03-01T00:00:00Z")
				.WithQueryString("to", "2024-03-31T23:59:59Z")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from,
				to,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_OrderDescending()
		{
			// Arrange
			var sessionId = "session789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "2")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				orderAscending: false,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_NoSorting()
		{
			// Arrange
			var sessionId = "session999";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				orderAscending: null,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_WithAllParameters()
		{
			// Arrange
			var sessionId = "fullSession";
			var from = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 3, 20, 23, 59, 59, DateTimeKind.Utc);
			var orderAscending = false;
			var recordsPerPage = 75;
			var pagingToken = "fullTestToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "2")
				.WithQueryString("from", "2024-03-15T00:00:00Z")
				.WithQueryString("to", "2024-03-20T23:59:59Z")
				.WithQueryString("page_size", "75")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from,
				to,
				orderAscending,
				recordsPerPage,
				pagingToken,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_VerifyAttachmentDetails()
		{
			// Arrange
			var sessionId = "attachmentSession";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records[0].Attachments.ShouldNotBeNull();
			result.Records[0].Attachments.Length.ShouldBe(1);
			result.Records[0].Attachments[0].Id.ShouldBe("x18dcVWxTcCzbp4zr2AT3A");
			result.Records[0].Attachments[0].Name.ShouldBe("FWDHOMaNRaqIvNc3aIdisg.jpg");
			result.Records[0].Attachments[0].Size.ShouldBe(225740);
		}

		#endregion

		#region GetSmsByMessageIdAsync Tests

		[Fact]
		public async Task GetSmsByMessageIdAsync()
		{
			// Arrange
			var sessionId = "session123";
			var messageId = "msg789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId, "messages", messageId))
				.Respond("application/json", EndpointsResource.phone_sms_sessions__sessionId__messages__messageId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsByMessageIdAsync(
				sessionId,
				messageId,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.MessageId.ShouldBe("IQ-cRH5P5EiTWCwpNzScnECJw");
			result.Message.ShouldBe("welcome");
			result.Direction.ShouldBe(SmsDirection.Inbound);
			result.Type.ShouldBe(SmsMessageType.Mms);
			result.CreatedOn.ShouldBe(new DateTime(2022, 3, 23, 2, 58, 1, DateTimeKind.Utc));
			result.Sender.ShouldNotBeNull();
			result.Sender.DisplayName.ShouldBe("ezreal mao");
			result.Sender.PhoneNumber.ShouldBe("12092693625");
			result.Recipients.ShouldNotBeNull();
			result.Recipients.Length.ShouldBe(1);
			result.Recipients[0].DisplayName.ShouldBe("test api");
			result.Recipients[0].PhoneNumber.ShouldBe("18108001001");
			result.Attachments.ShouldNotBeNull();
			result.Attachments.Length.ShouldBe(1);
			result.Attachments[0].Name.ShouldBe("FWDHOMaNRaqIvNc3aIdisg.jpg");
			result.Attachments[0].Size.ShouldBe(225740);
		}

		#endregion
	}
}
