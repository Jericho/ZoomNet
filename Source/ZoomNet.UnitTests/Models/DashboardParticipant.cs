using Shouldly;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class DashboardParticipantTests
	{
		#region FIELDS

		internal const string SINGLE_DASHBOARDPARTICIPANT_JSON = @"{
			""id"": ""d52f19c548b88490b5d16fcbd38"",
			""user_id"": ""32dsfsd4g5gd"",
			""user_name"": ""dojo"",
			""device"": ""Unknown"",
			""ip_address"": ""127.0.0.1"",
			""location"": ""New York"",
			""network_type"": ""Wired"",
			""microphone"": ""Plantronics BT600"",
			""camera"": ""FaceTime HD Camera"",
			""speaker"": ""Plantronics BT600"",
			""data_center"": ""SC"",
			""full_data_center"": ""United States;United States (US03_SC CRC)"",
			""connection_type"": ""P2P"",
			""join_time"": ""2019-09-07T13:15:02.837Z"",
			""leave_time"": ""2019-09-07T13:15:09.837Z"",
			""share_application"": false,
			""share_desktop"": true,
			""share_whiteboard"": true,
			""recording"": false,
			""status"": ""in_waiting_room"",
			""pc_name"": ""dojo\""s pc"",
			""domain"": ""Dojo-workspace"",
			""mac_addr"": "" 00:0a:95:9d:68:16"",
			""harddisk_id"": ""sed proident in"",
			""version"": ""4.4.55383.0716"",
			""leave_reason"": ""Dojo left the meeting.<br>Reason: Host ended the meeting."",
			""sip_uri"": ""sip:sipp@10.100.112.140:11029"",
			""from_sip_uri"": ""sip:sipp@10.100.112.140:11029"",
			""role"": ""panelist""
		}";

		#endregion

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<DashboardParticipant>(SINGLE_DASHBOARDPARTICIPANT_JSON, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Devices.ShouldNotBeNull();
			result.Devices.Length.ShouldBe(1);
			result.Devices[0].ShouldBe("Unknown");
			result.Role.ShouldBe("panelist");
		}
	}
}
