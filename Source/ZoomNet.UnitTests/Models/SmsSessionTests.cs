using Shouldly;
using System;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class SmsSessionTests
	{
		#region constants

		internal const string SMS_HISTORY = @"{
			""attachments"": [
					{
						""download_url"": ""https://exampleurl.us/file/download/x18dcVWxTcCzbp4zr2AT3A?jwt=eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjoiYTZkODE4NzQ2MDNmN2UzZWM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oto"",
						""id"": ""x18dcVWxTcCzbp4zr2AT3A"",
						""name"": ""FWDHOMaNRaqIvNc3aIdisg.jpg"",
						""size"": 225740,
						""type"": ""JPG""
					},
					{
						""download_url"": ""https://exampleurl.us/file/download/TZkODE4NzQ2MDNmN2UzZWM?jwt=rGJXbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjASf7fsJsfhl88jf02fLgyuM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oti"",
						""id"": ""TZkODE4NzQ2MDNmN2UzZWM"",
						""name"": ""ASf7fsJsfhl88jf02fLgyu.mp3"",
						""size"": 303890,
						""type"": ""AUDIO""
					}
			],
			""date_time"": ""2024-03-23T02:58:01Z"",
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
		}";

		#endregion

		#region Tests

		[Fact]
		public void Parse_Json_SmsHistory()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<SmsMessage>(
				SMS_HISTORY, JsonFormatter.DefaultDeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Attachments.ShouldNotBeNull();
			result.Attachments.Length.ShouldBe(2);
			result.Attachments[0].DownloadUrl.ShouldBe("https://exampleurl.us/file/download/x18dcVWxTcCzbp4zr2AT3A?jwt=eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjoiYTZkODE4NzQ2MDNmN2UzZWM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oto");
			result.Attachments[0].Id.ShouldBe("x18dcVWxTcCzbp4zr2AT3A");
			result.Attachments[0].Name.ShouldBe("FWDHOMaNRaqIvNc3aIdisg.jpg");
			result.Attachments[0].Size.ShouldBe(225740);
			result.Attachments[0].Type.ShouldBe(SmsAttachmentType.Jpg);
			result.Attachments[1].DownloadUrl.ShouldBe("https://exampleurl.us/file/download/TZkODE4NzQ2MDNmN2UzZWM?jwt=rGJXbGciOiJIUzI1NiJ9.eyJpc3MiOiJjcm9zc2ZpbGUiLCJhdWQiOiJmaWxlIiwiZGlnIjASf7fsJsfhl88jf02fLgyuM4OThkNDMxM2IxNjNhNTQ4NGI4MjkxMTA0ZmQyYzc4MTg1NmY0MGUxY2FlOTI3YyIsImV4cCI6MTY0ODE5NDA1NH0.eCURcan9QOOw9wvBdSn_-TBzgT5HWBzp04IfsK19Oti");
			result.Attachments[1].Id.ShouldBe("TZkODE4NzQ2MDNmN2UzZWM");
			result.Attachments[1].Name.ShouldBe("ASf7fsJsfhl88jf02fLgyu.mp3");
			result.Attachments[1].Size.ShouldBe(303890);
			result.Attachments[1].Type.ShouldBe(SmsAttachmentType.Audio);

			result.CreatedOn.ShouldBe(new DateTime(2024, 3, 23, 2, 58, 1, DateTimeKind.Utc));
			result.Direction.ShouldBe(SmsDirection.Inbound);
			result.Message.ShouldBe("welcome");
			result.MessageId.ShouldBe("IQ-cRH5P5EiTWCwpNzScnECJw");
			result.Type.ShouldBe(SmsMessageType.Mms);
			result.Sender.ShouldNotBeNull();
			result.Sender.PhoneNumber.ShouldBe("18108001001");
			result.Sender.DisplayName.ShouldBe("test api");
			result.Sender.Owner.ShouldNotBeNull();
			result.Sender.Owner.Id.ShouldBe("DnEopNmXQEGU2uvvzjgojw");
			result.Sender.Owner.Type.ShouldBe(SmsParticipantOwnerType.User);
			result.Recipients.ShouldNotBeNull();
			result.Recipients.Length.ShouldBe(1);
			result.Recipients[0].DisplayName.ShouldBe("ezreal mao");
			result.Recipients[0].PhoneNumber.ShouldBe("12092693625");
			result.Recipients[0].Owner.ShouldNotBeNull();
			result.Recipients[0].Owner.Id.ShouldBe("WeD59Hn7SvqNRB9jcxz5NQ");
			result.Recipients[0].Owner.Type.ShouldBe(SmsParticipantOwnerType.User);
		}

		#endregion
	}
}
