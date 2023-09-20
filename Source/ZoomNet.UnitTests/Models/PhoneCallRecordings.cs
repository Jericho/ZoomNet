using Shouldly;
using System;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class PhoneCallRecordingsTests
	{
		#region constants

		internal const string PHONE_CALL_RECORDING = @"{
			""id"": ""1234abcd5678efgh9012ijkl3456mnop"",
			""caller_number"": ""+12345678901"",
			""caller_number_type"": 2,
			""caller_name"": ""12345678901"",
			""callee_number"": ""123"",
			""callee_number_type"": 1,
			""callee_name"": ""Callee Name"",
			""direction"": ""inbound"",
			""duration"": 25,
			""download_url"": ""https://zoom.us/v2/phone/recording/download/Id_abc123DEF456ghi789J"",
			""file_url"": ""https://file.zoom.us/file?business=phone&filename=call_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3&jwt=eyJhbGciOiJIUzI1NiJ9.eyJoZGlnIjpmYWxzZSwiaXNzIjoiY3Jvc3NmaWxlIiwiYXVkIjoiZmlsZSIsIm9yaSI6InBieHdlYiIsImRpZyI6ImZmNDg5ZmE3Y2NhMjdlZmVmYzY3MmE4ZjBhODFmYjYwODBkNjI0NGZiNjQ5ZmQ5MThkY2NhMmI4YmQyYzYxYjMiLCJpYXQiOjE2OTQ4MDY1MDAsImlpYyI6ImF3MSIsImV4cCI6MTY5NDgwODMwMH0.UZ_6_j4f1ibvuiMjSAqba9pk51roqE9hAu54S8FDEMw&mode=play&path=zoomfs%3A%2F%2Fzoom-pbx%2Frecording%2F2023%2F8%2F15%2FbVaPMLDOTEq4rOIJYmStRA%2FWZYkJSSXTxywYfFaUHbMFw%2F1234abcd-5678-efgh-9012-ijkl3456mnop%2Fcall_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3"",
			""date_time"": ""2023-09-16T12:34:56Z"",
			""recording_type"": ""Automatic"",
			""call_log_id"": ""1234abcd-5678-efgh-9012-ijkl3456mnop"",
			""call_id"": ""1234567890123456789"",
			""owner"": {
				""type"": ""user"",
				""id"": ""bGcIjUHbbOOk8Yoi0_6dfT"",
				""name"": ""Owner Name"",
				""extension_number"": 800
			},
			""site"": {
				""id"": ""8f71O6rWT8KFUGQmJIFAdQ""
			},
			""end_time"": ""2023-09-16T12:35:21Z"",
			""disclaimer_status"": 0
		}";

		internal const string PHONE_CALL_RECORDING_EXTENDED = @"{
			""id"": ""1234abcd5678efgh9012ijkl3456mnop"",
			""caller_number"": ""+12345678901"",
			""caller_number_type"": 2,
			""caller_name"": ""12345678901"",
			""callee_number"": ""123"",
			""callee_number_type"": 1,
			""callee_name"": ""Callee Name"",
			""outgoing_by"": {
				""name"": ""Call Initiator"",
				""extension_number"": ""100""
			},
			""accepted_by"": {
				""name"": ""Call Receiver"",
				""extension_number"": ""200""
			},
			""direction"": ""inbound"",
			""duration"": 25,
			""download_url"": ""https://zoom.us/v2/phone/recording/download/Id_abc123DEF456ghi789J"",
			""file_url"": ""https://file.zoom.us/file?business=phone&filename=call_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3&jwt=eyJhbGciOiJIUzI1NiJ9.eyJoZGlnIjpmYWxzZSwiaXNzIjoiY3Jvc3NmaWxlIiwiYXVkIjoiZmlsZSIsIm9yaSI6InBieHdlYiIsImRpZyI6ImZmNDg5ZmE3Y2NhMjdlZmVmYzY3MmE4ZjBhODFmYjYwODBkNjI0NGZiNjQ5ZmQ5MThkY2NhMmI4YmQyYzYxYjMiLCJpYXQiOjE2OTQ4MDY1MDAsImlpYyI6ImF3MSIsImV4cCI6MTY5NDgwODMwMH0.UZ_6_j4f1ibvuiMjSAqba9pk51roqE9hAu54S8FDEMw&mode=play&path=zoomfs%3A%2F%2Fzoom-pbx%2Frecording%2F2023%2F8%2F15%2FbVaPMLDOTEq4rOIJYmStRA%2FWZYkJSSXTxywYfFaUHbMFw%2F1234abcd-5678-efgh-9012-ijkl3456mnop%2Fcall_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3"",
			""date_time"": ""2023-09-16T12:34:56Z"",
			""recording_type"": ""Automatic"",
			""call_log_id"": ""1234abcd-5678-efgh-9012-ijkl3456mnop"",
			""call_id"": ""1234567890123456789"",
			""owner"": {
				""type"": ""call queue"",
				""id"": ""bGcIjUHbbOOk8Yoi0_6dfT"",
				""name"": ""Owner Name"",
				""extension_number"": 800,
				""extension_status"": ""inactive""
			},
			""site"": {
				""id"": ""8f71O6rWT8KFUGQmJIFAdQ"",
				""name"": ""Site Name""
			},
			""end_time"": ""2023-09-16T12:35:21Z"",
			""disclaimer_status"": 1
		}";

		internal const string PHONE_CALL_RECORDING_TRANSCRIPT = @"{
			""type"": ""zoom_transcript"",
			""ver"": 1,
			""recording_id"": ""1234abcd5678efgh9012ijkl3456mnop"",
			""meeting_id"": ""abcdefghijklmnop1234567890123456"",
			""account_id"": ""yIuKOPVYTg7FU0cIpgErD3"",
			""host_id"": ""yg6gFTJIu88fdrtUOIGft5"",
			""recording_start"": ""2023-09-16T12:34:56Z"",
			""recording_end"": ""2023-09-16T12:35:21Z"",
			""timeline"": [
				{
					""text"": ""Lorem Ipsum."",
					""end_ts"": ""00:00:02.584"",
					""ts"": ""00:00:00.789"",
					""users"": [
						{
							""username"": ""+12345678901"",
							""multiple_people"": false,
							""user_id"": ""+12345678901"",
							""zoom_userid"": ""Unknown Speaker"",
							""client_type"": 0
						}
					]
				},
				{
					""text"": ""Dolor sit amet."",
					""end_ts"": ""00:00:04.923"",
					""ts"": ""00:00:03.172"",
					""users"": [
						{
							""username"": ""Callee Name"",
							""multiple_people"": true,
							""user_id"": ""123"",
							""zoom_userid"": ""hYU_fr-6tdVBN0IPvvTxeR"",
							""client_type"": 1
						},
						{
							""username"": ""+12345678901"",
							""multiple_people"": false,
							""user_id"": ""+12345678901"",
							""zoom_userid"": ""Unknown Speaker"",
							""client_type"": 0
						}
					]
				},
				{
					""text"": ""Consectetur adipiscing elit."",
					""end_ts"": ""00:00:05.000"",
					""ts"": ""00:00:08.435"",
					""users"": []
				}
			]
		}";

		#endregion

		#region tests

		[Fact]
		public void Parse_Json_PhoneCallRecording()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneCallRecording>(
				PHONE_CALL_RECORDING, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("1234abcd5678efgh9012ijkl3456mnop");
			result.CallerNumber.ShouldBe("+12345678901");
			result.CallerNumberType.ShouldBe(PhoneCallRecordingCallerNumberType.External);
			result.CallerName.ShouldBe("12345678901");
			result.CalleeNumber.ShouldBe("123");
			result.CalleeNumberType.ShouldBe(PhoneCallRecordingCalleeNumberType.Internal);
			result.CalleeName.ShouldBe("Callee Name");
			result.CallInitiator.ShouldBeNull();
			result.CallReceiver.ShouldBeNull();
			result.Direction.ShouldBe(CallLogDirection.Inbound);
			result.Duration.ShouldBe(25);
			result.DownloadUrl.ShouldBe("https://zoom.us/v2/phone/recording/download/Id_abc123DEF456ghi789J");
			result.FileUrl.ShouldBe("https://file.zoom.us/file?business=phone&filename=call_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3&jwt=eyJhbGciOiJIUzI1NiJ9.eyJoZGlnIjpmYWxzZSwiaXNzIjoiY3Jvc3NmaWxlIiwiYXVkIjoiZmlsZSIsIm9yaSI6InBieHdlYiIsImRpZyI6ImZmNDg5ZmE3Y2NhMjdlZmVmYzY3MmE4ZjBhODFmYjYwODBkNjI0NGZiNjQ5ZmQ5MThkY2NhMmI4YmQyYzYxYjMiLCJpYXQiOjE2OTQ4MDY1MDAsImlpYyI6ImF3MSIsImV4cCI6MTY5NDgwODMwMH0.UZ_6_j4f1ibvuiMjSAqba9pk51roqE9hAu54S8FDEMw&mode=play&path=zoomfs%3A%2F%2Fzoom-pbx%2Frecording%2F2023%2F8%2F15%2FbVaPMLDOTEq4rOIJYmStRA%2FWZYkJSSXTxywYfFaUHbMFw%2F1234abcd-5678-efgh-9012-ijkl3456mnop%2Fcall_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3");
			result.StartDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 34, 56, DateTimeKind.Utc));
			result.Type.ShouldBe(PhoneCallRecordingType.Automatic);
			result.CallLogId.ShouldBe("1234abcd-5678-efgh-9012-ijkl3456mnop");
			result.CallId.ShouldBe("1234567890123456789");
			result.Owner.Type.ShouldBe(PhoneCallRecordingOwnerType.User);
			result.Owner.Id.ShouldBe("bGcIjUHbbOOk8Yoi0_6dfT");
			result.Owner.Name.ShouldBe("Owner Name");
			result.Owner.ExtensionNumber.ShouldBe(800);
			result.Owner.ExtensionStatus.ShouldBeNull();
			result.Site.Id.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
			result.Site.Name.ShouldBeNull();
			result.EndDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 35, 21, DateTimeKind.Utc));
			result.DisclaimerStatus.ShouldBe(PhoneCallRecordingDisclaimerStatus.Implicit);
		}

		[Fact]
		public void Parse_Json_PhoneCallRecording_Extended()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneCallRecording>(
				PHONE_CALL_RECORDING_EXTENDED, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("1234abcd5678efgh9012ijkl3456mnop");
			result.CallerNumber.ShouldBe("+12345678901");
			result.CallerNumberType.ShouldBe(PhoneCallRecordingCallerNumberType.External);
			result.CallerName.ShouldBe("12345678901");
			result.CalleeNumber.ShouldBe("123");
			result.CalleeNumberType.ShouldBe(PhoneCallRecordingCalleeNumberType.Internal);
			result.CalleeName.ShouldBe("Callee Name");
			result.CallInitiator.Name.ShouldBe("Call Initiator");
			result.CallInitiator.ExtensionNumber.ShouldBe("100");
			result.CallReceiver.Name.ShouldBe("Call Receiver");
			result.CallReceiver.ExtensionNumber.ShouldBe("200");
			result.Direction.ShouldBe(CallLogDirection.Inbound);
			result.Duration.ShouldBe(25);
			result.DownloadUrl.ShouldBe("https://zoom.us/v2/phone/recording/download/Id_abc123DEF456ghi789J");
			result.FileUrl.ShouldBe("https://file.zoom.us/file?business=phone&filename=call_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3&jwt=eyJhbGciOiJIUzI1NiJ9.eyJoZGlnIjpmYWxzZSwiaXNzIjoiY3Jvc3NmaWxlIiwiYXVkIjoiZmlsZSIsIm9yaSI6InBieHdlYiIsImRpZyI6ImZmNDg5ZmE3Y2NhMjdlZmVmYzY3MmE4ZjBhODFmYjYwODBkNjI0NGZiNjQ5ZmQ5MThkY2NhMmI4YmQyYzYxYjMiLCJpYXQiOjE2OTQ4MDY1MDAsImlpYyI6ImF3MSIsImV4cCI6MTY5NDgwODMwMH0.UZ_6_j4f1ibvuiMjSAqba9pk51roqE9hAu54S8FDEMw&mode=play&path=zoomfs%3A%2F%2Fzoom-pbx%2Frecording%2F2023%2F8%2F15%2FbVaPMLDOTEq4rOIJYmStRA%2FWZYkJSSXTxywYfFaUHbMFw%2F1234abcd-5678-efgh-9012-ijkl3456mnop%2Fcall_recording_1234abcd-5678-efgh-9012-ijkl3456mnop_20230916123456.mp3");
			result.StartDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 34, 56, DateTimeKind.Utc));
			result.Type.ShouldBe(PhoneCallRecordingType.Automatic);
			result.CallLogId.ShouldBe("1234abcd-5678-efgh-9012-ijkl3456mnop");
			result.CallId.ShouldBe("1234567890123456789");
			result.Owner.Type.ShouldBe(PhoneCallRecordingOwnerType.CallQueue);
			result.Owner.Id.ShouldBe("bGcIjUHbbOOk8Yoi0_6dfT");
			result.Owner.Name.ShouldBe("Owner Name");
			result.Owner.ExtensionNumber.ShouldBe(800);
			result.Owner.ExtensionStatus.ShouldBe(PhoneCallRecordingOwnerExtensionStatus.Inactive);
			result.Site.Id.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
			result.Site.Name.ShouldBe("Site Name");
			result.EndDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 35, 21, DateTimeKind.Utc));
			result.DisclaimerStatus.ShouldBe(PhoneCallRecordingDisclaimerStatus.Agree);
		}

		[Fact]
		public void Parse_Json_PhoneCallRecordingTranscript()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneCallRecordingTranscript>(
				PHONE_CALL_RECORDING_TRANSCRIPT, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Type.ShouldBe("zoom_transcript");
			result.Version.ShouldBe(1);
			result.RecordingId.ShouldBe("1234abcd5678efgh9012ijkl3456mnop");
			result.MeetingId.ShouldBe("abcdefghijklmnop1234567890123456");
			result.AccountId.ShouldBe("yIuKOPVYTg7FU0cIpgErD3");
			result.HostId.ShouldBe("yg6gFTJIu88fdrtUOIGft5");
			result.StartDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 34, 56, DateTimeKind.Utc));
			result.EndDateTime.ShouldBe(new DateTime(2023, 9, 16, 12, 35, 21, DateTimeKind.Utc));
			result.TimelineFractions.ShouldNotBeNull();
			result.TimelineFractions.Length.ShouldBe(3);
			result.TimelineFractions[0].Text.ShouldBe("Lorem Ipsum.");
			result.TimelineFractions[0].StartTimeSpan.ShouldBe(TimeSpan.Parse("00:00:00.789"));
			result.TimelineFractions[0].EndTimeSpan.ShouldBe(TimeSpan.Parse("00:00:02.584"));
			result.TimelineFractions[0].Users.ShouldNotBeNull();
			result.TimelineFractions[0].Users.Length.ShouldBe(1);
			result.TimelineFractions[0].Users[0].Username.ShouldBe("+12345678901");
			result.TimelineFractions[0].Users[0].IsMultiplePeople.ShouldBeFalse();
			result.TimelineFractions[0].Users[0].UserId.ShouldBe("+12345678901");
			result.TimelineFractions[0].Users[0].ZoomUserId.ShouldBe("Unknown Speaker");
			result.TimelineFractions[0].Users[0].ClientType.ShouldBe(0);
			result.TimelineFractions[1].Text.ShouldBe("Dolor sit amet.");
			result.TimelineFractions[1].StartTimeSpan.ShouldBe(TimeSpan.Parse("00:00:03.172"));
			result.TimelineFractions[1].EndTimeSpan.ShouldBe(TimeSpan.Parse("00:00:04.923"));
			result.TimelineFractions[1].Users.ShouldNotBeNull();
			result.TimelineFractions[1].Users.Length.ShouldBe(2);
			result.TimelineFractions[1].Users[0].Username.ShouldBe("Callee Name");
			result.TimelineFractions[1].Users[0].IsMultiplePeople.ShouldBeTrue();
			result.TimelineFractions[1].Users[0].UserId.ShouldBe("123");
			result.TimelineFractions[1].Users[0].ZoomUserId.ShouldBe("hYU_fr-6tdVBN0IPvvTxeR");
			result.TimelineFractions[1].Users[0].ClientType.ShouldBe(1);
			result.TimelineFractions[1].Users[1].Username.ShouldBe("+12345678901");
			result.TimelineFractions[1].Users[1].IsMultiplePeople.ShouldBeFalse();
			result.TimelineFractions[1].Users[1].UserId.ShouldBe("+12345678901");
			result.TimelineFractions[1].Users[1].ZoomUserId.ShouldBe("Unknown Speaker");
			result.TimelineFractions[1].Users[1].ClientType.ShouldBe(0);
			result.TimelineFractions[2].Text.ShouldBe("Consectetur adipiscing elit.");
			result.TimelineFractions[2].StartTimeSpan.ShouldBe(TimeSpan.Parse("00:00:08.435"));
			result.TimelineFractions[2].EndTimeSpan.ShouldBe(TimeSpan.Parse("00:00:05.000"));
			result.TimelineFractions[2].Users.ShouldNotBeNull();
			result.TimelineFractions[2].Users.Length.ShouldBe(0);
		}

		#endregion
	}
}
