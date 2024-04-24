using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneCallUserProfilesPaginationObjectTests
	{
		[Fact]
		public async Task PhoneCallUserProfilesPaginationObjectTestsAsync()
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
					Models.PhoneCallUserProfilesPaginationObjectTests.PHONE_CALL_USER_PROFILES_PAGINATED_OBJECT);

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
			result.Users.ShouldNotBeNull();
			result.Users.Length.ShouldBe(1);
			result.Users[0].Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			result.Users[0].PhoneNumbers[0].PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			result.Users[0].PhoneNumbers[0].PhoneNumber.ShouldBe("14232058798");
		}
	}
}
