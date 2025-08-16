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
	public class AccountCallLogsTests
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

		public AccountCallLogsTests(ITestOutputHelper outputHelper)
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
			result.RecordingType.ShouldBe("Automatic");
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
			var result = await calllogs.GetAsync(from, to, CallLogType.All, null, CallLogTimeType.StartTime, null, false, recordsPerPage, null, TestContext.Current.CancellationToken).ConfigureAwait(true);

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
	}
}
