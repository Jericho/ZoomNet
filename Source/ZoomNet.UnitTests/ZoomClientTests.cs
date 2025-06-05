using Shouldly;
using System;
using System.Net;
using Xunit;

namespace ZoomNet.UnitTests
{
	public class ZoomClientTests
	{
		private const string API_KEY = "my_api_key";
		private const string API_SECRET = "my_api_secret";

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
			var connectionInfo = new JwtConnectionInfo(API_KEY, API_SECRET);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null, logger: logger);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}

		[Fact]
		public void Throws_if_apikey_is_null()
		{
			// Arrange
			var logger = _outputHelper.ToLogger<IZoomClient>();

			// Act
			Should.Throw<ArgumentNullException>(() => new ZoomClient(new JwtConnectionInfo(null, API_SECRET), logger: logger));
		}
	}
}
