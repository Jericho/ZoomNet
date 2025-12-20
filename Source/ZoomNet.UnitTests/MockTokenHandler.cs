using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests
{
	internal class MockTokenHandler : IHttpFilter, ITokenHandler
	{
		public string Token => "This is a mock token";

		public IConnectionInfo ConnectionInfo => new MockConnectionInfo();

		public void OnRequest(IRequest request) { }

		public void OnResponse(IResponse response, bool httpErrorAsException) { }

		public string RefreshTokenIfNecessary(bool forceRefresh)
		{
			return "This is a refreshed token";
		}
	}
}
