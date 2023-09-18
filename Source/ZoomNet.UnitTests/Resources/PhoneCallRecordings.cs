using RichardSzalay.MockHttp;
using Shouldly;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PhoneCallRecordingsTests
	{
		[Fact]
		public async Task GetRecordingAsync()
		{
			// Arrange
			string callId = "1234567890123456789";
			
			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(
					HttpMethod.Get,
					Utils.GetZoomApiUri("phone/call_logs", callId, "recordings"))
				.Respond(
					"application/json",
					Models.PhoneCallRecordingsTests.PHONE_CALL_RECORDING);

			var client = Utils.GetFluentClient(mockHttp);
			var phone = new Phone(client);

			// Act
			var result = await phone
				.GetRecordingAsync(callId, CancellationToken.None)
				.ConfigureAwait(false);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldNotBeNull();
			result.CallId.ShouldBe(callId);
		}
	}
}
