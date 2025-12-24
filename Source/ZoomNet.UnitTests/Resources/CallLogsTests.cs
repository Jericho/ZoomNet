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

namespace ZoomNet.UnitTests.Resources
{
	public class CallLogsTests
	{
		private const string SINGLE_ACCOUNT_CALL_LOGS_JSON = @"
    {
      ""id"": ""c82112cd-0916-412c-8d2d-4620c93edfa6"",
      ""user_id"": ""1LzxYZ4WUsVp1zn7hsT6B"",
      ""call_type"": ""pstn"",
      ""caller_number"": ""1100"",
      ""caller_number_type"": 1,
      ""caller_name"": ""Main Auto Receptionist"",
      ""callee_number"": ""+17506977821"",
      ""callee_number_type"": 2,
      ""callee_number_source"": ""external"",
      ""callee_location"": ""Texas"",
      ""direction"": ""outbound"",
      ""duration"": 3,
      ""result"": ""Auto Recorded"",
      ""date_time"": ""2023-12-06T03:10:06Z"",
      ""path"": ""pstn"",
      ""recording_id"": ""0b24374d5d687899a021b28334b5e6a7"",
      ""recording_type"": ""Automatic"",
      ""has_voicemail"": false,
      ""call_id"": ""7102333465063798289"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-EoDdR-eey3q9_wdGMheR"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_did_number"": ""+17506476849"",
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""call_end_time"": ""2023-12-06T03:10:24Z"",
      ""department"": """",
      ""cost_center"": """"
    }
";

