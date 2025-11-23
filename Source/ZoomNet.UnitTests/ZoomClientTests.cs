using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace ZoomNet.UnitTests
{
	public class ZoomClientTests
	{
		private const string CLIENT_ID = "my_client_id";
		private const string CLIENT_SECRET = "my_client_secret";
		private const string ACCOUNT_ID = "my_account_id";
		private const string ACCESS_TOKEN = null;

		private readonly ITestOutputHelper _outputHelper;

		public ZoomClientTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public void Version_is_not_empty()
		{
			// Arrange

			// Act
			var result = ZoomClient.Version;

			// Assert
			result.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void Dispose()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null, logger: logger);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}

		[Fact]
		public void Throws_if_connectioninfo_is_null()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = (IConnectionInfo)null;

			// Act
			Should.Throw<ArgumentNullException>(() => new ZoomClient(connectionInfo, logger: logger));
		}

		[Fact]
		public void Throws_if_httpclient_is_null()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer(CLIENT_ID, CLIENT_SECRET, ACCOUNT_ID, ACCESS_TOKEN);
			var httpClient = (HttpClient)null;

			// Act
			Should.Throw<ArgumentNullException>(() => new ZoomClient(connectionInfo, httpClient, logger: logger));
		}
	}
}
