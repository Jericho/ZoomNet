using System.Text.Json;
using Shouldly;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class PhoneCallUserProfilesPaginationObjectTests
	{
		#region constants

		internal const string PHONE_CALL_USER_PROFILES_PAGINATED_OBJECT = @"{
			""next_page_token"": ""F2qwertyg5eIqRRgC2YMauur8ZHUaJqtS3i"",
			""page_size"": 1,
			""total_records"": 10,
			""users"": [
				{
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
				}
			]
		}";

		#endregion

		#region tests

		[Fact]
		public void Parse_Json_PhoneCallUserProfilesPaginationObjectTests()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneCallUserProfilesPaginationObject>(
				PHONE_CALL_USER_PROFILES_PAGINATED_OBJECT, JsonFormatter.SerializerOptions);

			// Assert
			result.NextPageToken.ShouldNotBeNullOrEmpty();
			result.PageSize.ShouldBe(1);
			result.TotalRecords.ShouldBe(10);
			result.Users.ShouldNotBeNull();
			result.Users.Length.ShouldBe(1);

			var user = result.Users[0];
			user.Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			user.PhoneUserId.ShouldBe("u7pnC468TaS46OuNoEw6GA");
			user.Email.ShouldBe("test_phone_user@testapi.com");
			user.Name.ShouldBe("test phone user");
			user.ExtensionId.ShouldBe("CcrEGgmeQem1uyJsuIRKwA");
			user.ExtensionNumber.ShouldBe(123);
			user.Status.ShouldBe(PhoneCallUserStatus.Active);
			user.CallingPlans.ShouldNotBeNull();
			user.CallingPlans.Length.ShouldBe(1);
			user.PhoneNumbers.ShouldNotBeNull();
			user.PhoneNumbers.Length.ShouldBe(1);
			user.Department.ShouldBe("Test");
			user.CostCenter.ShouldBe("Cost Test Center");

			var callingPlan = user.CallingPlans[0];
			callingPlan.BillingAccountId.ShouldBe("3WWAEiEjTj2IQuyDiKMd_A");
			callingPlan.BillingAccountName.ShouldBe("Delhi billing");

			var phoneNumber = user.PhoneNumbers[0];
			phoneNumber.PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			phoneNumber.PhoneNumber.ShouldBe("14232058798");
		}

		#endregion
	}
}