		private const string MULTIPLE_ACCOUNT_CALL_LOGS_JSON = @"
{
  ""next_page_token"": ""drhhjtYGfgrg56456456"",
  ""page_size"": 5,
  ""total_records"": 95,
  ""from"": ""2023-12-05"",
  ""to"": ""2023-12-06"",
  ""call_logs"": [
    {
      ""id"": ""c82112cd-0916-412c-8d2d-4620c93edfa6"",
      ""user_id"": ""1LzxYZ4WUsVp1zn7hsT6B"",
      ""call_type"": ""pstn"",
      ""caller_number"": ""1100"",
      ""caller_number_type"": 1,
      ""caller_name"": ""Main Auto Receptionist"",
      ""callee_number"": ""+17506977821"",
      ""callee_number_type"": 2,
      ""callee_number_source"": ""external"",
      ""callee_location"": ""Texas"",
      ""direction"": ""outbound"",
      ""duration"": 3,
      ""result"": ""Auto Recorded"",
      ""date_time"": ""2023-12-06T03:10:06Z"",
      ""path"": ""pstn"",
      ""recording_id"": ""0b24374d5d687899a021b28334b5e6a7"",
      ""recording_type"": ""Automatic"",
      ""has_voicemail"": false,
      ""call_id"": ""7102333465063798289"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-EoDdR-eey3q9_wdGMheR"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_did_number"": ""+17506476849"",
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""call_end_time"": ""2023-12-06T03:10:24Z"",
      ""department"": """",
      ""cost_center"": """"
    },
    {
      ""id"": ""b3d4f865-24d8-4f36-afd3-3492c09c69ed"",
      ""user_id"": ""4Lm9W8KHooPnQrt6Y4RuF"",
      ""call_type"": ""pstn"",
      ""caller_number"": ""1100"",
      ""caller_number_type"": 1,
      ""caller_name"": ""Main Auto Receptionist"",
      ""callee_number"": ""+15162788092"",
      ""callee_number_type"": 2,
      ""callee_number_source"": ""external"",
      ""callee_location"": ""North Carolina"",
      ""direction"": ""outbound"",
      ""duration"": 15,
      ""result"": ""Auto Recorded"",
      ""date_time"": ""2023-12-06T00:47:43Z"",
      ""path"": ""pstn"",
      ""recording_id"": ""963526f778894a6bbcfd2e6f5304f556"",
      ""recording_type"": ""Automatic"",
      ""has_voicemail"": false,
      ""call_id"": ""7309475677182123450"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-RtfghR-Rdg569_PDefGLhA"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_did_number"": ""+175004345645"",
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""call_end_time"": ""2023-12-06T00:48:05Z"",
      ""department"": """",
      ""cost_center"": """"
    },
    {
      ""id"": ""a7c1480c-7da1-4f7b-9913-10448ab4e993"",
      ""call_type"": ""voip"",
      ""caller_number"": ""+17566678189"",
      ""caller_number_type"": 2,
      ""caller_number_source"": ""external"",
      ""caller_name"": ""WAL MART"",
      ""callee_number"": ""1100"",
      ""callee_number_type"": 1,
      ""callee_name"": ""Main Auto Receptionist"",
      ""direction"": ""inbound"",
      ""duration"": 103,
      ""result"": ""Call connected"",
      ""date_time"": ""2023-12-06T00:44:44Z"",
      ""path"": ""autoReceptionist"",
      ""has_recording"": false,
      ""has_voicemail"": false,
      ""call_id"": ""7405266708095066735"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-WodfT-rTHghH_TfgRteA"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_did_number"": ""+175056644545"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""answer_start_time"": ""2023-12-06T00:44:44Z"",
      ""department"": """",
      ""cost_center"": """"
    },
    {
      ""id"": ""53a60306-ab5b-4c5e-b2c3-a1da5a47608e"",
      ""user_id"": ""vD9AUZ7rtx9Ueqa6F3Kuq"",
      ""call_type"": ""pstn"",
      ""caller_number"": ""1100"",
      ""caller_number_type"": 1,
      ""caller_name"": ""Main Auto Receptionist"",
      ""callee_number"": ""+184567434534"",
      ""callee_number_type"": 2,
      ""callee_number_source"": ""external"",
      ""callee_location"": ""United States"",
      ""direction"": ""outbound"",
      ""duration"": 24,
      ""result"": ""Auto Recorded"",
      ""date_time"": ""2023-12-06T00:44:13Z"",
      ""path"": ""pstn"",
      ""recording_id"": ""1d224334a596477788d87849f9f999c"",
      ""recording_type"": ""Automatic"",
      ""has_voicemail"": false,
      ""call_id"": ""7392374044525562788"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-RregY-DfgGfg_ERdfg"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_did_number"": ""+1704556777"",
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""call_end_time"": ""2023-12-06T00:44:38Z"",
      ""department"": """",
      ""cost_center"": """"
    },
    {
      ""id"": ""f64e97b9-b234-487e-8c12-299e7f664922"",
      ""user_id"": ""1C6XSI2pjAblbUsJkSAmy"",
      ""call_type"": ""pstn"",
      ""caller_number"": ""1100"",
      ""caller_number_type"": 1,
      ""caller_name"": ""Main Auto Receptionist"",
      ""callee_number"": ""+145665788674"",
      ""callee_number_type"": 2,
      ""callee_number_source"": ""external"",
      ""callee_location"": ""United States"",
      ""direction"": ""outbound"",
      ""duration"": 36,
      ""result"": ""Auto Recorded"",
      ""date_time"": ""2023-12-06T00:43:26Z"",
      ""path"": ""pstn"",
      ""recording_id"": ""d3456767517fh556576554456565656909"",
      ""recording_type"": ""Automatic"",
      ""has_voicemail"": false,
      ""call_id"": ""73455748542158465463"",
      ""owner"": {
        ""type"": ""autoReceptionist"",
        ""id"": ""o-Edcfr-RfFdfG_SDfdfA"",
        ""name"": ""Main Auto Receptionist"",
        ""extension_number"": 1100
      },
      ""caller_did_number"": ""+17045644435"",
      ""caller_country_code"": ""1"",
      ""caller_country_iso_code"": ""US"",
      ""callee_country_code"": ""1"",
      ""callee_country_iso_code"": ""US"",
      ""call_end_time"": ""2023-12-06T00:44:04Z"",
      ""department"": """",
      ""cost_center"": """"
    }
  ]
}
";

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
			var result = JsonSerializer.Deserialize<AccountCallLog>(SINGLE_ACCOUNT_CALL_LOGS_JSON, JsonFormatter.DefaultDeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("c82112cd-0916-412c-8d2d-4620c93edfa6");
			result.UserId.ShouldBe("1LzxYZ4WUsVp1zn7hsT6B");
			result.CallType.ShouldBe(CallLogCallType.Pstn);
			result.CallerNumber.ShouldBe("1100");
			result.CallerNumberType.ShouldBe(CallLogCallerNumberType.Extension);
			result.CallerName.ShouldBe("Main Auto Receptionist");
			result.CalleeNumber.ShouldBe("+17506977821");
			result.CalleeNumberType.ShouldBe(CallLogCalleeNumberType.Phone);
			result.CalleeNumberSource.ShouldBe(CallLogNumberSource.External);
			result.Direction.ShouldBe(CallLogDirection.Outbound);
			result.Duration.ShouldBe(3);
			result.Result.ShouldBe(CallLogResult.AutoRecorded);
			result.StartedTime.ShouldBe(new DateTime(2023, 12, 6, 3, 10, 6, DateTimeKind.Utc));// DateTime.Parse("2023-12-06T03:10:06Z"));
			result.Path.ShouldBe("pstn");
			result.RecordingId.ShouldBe("0b24374d5d687899a021b28334b5e6a7");
			result.RecordingType.ShouldBe(PhoneCallRecordingType.Automatic);
			result.CallId.ShouldBe("7102333465063798289");
			result.Owner.ShouldNotBeNull();
			result.Owner.Type.ShouldBe(CallLogOwnerType.AutoReceptionist);
			result.Owner.Id.ShouldBe("o-EoDdR-eey3q9_wdGMheR");
			result.Owner.Name.ShouldBe("Main Auto Receptionist");
			result.Owner.ExtensionNumber.ShouldBe(1100);
			result.CallerDidNumber.ShouldBe("+17506476849");
			result.CallerCountryCode.ShouldBe("1");
			result.CallerCountryIsoCode.ShouldBe(Country.United_States_of_America);
			result.CalleeCountryCode.ShouldBe("1");
			result.CalleeCountryIsoCode.ShouldBe(Country.United_States_of_America);
			result.CallEndTime.ShouldBe(new DateTime(2023, 12, 6, 3, 10, 24, DateTimeKind.Utc));// DateTime.Parse("2023-12-06T03:10:24Z"));
			result.UserDepartment.ShouldBe("");
			result.UserCostCenter.ShouldBe("");
		}

