using System.Text.Json;
using Shouldly;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class PhoneUserTests
	{
		#region constants

		internal const string PHONE_USER = @"{
			""id"": ""NL3cEpSdRc-c2t8aLoZqiw"",
			""phone_user_id"": ""u7pnC468TaS46OuNoEw6GA"",
			""email"": ""test_phone_user@testapi.com"",
			""name"": ""test phone user"",
			""extension_id"": ""CcrEGgmeQem1uyJsuIRKwA"",
			""extension_number"": 123,
			""status"": ""activate"",
			""calling_plans"": [
				{
					""type"": 600,
					""name"": ""Delhi billing"",
					""billing_account_id"": ""3WWAEiEjTj2IQuyDiKMd_A"",
					""billing_account_name"": ""Delhi billing""
				}
			],
			""phone_numbers"": [
				{
					""id"": ""---M1padRvSUtw7YihN7sA"",
					""number"": ""14232058798""
				}
			],
			""site"": {
				""id"": ""8f71O6rWT8KFUGQmJIFAdQ"",
				""name"": ""Test Site""
			},
			""department"": ""Test"",
			""cost_center"": ""Cost Test Center""
		}";

		#endregion

		#region tests

		[Fact]
		public void Parse_Json_PhoneUserTests()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneUser>(
				PHONE_USER, JsonFormatter.SerializerOptions);

			// Assert
			result.Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			result.PhoneUserId.ShouldBe("u7pnC468TaS46OuNoEw6GA");
			result.Email.ShouldBe("test_phone_user@testapi.com");
			result.Name.ShouldBe("test phone user");
			result.ExtensionId.ShouldBe("CcrEGgmeQem1uyJsuIRKwA");
			result.ExtensionNumber.ShouldBe(123);
			result.Status.ShouldBe(PhoneCallUserStatus.Active);
			result.CallingPlans.ShouldNotBeNull();
			result.CallingPlans.Length.ShouldBe(1);
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.Length.ShouldBe(1);
			result.Department.ShouldBe("Test");
			result.CostCenter.ShouldBe("Cost Test Center");
			result.Site.Id.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
			result.Site.Name.ShouldBe("Test Site");

			var callingPlan = result.CallingPlans[0];
			callingPlan.BillingAccountId.ShouldBe("3WWAEiEjTj2IQuyDiKMd_A");
			callingPlan.BillingAccountName.ShouldBe("Delhi billing");

			var phoneNumber = result.PhoneNumbers[0];
			phoneNumber.PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			phoneNumber.PhoneNumber.ShouldBe("14232058798");
		}

		#endregion
	}
}
