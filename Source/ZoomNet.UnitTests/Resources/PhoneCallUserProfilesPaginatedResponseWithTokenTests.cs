using System;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneCallUserProfilesPaginatedResponseWithTokenTests
	{
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

		[Fact]
		public async Task PhoneCallUserProfilesPaginatedResponseTestsAsync()
		{
			// Arrange
			var pageSize = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(
					HttpMethod.Get,
					Utils.GetZoomApiUri("phone/users"))
				.Respond(
					"application/json",
					PHONE_CALL_USER_PROFILES_PAGINATED_OBJECT);

			var client = Utils.GetFluentClient(mockHttp);
			var phone = new Phone(client);

			// Act
			var result = await phone
				.ListPhoneCallUserProfilesAsync(pageSize: pageSize)
				.ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.NextPageToken.ShouldNotBeNullOrEmpty();
			result.PageSize.ShouldBe(1);
			result.TotalRecords.ShouldBe(10);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			result.Records[0].PhoneNumbers[0].PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			result.Records[0].PhoneNumbers[0].PhoneNumber.ShouldBe("14232058798");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(101)]
		public void InvalidPageSize_PhoneCallUserProfilesPaginatedResponseTests(int pageSize)
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();

			var client = Utils.GetFluentClient(mockHttp);
			var phone = new Phone(client);

			// Act and Assert
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => phone
				.ListPhoneCallUserProfilesAsync(pageSize: pageSize)
				.ConfigureAwait(true));

			exception.ParamName.ShouldBe(nameof(pageSize));
			exception.Message.ShouldBe($"Records per page must be between 1 and 100 (Parameter '{nameof(pageSize)}')");
		}
	}
}
