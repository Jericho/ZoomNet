using RichardSzalay.MockHttp;
using Shouldly;
using System.Net;
using System.Net.Http;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class OAuthTokenHandlerTests
	{
		/// <summary>
		/// This unit test demonstrates that an exception thrown in the OAuthTokenHandler
		/// when attempting to refresh tokens does not prevent us from retrying. 
		/// </summary>
		/// <remarks>
		/// This unit test was used to reproduce the problem described in this
		/// <a href="https://github.com/Jericho/ZoomNet/issues/109">Github issue</a>
		/// and to subsequently demonstrate that the bug was fixed.
		/// </remarks>
		[Fact]
		public void Attempt_to_refresh_token_multiple_times_despite_exception()
		{
			// Arrange
			var clientId = "abc123";
			var clientSecret = "xyz789";
			var authorizationCode = "INVALID_AUTH_CODE";
			var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode,
				(newRefreshToken, newAccessToken) =>
				{
					// Intentionally left blank
				},
				null);
			var apiResponse = "{ \"reason\":\"Invalid authorization code " + authorizationCode + "\",\"error\":\"invalid_request\"}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.When(HttpMethod.Post, $"https://api.zoom.us/oauth/token")
				.Respond(HttpStatusCode.BadRequest, "application/json", apiResponse);

			var handler = new OAuthTokenHandler(connectionInfo, mockHttp.ToHttpClient(), null);

			// Act
			Should.Throw<ZoomException>(() => handler.RefreshTokenIfNecessary(true));
			Should.Throw<ZoomException>(() => handler.RefreshTokenIfNecessary(true));

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}
	}
}
