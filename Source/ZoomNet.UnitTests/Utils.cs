using Microsoft.Extensions.Logging;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using ZoomNet.Json;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests
{
	public static class Utils
	{
		private const string ZOOM_V2_BASE_URI = "https://api.zoom.us/v2";

		public static IClient GetFluentClient(MockHttpMessageHandler httpMessageHandler, MockHttpMessageHandler tokenMessageHandler = null, ILogger logger = null)
		{
			var client = new FluentClient(new Uri(ZOOM_V2_BASE_URI), httpMessageHandler.ToHttpClient());
			var tokenHandler = tokenMessageHandler == null ?
				new OAuthTokenHandler(OAuthConnectionInfo.ForServerToServer("bogus clientId", "bogus secret", "bogus accountId", "bogus access token"), null) :
				new OAuthTokenHandler(OAuthConnectionInfo.ForServerToServer("bogus clientId", "bogus secret", "bogus accountId"), tokenMessageHandler.ToHttpClient(), null);

			client.SetRequestCoordinator(new ZoomRetryCoordinator(new Http429RetryStrategy(), tokenHandler));
			client.Filters.Remove<DefaultErrorFilter>();

			// Remove all the built-in formatters and replace them with our custom JSON formatter
			client.Formatters.Clear();
			client.Formatters.Add(new JsonFormatter());

			// Order is important:
			//   - Token handler must be first
			//   - Diagnostic handler must be second
			//   - Error handler must be last
			// Also, the list of filters must be kept in sync with the filters in ZoomClient in the ZoomNet project.
			client.Filters.Add(tokenHandler);
			client.Filters.Add(new DiagnosticHandler(LogLevel.Debug, LogLevel.Error, logger));
			client.Filters.Add(new ZoomErrorHandler());

			return client;
		}

		public static string GetZoomApiUri(params object[] resources)
		{
			return resources.Aggregate(ZOOM_V2_BASE_URI, (current, path) => $"{current.TrimEnd('/')}/{path.ToString().TrimStart('/')}");
		}
	}
}
