using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneTests
	{
		private const string RECORDING_TRANSCRIPT_JSON = @"
		{
			""type"": ""zoom_transcript"",
			""ver"": 1,
			""recording_id"": ""RECORDING_ID"",
			""meeting_id"": ""MEETING_ID"",
			""account_id"": ""ACCOUNT_ID"",
			""host_id"": ""HOST_ID"",
			""recording_start"": ""YYYY-MM-DDThh:mm:ssZ"",
			""recording_end"": ""YYYY-MM-DDThh:mm:ssZ"",
			""timeline"": [
				{
					""text"": ""TRANSCRIPTED TEXT APPEARS HERE"",
					""raw_text"": ""TRANSCRIPTED RAW TEXT APPEARS HERE"",
					""ts"": ""00:00:00.000"",
					""end_ts"": ""00:00:00.600"",
					""users"": [],
					""userId"": ""USER PHONE EXTENSION NUMBER APPEARS HERE"",
					""userIds"": [],
					""channelMark"": ""L""
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public PhoneTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetRecordingAsync Tests

		[Fact]
		public async Task GetRecordingAsync_WithValidCallId_ReturnsRecording()
		{
			// Arrange
			var callId = "call123456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs", callId, "recordings"))
				.Respond("application/json", EndpointsResource.phone_call_logs__id__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetRecordingAsync(callId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("rec123456");
			result.CallLogId.ShouldBe("log789012");
			result.CalleeName.ShouldBe("Jane Doe");
			result.CalleeNumber.ShouldBe("+15551234567");
			result.CallerName.ShouldBe("John Smith");
			result.CallerNumber.ShouldBe("+15559876543");
			result.Duration.ShouldBe(300);
		}

		#endregion

		#region GetRecordingTranscriptAsync Tests

		[Fact]
		public async Task GetRecordingTranscriptAsync_WithValidRecordingId_ReturnsTranscript()
		{
			// Arrange
			var recordingId = "rec123456";
			var redirectUrl = "http://this_is_a_test.com/transcript.txt";
			var headers = new Dictionary<string, string>()
			{
				{ "location", redirectUrl },
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "recording_transcript", "download", recordingId))
				.Respond(HttpStatusCode.Found, headers, (HttpContent)null);
			mockHttp.Expect(HttpMethod.Get, redirectUrl)
				.Respond("application/json", RECORDING_TRANSCRIPT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetRecordingTranscriptAsync(recordingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordingId.ShouldBe("rec123456");
			result.Type.ShouldBe("zoom_transcript");
			result.Version.ShouldBe(1);
		}

		#endregion

		#region GetPhoneCallUserProfileAsync Tests

		[Fact]
		public async Task GetPhoneCallUserProfileAsync_WithValidUserId_ReturnsProfile()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId))
				.Respond("application/json", EndpointsResource.phone_users__userId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetPhoneCallUserProfileAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("user123");
			result.Email.ShouldBe("john.smith@example.com");
			result.ExtensionId.ShouldBe("ext456");
			result.ExtensionNumber.ShouldBe(1001);
			result.PhoneUserId.ShouldBe("phoneuser789");
			result.Department.ShouldBe("Development");
			result.CostCenter.ShouldBe("Engineering");
		}

		#endregion

		#region ListPhoneUsersAsync Tests

		[Fact]
		public async Task ListPhoneUsersAsync_DefaultParameters_ReturnsUsers()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("user001");
			result.Records[0].Name.ShouldBe("Alice Johnson");
			result.Records[0].Email.ShouldBe("alice@example.com");
			result.Records[0].Department.ShouldBe("Sales");
			result.Records[1].Id.ShouldBe("user002");
			result.Records[1].Name.ShouldBe("Bob Williams");
		}

		[Fact]
		public async Task ListPhoneUsersAsync_WithFilters_ReturnsFilteredUsers()
		{
			// Arrange
			var siteId = "site001";
			var callingType = 1;
			var status = PhoneCallUserStatus.Active;
			var department = "Sales";
			var costCenter = "Sales Team";
			var keyword = "Alice";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.WithQueryString("page_size", "30")
				.WithQueryString("site_id", siteId)
				.WithQueryString("calling_type", callingType.ToString())
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("department", department)
				.WithQueryString("cost_center", costCenter)
				.WithQueryString("keyword", keyword)
				.Respond("application/json", EndpointsResource.phone_users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(
				siteId: siteId,
				callingType: callingType,
				status: status,
				department: department,
				costCenter: costCenter,
				keyword: keyword,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region ListPhonesAsync Tests

		[Fact]
		public async Task ListPhonesAsync_DefaultParameters_ReturnsPhones()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "numbers"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.phone_numbers_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhonesAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token456");
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task ListPhonesAsync_WithFilters_ReturnsFilteredPhones()
		{
			// Arrange
			var siteId = "site001";
			var assignmentType = PhoneNumberAssignmentType.Assigned;
			var extensionType = PhoneNumberExtensionType.User;
			var numberType = PhoneNumberType.Toll;
			var pendingNumbers = false;
			var keyword = "555";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "numbers"))
				.WithQueryString("page_size", "30")
				.WithQueryString("site_id", siteId)
				.WithQueryString("type", assignmentType.ToEnumString())
				.WithQueryString("extension_type", extensionType.ToEnumString())
				.WithQueryString("number_type", numberType.ToEnumString())
				.WithQueryString("pending_numbers", pendingNumbers.ToString())
				.WithQueryString("keyword", keyword)
				.Respond("application/json", EndpointsResource.phone_numbers_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhonesAsync(
				siteId: siteId,
				assignmentType: assignmentType,
				extensionType: extensionType,
				numberType: numberType,
				pendingNumbers: pendingNumbers,
				keyword: keyword,
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region UpdateCallHandlingSettingsAsync Tests

		[Fact]
		public async Task UpdateCallHandlingSettingsAsync_WithValidParameters_UpdatesSettings()
		{
			// Arrange
			var extensionId = "ext123";
			var settingType = CallHandlingSettingType.BusinessHours;
			var subsettings = new CallForwardingSubsettings
			{
				RequirePress1BeforeConnecting = false
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "extension", extensionId, "call_handling", "settings", settingType.ToEnumString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			await phone.UpdateCallHandlingSettingsAsync(extensionId, settingType, subsettings, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateCallHandlingSettingsAsync_WithDifferentSettingType_UpdatesSettings()
		{
			// Arrange
			var extensionId = "ext456";
			var settingType = CallHandlingSettingType.HolidayHours;
			var subsettings = new CallHandlingSubsettings
			{
				CallNotAnswerAction = CallNotAnswerActionType.ForwardToVoicemail,
				BusyOnAnotherCallAction = BusyOnAnotherCallActionType.ForwardToVoicemail
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "extension", extensionId, "call_handling", "settings", settingType.ToEnumString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			await phone.UpdateCallHandlingSettingsAsync(extensionId, settingType, subsettings, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
