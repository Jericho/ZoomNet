using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneTests
	{
		private const string PHONE_CALL_RECORDING_JSON = @"{
			""id"": ""rec123456"",
			""call_log_id"": ""log789012"",
			""callee_name"": ""Jane Doe"",
			""callee_number"": ""+15551234567"",
			""callee_number_type"": 1,
			""caller_name"": ""John Smith"",
			""caller_number"": ""+15559876543"",
			""caller_number_type"": 1,
			""date_time"": ""2023-06-01T10:00:00Z"",
			""direction"": ""inbound"",
			""download_url"": ""https://zoom.us/rec/download/rec123456"",
			""transcript_download_url"": ""https://zoom.us/rec/transcript/rec123456"",
			""duration"": 300,
			""end_time"": ""2023-06-01T10:05:00Z"",
			""recording_type"": ""automatic""
		}";

		private const string PHONE_CALL_RECORDING_TRANSCRIPT_JSON = @"{
			""type"": ""zoom_transcript"",
			""ver"": 1,
			""recording_id"": ""rec123456"",
			""meeting_id"": ""meeting123"",
			""account_id"": ""account456"",
			""host_id"": ""host789"",
			""recording_start"": ""2023-06-01T10:00:00Z"",
			""recording_end"": ""2023-06-01T10:05:00Z"",
			""timeline"": []
		}";

		private const string PHONE_CALL_USER_PROFILE_JSON = @"{
			""id"": ""user123"",
			""email"": ""john.smith@example.com"",
			""extension_id"": ""ext456"",
			""extension_number"": 1001,
			""phone_user_id"": ""phoneuser789"",
			""site_id"": ""site001"",
			""status"": ""activate"",
			""cost_center"": ""Engineering"",
			""department"": ""Development"",
			""calling_plans"": [
				{
					""type"": 100,
					""name"": ""US Calling Plan""
				}
			]
		}";

		private const string PHONE_USERS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""users"": [
				{
					""id"": ""user001"",
					""name"": ""Alice Johnson"",
					""email"": ""alice@example.com"",
					""extension_id"": ""ext001"",
					""extension_number"": 2001,
					""phone_user_id"": ""phoneuser001"",
					""status"": ""activate"",
					""department"": ""Sales"",
					""cost_center"": ""Sales Team""
				},
				{
					""id"": ""user002"",
					""name"": ""Bob Williams"",
					""email"": ""bob@example.com"",
					""extension_id"": ""ext002"",
					""extension_number"": 2002,
					""phone_user_id"": ""phoneuser002"",
					""status"": ""activate"",
					""department"": ""Marketing"",
					""cost_center"": ""Marketing Team""
				}
			]
		}";

		private const string PHONE_NUMBERS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token456"",
			""phone_numbers"": [
				{
					""id"": ""number001"",
					""number"": ""+15551111111"",
					""display_number"": ""(555) 111-1111"",
					""type"": ""toll"",
					""assignee"": {
						""id"": ""user001"",
						""name"": ""Alice Johnson"",
						""extension_number"": 2001
					}
				},
				{
					""id"": ""number002"",
					""number"": ""+15552222222"",
					""display_number"": ""(555) 222-2222"",
					""type"": ""toll_free"",
					""assignee"": {
						""id"": ""user002"",
						""name"": ""Bob Williams"",
						""extension_number"": 2002
					}
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
				.Respond("application/json", PHONE_CALL_RECORDING_JSON);

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

		[Fact]
		public async Task GetRecordingAsync_WithDifferentCallId_ReturnsCorrectRecording()
		{
			// Arrange
			var callId = "call789012";
			var differentRecordingJson = @"{
				""id"": ""rec789012"",
				""call_log_id"": ""log456789"",
				""callee_name"": ""Test User"",
				""callee_number"": ""+15550000000"",
				""caller_name"": ""Test Caller"",
				""caller_number"": ""+15551111111"",
				""date_time"": ""2023-07-01T14:00:00Z"",
				""direction"": ""outbound"",
				""duration"": 180,
				""end_time"": ""2023-07-01T14:03:00Z""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs", callId, "recordings"))
				.Respond("application/json", differentRecordingJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetRecordingAsync(callId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("rec789012");
			result.CallLogId.ShouldBe("log456789");
			result.Duration.ShouldBe(180);
		}

		#endregion

		#region GetRecordingTranscriptAsync Tests

		[Fact]
		public async Task GetRecordingTranscriptAsync_WithValidRecordingId_ReturnsTranscript()
		{
			// Arrange
			var recordingId = "rec123456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "recording_transcript", "download", recordingId))
				.Respond("application/json", PHONE_CALL_RECORDING_TRANSCRIPT_JSON);

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

		[Fact]
		public async Task GetRecordingTranscriptAsync_WithDifferentRecordingId_ReturnsCorrectTranscript()
		{
			// Arrange
			var recordingId = "rec789012";
			var differentTranscriptJson = @"{
				""type"": ""zoom_transcript"",
				""ver"": 2,
				""recording_id"": ""rec789012"",
				""meeting_id"": ""meeting456"",
				""account_id"": ""account789"",
				""host_id"": ""host012"",
				""recording_start"": ""2023-07-01T14:00:00Z"",
				""recording_end"": ""2023-07-01T14:03:00Z"",
				""timeline"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "recording_transcript", "download", recordingId))
				.Respond("application/json", differentTranscriptJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetRecordingTranscriptAsync(recordingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordingId.ShouldBe("rec789012");
			result.Version.ShouldBe(2);
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
				.Respond("application/json", PHONE_CALL_USER_PROFILE_JSON);

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

		[Fact]
		public async Task GetPhoneCallUserProfileAsync_WithDifferentUserId_ReturnsCorrectProfile()
		{
			// Arrange
			var userId = "user456";
			var differentProfileJson = @"{
				""id"": ""user456"",
				""email"": ""jane.doe@example.com"",
				""extension_id"": ""ext789"",
				""extension_number"": 2001,
				""phone_user_id"": ""phoneuser456"",
				""site_id"": ""site002"",
				""status"": ""activate"",
				""cost_center"": ""Marketing"",
				""department"": ""Sales""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId))
				.Respond("application/json", differentProfileJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetPhoneCallUserProfileAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("user456");
			result.Email.ShouldBe("jane.doe@example.com");
			result.ExtensionNumber.ShouldBe(2001);
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
				.Respond("application/json", PHONE_USERS_JSON);

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
		public async Task ListPhoneUsersAsync_WithPagination_ReturnsUsers()
		{
			// Arrange
			var recordsPerPage = 10;
			var nextPageToken = "customToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", nextPageToken)
				.Respond("application/json", PHONE_USERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(recordsPerPage, nextPageToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
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
				.Respond("application/json", PHONE_USERS_JSON);

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

		[Fact]
		public async Task ListPhoneUsersAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(async () =>
			{
				await phone.ListPhoneUsersAsync(recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		[Fact]
		public async Task ListPhoneUsersAsync_EmptyUsers_ReturnsEmptyArray()
		{
			// Arrange
			var emptyUsersJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""users"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyUsersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
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
				.Respond("application/json", PHONE_NUMBERS_JSON);

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
		public async Task ListPhonesAsync_WithPagination_ReturnsPhones()
		{
			// Arrange
			var recordsPerPage = 20;
			var nextPageToken = "phoneToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "numbers"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", nextPageToken)
				.Respond("application/json", PHONE_NUMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhonesAsync(recordsPerPage, nextPageToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
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
				.Respond("application/json", PHONE_NUMBERS_JSON);

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

		[Fact]
		public async Task ListPhonesAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(async () =>
			{
				await phone.ListPhonesAsync(recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		[Fact]
		public async Task ListPhonesAsync_EmptyPhones_ReturnsEmptyArray()
		{
			// Arrange
			var emptyPhonesJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""phone_numbers"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "numbers"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyPhonesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhonesAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
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

		#region Edge Case Tests

		[Fact]
		public async Task ListPhoneUsersAsync_MaxRecordsPerPage_ReturnsUsers()
		{
			// Arrange
			var recordsPerPage = 100;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", PHONE_USERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task ListPhonesAsync_MaxRecordsPerPage_ReturnsPhones()
		{
			// Arrange
			var recordsPerPage = 100;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "numbers"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", PHONE_NUMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhonesAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingAsync_WithSpecialCharactersInCallId_WorksCorrectly()
		{
			// Arrange
			var callId = "call/123+456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs", callId, "recordings"))
				.Respond("application/json", PHONE_CALL_RECORDING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.GetRecordingAsync(callId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
