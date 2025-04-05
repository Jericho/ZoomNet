using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneUserTests
	{
		private const string PHONE_USERS_PAGINATED_OBJECT = @"{
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

		private readonly ITestOutputHelper _outputHelper;

		public PhoneUserTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public async Task GetPhoneUsersPaginatedResponseTestsAsync()
		{
			// Arrange
			var recordsPerPage = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(
					HttpMethod.Get,
					Utils.GetZoomApiUri("phone/users"))
				.Respond(
					"application/json",
					PHONE_USERS_PAGINATED_OBJECT);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone
				.ListPhoneUsersAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken)
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
		public void InvalidPageSize_GetPhoneUsersPaginatedResponseTests(int recordsPerPage)
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act and Assert
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => phone
				.ListPhoneUsersAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken)
				.ConfigureAwait(true));

			exception.ParamName.ShouldBe(nameof(recordsPerPage));
			exception.Message.ShouldStartWith("Records per page must be between 1 and 100");
		}
	}
}
