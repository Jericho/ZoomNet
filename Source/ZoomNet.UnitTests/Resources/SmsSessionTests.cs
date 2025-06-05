using RichardSzalay.MockHttp;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class SmsSessionTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public SmsSessionTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public async Task GetSmsSessionAsync()
		{
			// Arrange
			string sessionId = "aa74aeea445022779e375d2de1e193c5c131e7347283e3a6f308637cdd2a6832dc2cd7d4694c4a3316b010048373ad14174d6a8e0c548860dc2f3586e0de7fde8d2e097badf450605424521e2523139ddc2cd7d4694c4a3316b010048373ad141305076fc94fe1a4c8c52f47d6c3dcc72c74aeea445022779e375d2de1e193c5";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(
					HttpMethod.Get,
					Utils.GetZoomApiUri("phone/sms/sessions", sessionId))
				.Respond(
					"application/json",
					Models.SmsSessionTests.SMS_HISTORY);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var sms = new Sms(client);

			// Act
			var result = await sms
				.GetSmsSessionDetailsAsync(
					sessionId,
					from: null,
					to: null,
					cancellationToken: TestContext.Current.CancellationToken)
				.ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}
	}
}
