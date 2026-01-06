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
	public class SmsSessionTests
	{
		private const string SMS_SESSION_DETAILS_JSON = @"{
			""next_page_token"": ""2z0Ov7kngllAbfQi4ZR2eQTb3mFVYQpYAe4"",
			""page_size"": 30,
			""sms_histories"": [
				{
					""attachments"": [
						{
							""download_url"": ""https://exampleurl.us/file/download/x18dcVWxTcCzbp4zr2AT3A?jwt=eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjoiYTZkODE4NzQ2MDNmN2UzZWM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oto"",
							""id"": ""x18dcVWxTcCzbp4zr2AT3A"",
							""name"": ""FWDHOMaNRaqIvNc3aIdisg.jpg"",
							""size"": 225740,
							""type"": ""JPG/JPEG""
						}
					],
					""date_time"": ""2022-03-23T02:58:01Z"",
					""direction"": ""In"",
					""message"": ""welcome"",
					""message_id"": ""IQ-cRH5P5EiTWCwpNzScnECJw"",
					""message_type"": 2,
					""sender"": {
						""display_name"": ""test api"",
						""owner"": {
							""id"": ""DnEopNmXQEGU2uvvzjgojw"",
							""type"": ""user""
						},
						""phone_number"": ""18108001001""
					},
					""to_members"": [
						{
							""display_name"": ""ezreal mao"",
							""owner"": {
							""id"": ""WeD59Hn7SvqNRB9jcxz5NQ"",
							""type"": ""user""
							},
							""phone_number"": ""12092693625""
						}
					]
				}
			]
		}";

		private const string SINGLE_SMS_MESSAGE_JSON = @"{
			""attachments"": [
				{
					""download_url"": ""https://exampleurl.us/file/download/x18dcVWxTcCzbp4zr2AT3A?jwt=eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjoiYTZkODE4NzQ2MDNmN2UzZWM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oto"",
					""id"": ""x18dcVWxTcCzbp4zr2AT3A"",
					""name"": ""FWDHOMaNRaqIvNc3aIdisg.jpg"",
					""size"": 225740,
					""type"": ""JPG/JPEG""
				}
			],
			""date_time"": ""2022-03-23T02:58:01Z"",
			""direction"": ""In"",
			""message"": ""welcome"",
			""message_id"": ""IQ-cRH5P5EiTWCwpNzScnECJw"",
			""message_type"": 2,
			""sender"": {
				""display_name"": ""ezreal mao"",
				""owner"": {
					""id"": ""WeD59Hn7SvqNRB9jcxz5NQ"",
					""type"": ""user""
				},
				""phone_number"": ""12092693625""
			},
			""to_members"": [
				{
					""display_name"": ""test api"",
					""owner"": {
					""id"": ""DnEopNmXQEGU2uvvzjgojw"",
					""type"": ""user""
					},
					""phone_number"": ""18108001001""
				}
			]
		}";

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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
		public async Task GetSmsSessionDetailsAsync_WithPagination()
		{
			// Arrange
			var sessionId = "session123";
			var recordsPerPage = 50;
			var pagingToken = "customToken123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				recordsPerPage: recordsPerPage,
				pagingToken: pagingToken,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_MaxRecordsPerPage()
		{
			// Arrange
			var sessionId = "session456";
			var recordsPerPage = 100;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", "100")
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms.GetSmsSessionDetailsAsync(
				sessionId,
				from: null,
				to: null,
				recordsPerPage: recordsPerPage,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_EmptyResults()
		{
			// Arrange
			var sessionId = "emptySession";
			var emptyJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""sms_histories"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyJson);

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
			result.Records.Length.ShouldBe(0);
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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
		public async Task GetSmsSessionDetailsAsync_LongSessionId()
		{
			// Arrange
			var sessionId = "aa74aeea445022779e375d2de1e193c5c131e7347283e3a6f308637cdd2a6832dc2cd7d4694c4a3316b010048373ad14174d6a8e0c548860dc2f3586e0de7fde8d2e097badf450605424521e2523139ddc2cd7d4694c4a3316b010048373ad141305076fc94fe1a4c8c52f47d6c3dcc72c74aeea445022779e375d2de1e193c5";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("page_size", "30")
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
		}

		[Fact]
		public async Task GetSmsSessionDetailsAsync_DateRangeAtBoundary()
		{
			// Arrange
			var sessionId = "boundarySession";
			var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.WithQueryString("sort", "1")
				.WithQueryString("from", "2024-01-01T00:00:00Z")
				.WithQueryString("to", "2024-01-31T23:59:59Z")
				.WithQueryString("page_size", "30")
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
				.Respond("application/json", SMS_SESSION_DETAILS_JSON);

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
				.Respond("application/json", SINGLE_SMS_MESSAGE_JSON);

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

		[Fact]
		public async Task GetSmsByMessageIdAsync_DifferentMessage()
		{
			// Arrange
			var sessionId = "session456";
			var messageId = "differentMsg123";
			var customMessageJson = @"{
				""date_time"": ""2024-03-25T14:20:00Z"",
				""direction"": ""Out"",
				""message"": ""Custom test message"",
				""message_id"": ""differentMsg123"",
				""message_type"": 1,
				""sender"": {
					""display_name"": ""Test Sender"",
					""owner"": {
						""id"": ""sender123"",
						""type"": ""user""
					},
					""phone_number"": ""18005551234""
				},
				""to_members"": [
					{
						""display_name"": ""Test Recipient"",
						""owner"": {
							""id"": ""recipient456"",
							""type"": ""user""
						},
						""phone_number"": ""18005559876""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId, "messages", messageId))
				.Respond("application/json", customMessageJson);

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
			result.MessageId.ShouldBe("differentMsg123");
			result.Message.ShouldBe("Custom test message");
			result.Direction.ShouldBe(SmsDirection.Outbound);
			result.Type.ShouldBe(SmsMessageType.Sms);
		}

		[Fact]
		public async Task GetSmsByMessageIdAsync_WithoutAttachments()
		{
			// Arrange
			var sessionId = "session789";
			var messageId = "msgNoAttachment";
			var noAttachmentJson = @"{
				""date_time"": ""2024-03-26T09:00:00Z"",
				""direction"": ""In"",
				""message"": ""Simple text message"",
				""message_id"": ""msgNoAttachment"",
				""message_type"": 1,
				""sender"": {
					""display_name"": ""Sender Name"",
					""owner"": {
						""id"": ""user111"",
						""type"": ""user""
					},
					""phone_number"": ""15551112222""
				},
				""to_members"": [
					{
						""display_name"": ""Recipient Name"",
						""owner"": {
							""id"": ""user222"",
							""type"": ""user""
						},
						""phone_number"": ""15553334444""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId, "messages", messageId))
				.Respond("application/json", noAttachmentJson);

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
			result.MessageId.ShouldBe("msgNoAttachment");
			result.Message.ShouldBe("Simple text message");
		}

		[Fact]
		public async Task GetSmsByMessageIdAsync_LongIds()
		{
			// Arrange
			var sessionId = "verylongsessionid1234567890abcdefghijklmnopqrstuvwxyz1234567890";
			var messageId = "verylongmessageid9876543210zyxwvutsrqponmlkjihgfedcba0987654321";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId, "messages", messageId))
				.Respond("application/json", SINGLE_SMS_MESSAGE_JSON);

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
		}

		[Fact]
		public async Task GetSmsByMessageIdAsync_GroupMessage()
		{
			// Arrange
			var sessionId = "groupSession";
			var messageId = "groupMsg123";
			var groupMessageJson = @"{
				""date_time"": ""2024-03-27T11:45:00Z"",
				""direction"": ""Out"",
				""message"": ""Group announcement"",
				""message_id"": ""groupMsg123"",
				""message_type"": 3,
				""sender"": {
					""display_name"": ""Group Admin"",
					""owner"": {
						""id"": ""admin123"",
						""type"": ""user""
					},
					""phone_number"": ""18001234567""
				},
				""to_members"": [
					{
						""display_name"": ""Member One"",
						""owner"": {
							""id"": ""member1"",
							""type"": ""user""
						},
						""phone_number"": ""15551111111""
					},
					{
						""display_name"": ""Member Two"",
						""owner"": {
							""id"": ""member2"",
							""type"": ""user""
						},
						""phone_number"": ""15552222222""
					},
					{
						""display_name"": ""Member Three"",
						""owner"": {
							""id"": ""member3"",
							""type"": ""user""
						},
						""phone_number"": ""15553333333""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone/sms/sessions", sessionId, "messages", messageId))
				.Respond("application/json", groupMessageJson);

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
			result.MessageId.ShouldBe("groupMsg123");
			result.Type.ShouldBe(SmsMessageType.GroupSms);
			result.Recipients.Length.ShouldBe(3);
			result.Recipients[0].DisplayName.ShouldBe("Member One");
			result.Recipients[1].DisplayName.ShouldBe("Member Two");
			result.Recipients[2].DisplayName.ShouldBe("Member Three");
		}

		#endregion
	}
}
