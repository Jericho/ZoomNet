using RichardSzalay.MockHttp;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneCallUserProfileTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public PhoneCallUserProfileTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

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

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var phone = new Phone(client);

			// Act
			var result = await phone
				.GetPhoneCallUserProfileAsync(userId, TestContext.Current.CancellationToken)
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
