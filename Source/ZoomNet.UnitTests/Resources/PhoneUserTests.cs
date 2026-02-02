using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneUserTests
	{
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
				.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
				.Respond("application/json", EndpointsResource.phone_users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone.ListPhoneUsersAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.NextPageToken.ShouldNotBeNullOrEmpty();
			result.RecordsPerPage.ShouldBe(1);
			result.TotalRecords.ShouldBe(10);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			result.Records[0].PhoneNumbers[0].PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			result.Records[0].PhoneNumbers[0].PhoneNumber.ShouldBe("14232058798");
		}
	}
}