		[Fact]
		public async Task GetCallLogsForAccountAsync()
		{
			// Arrange
			var from = new DateTime(2023, 12, 5, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2023, 12, 6, 0, 0, 0, DateTimeKind.Utc);
			var recordsPerPage = 5;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs")).Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(from, to, CallLogType.All, null, CallLogTimeType.StartTime, null, false, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(recordsPerPage);
			result.NextPageToken.ShouldNotBeNullOrEmpty();
			result.MoreRecordsAvailable.ShouldBeTrue();
			result.TotalRecords.ShouldBe(95);
			result.From.ShouldBe(from);
			result.To.ShouldBe(to);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(5);
		}

		#region User Call Logs Tests

		[Fact]
		public async Task GetUserCallLogsAsync_WithMinimalParameters()
		{
			// Arrange
			var userId = "user123";
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": [
					{
						""id"": ""log1"",
						""caller_number"": ""1234"",
						""callee_number"": ""5678"",
						""direction"": ""inbound"",
						""duration"": 60,
						""date_time"": ""2023-12-06T10:00:00Z""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "30")
				.Respond("application/json", userCallLogsJson);

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
			var from = new DateTime(2023, 12, 1);
			var to = new DateTime(2023, 12, 31);
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("from", "2023-12-01")
				.WithQueryString("to", "2023-12-31")
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "30")
				.Respond("application/json", userCallLogsJson);

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
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "missed")
				.WithQueryString("page_size", "30")
				.Respond("application/json", userCallLogsJson);

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
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("phone_number", phoneNumber)
				.WithQueryString("page_size", "30")
				.Respond("application/json", userCallLogsJson);

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
			var userCallLogsJson = @"{
				""next_page_token"": ""token456"",
				""page_size"": 50,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("page_size", "50")
				.WithQueryString("next_page_token", "token123")
				.Respond("application/json", userCallLogsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var calllogs = new CallLogs(client);

			// Act
			var result = await calllogs.GetAsync(userId, recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(50);
		}

		[Fact]
		public async Task GetUserCallLogsAsync_WithAllParameters()
		{
			// Arrange
			var userId = "user123";
			var from = new DateTime(2023, 11, 1);
			var to = new DateTime(2023, 11, 30);
			var type = CallLogType.Missed;
			var phoneNumber = "+9876543210";
			var recordsPerPage = 100;
			var pagingToken = "full_token";
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 100,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("from", "2023-11-01")
				.WithQueryString("to", "2023-11-30")
				.WithQueryString("type", "missed")
				.WithQueryString("phone_number", phoneNumber)
				.WithQueryString("page_size", "100")
				.WithQueryString("next_page_token", "full_token")
				.Respond("application/json", userCallLogsJson);

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
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "all")
				.Respond("application/json", userCallLogsJson);

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
			var userCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users", userId, "call_logs"))
				.WithQueryString("type", "missed")
				.Respond("application/json", userCallLogsJson);

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
			var accountCallLogsJson = @"{
				""next_page_token"": """",
				""page_size"": 30,
				""from"": ""2025-11-01"",
				""to"": ""2025-12-01"",
				""call_logs"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("type", "all")
				.WithQueryString("timeType", "startTime")
				.WithQueryString("charged_call_logs", false.ToString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", accountCallLogsJson);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
			var from = new DateTime(2023, 10, 1);
			var to = new DateTime(2023, 10, 31);
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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
			var from = new DateTime(2023, 9, 1);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("from", "2023-09-01")
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
			var to = new DateTime(2023, 9, 30);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "call_logs"))
				.WithQueryString("to", "2023-09-30")
				.Respond("application/json", MULTIPLE_ACCOUNT_CALL_LOGS_JSON);

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
