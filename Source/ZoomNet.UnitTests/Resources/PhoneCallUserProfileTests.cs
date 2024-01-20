using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneCallUserProfileTests
	{
		[Fact]
		public async Task GetPhoneCallUserProfileAsync()
		{
			// Arrange
			string userId = "NL3cEpSdRc-c2t8aLoZqiw";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(
					HttpMethod.Get,
					Utils.GetZoomApiUri("phone/users/", userId))
				.Respond(
					"application/json",
					Models.PhoneCallUserProfileTests.PHONE_CALL_USER_PROFILE);

			var client = Utils.GetFluentClient(mockHttp);
			var phone = new Phone(client);

			// Act
			var result = await phone
				.GetPhoneCallUserProfileAsync(userId, CancellationToken.None)
				.ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldNotBeNull();
			result.Id.ShouldBe(userId);
		}
	}
}
