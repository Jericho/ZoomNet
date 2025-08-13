using Shouldly;
using System;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class ChatMembersEditResultTests
	{

		#region FIELDS

		internal const string Promote_channel_members_to_administrators_JSON = @"{
			""added_at"": ""2020-02-10T21:39:50Z"",
			""ids"": ""D40dy5L7SJiSTayIvRV9Lw,KT6h5SfCSm6YNjZo7i8few"",
			""member_ids"": ""R4VM29Oj0fVM2hhEmSKVM2hhezJTezJTKVM2hezJT2hezJ,R4VM29Oj0fVM2hhEmSKVM2hhezJTezJTKVM2hezJT2hezJ""
		}";

		#endregion

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<ChatMembersEditResult>(Promote_channel_members_to_administrators_JSON, JsonFormatter.DefaultDeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.AddedAt.ShouldBe(new DateTime(2020, 2, 10, 21, 39, 50, DateTimeKind.Utc));
			result.Ids.ShouldBe(new string[] { "D40dy5L7SJiSTayIvRV9Lw", "KT6h5SfCSm6YNjZo7i8few" });
			result.MemberIds.ShouldBe(new string[] { "R4VM29Oj0fVM2hhEmSKVM2hhezJTezJTKVM2hezJT2hezJ", "R4VM29Oj0fVM2hhEmSKVM2hhezJTezJTKVM2hezJT2hezJ" });
		}
	}
}
