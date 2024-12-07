using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.IntegrationTests.TestSuites;

namespace ZoomNet.IntegrationTests
{
	internal class TestsRunner : IHostedService
	{
		private enum TestType
		{
			Api,
			WebSockets,
			Chatbot,
		}

		private enum ConnectionType
		{
			Jwt, // Zoom disabled the ability to create new JWT apps on June 1, 2023. The projected end-of-life for JWT apps is September 1, 2023. 
			OAuthAuthorizationCode, // Gets authorization code and sets refresh token.
			OAuthRefreshToken,      // Gets and sets refresh token and access token.
			OAuthClientCredentials, // Gets and sets access token. For cleanliness, it should use a different access token environment variable so they don't cross contaminate.
			OAuthServerToServer,    // Gets the account id and access token and sets access token. Same as above.
		}

		private readonly ILoggerFactory _loggerFactory;

		public TestsRunner(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// -----------------------------------------------------------------------------
			// Do you want to proxy requests through a tool such as Fiddler? Very useful for debugging.
			var useProxy = true;

			// By default Fiddler Classic uses port 8888 and Fiddler Everywhere uses port 8866
			var proxyPort = 8888;

			// What tests do you want to run
			var testType = TestType.Api;

			// Which connection type do you want to use?
			var connectionType = ConnectionType.OAuthServerToServer;
			// -----------------------------------------------------------------------------

			// As far as I know, Zoom only supports ClientCredentials when invoking the methods on the ChatBot endpoint
			if (testType == TestType.Chatbot && connectionType != ConnectionType.OAuthClientCredentials)
			{
				throw new Exception("Zoom only support client credentials when invoking the ChatBot endpoint.");
			}

			// Configure the proxy if desired
			var proxy = useProxy ? new WebProxy($"http://localhost:{proxyPort}") : null;

			// Get the connection info and test suite
			var connectionInfo = GetConnectionInfo(connectionType, testType);
			var testSuite = GetTestSuite(connectionInfo, testType, proxy, _loggerFactory);

			// Run the tests
			await testSuite.RunTestsAsync(cancellationToken).ConfigureAwait(false);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private static IConnectionInfo GetConnectionInfo(ConnectionType connectionType, TestType testType)
		{
			// Jwt
			if (connectionType == ConnectionType.Jwt)
			{
				var apiKey = Environment.GetEnvironmentVariable("ZOOM_JWT_APIKEY", EnvironmentVariableTarget.User);
				var apiSecret = Environment.GetEnvironmentVariable("ZOOM_JWT_APISECRET", EnvironmentVariableTarget.User);
				return new JwtConnectionInfo(apiKey, apiSecret);
			}

			// OAuth
			var clientIdVariableName = testType == TestType.Chatbot ? "ZOOM_CHATBOT_CLIENTID" : "ZOOM_OAUTH_CLIENTID";
			var clientSecretVariableName = testType == TestType.Chatbot ? "ZOOM_CHATBOT_CLIENTSECRET" : "ZOOM_OAUTH_CLIENTSECRET";

			var clientId = Environment.GetEnvironmentVariable(clientIdVariableName, EnvironmentVariableTarget.User);
			var clientSecret = Environment.GetEnvironmentVariable(clientSecretVariableName, EnvironmentVariableTarget.User);

			if (string.IsNullOrEmpty(clientId)) throw new Exception($"You must set the {clientIdVariableName} environment variable before you can run integration tests.");
			if (string.IsNullOrEmpty(clientSecret)) throw new Exception($"You must set the {clientSecretVariableName} environment variable before you can run integration tests.");

			switch (connectionType)
			{
				case ConnectionType.OAuthAuthorizationCode:
					{
						var authorizationCode = Environment.GetEnvironmentVariable("ZOOM_OAUTH_AUTHORIZATIONCODE", EnvironmentVariableTarget.User);

						if (string.IsNullOrEmpty(authorizationCode)) throw new Exception("Either the autorization code environment variable has not been set or it's no longer available because you already used it once.");

						return OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode,
							(newRefreshToken, newAccessToken) =>
							{
								// Clear the authorization code because it's intended to be used only once
								Environment.SetEnvironmentVariable("ZOOM_OAUTH_AUTHORIZATIONCODE", string.Empty, EnvironmentVariableTarget.User);
								Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
							});
					}
				case ConnectionType.OAuthRefreshToken:
					{
						var refreshToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", EnvironmentVariableTarget.User);
						if (string.IsNullOrEmpty(refreshToken)) throw new Exception("You must set the ZOOM_OAUTH_REFRESHTOKEN environment variable before you can run integration tests.");

						return OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken,
							(newRefreshToken, newAccessToken) =>
							{
								Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
							});
					}
				case ConnectionType.OAuthClientCredentials:
					{
						var accessTokenVariableName = testType == TestType.Chatbot ? "ZOOM_OAUTH_CHATBOT_ACCESSTOKEN" : "ZOOM_OAUTH_CLIENTCREDENTIALS_ACCESSTOKEN";
						var accessToken = Environment.GetEnvironmentVariable(accessTokenVariableName, EnvironmentVariableTarget.User);

						return OAuthConnectionInfo.WithClientCredentials(clientId, clientSecret, accessToken,
							(newRefreshToken, newAccessToken) =>
							{
								Environment.SetEnvironmentVariable(accessTokenVariableName, newAccessToken, EnvironmentVariableTarget.User);
							});
					}
				case ConnectionType.OAuthServerToServer:
					{
						var accountId = Environment.GetEnvironmentVariable("ZOOM_OAUTH_ACCOUNTID", EnvironmentVariableTarget.User);
						var accessToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_SERVERTOSERVER_ACCESSTOKEN", EnvironmentVariableTarget.User);

						return OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, accessToken,
							(newRefreshToken, newAccessToken) =>
							{
								Environment.SetEnvironmentVariable("ZOOM_OAUTH_SERVERTOSERVER_ACCESSTOKEN", newAccessToken, EnvironmentVariableTarget.User);
							});
					}
				default:
					{
						throw new Exception("Unknwon connection type");
					}
			};
		}

		private static TestSuite GetTestSuite(IConnectionInfo connectionInfo, TestType testType, IWebProxy proxy, ILoggerFactory loggerFactory)
		{
			switch (testType)
			{
				case TestType.Api: return new ApiTestSuite(connectionInfo, proxy, loggerFactory);
				case TestType.Chatbot: return new ChatbotTestSuite(connectionInfo, proxy, loggerFactory);
				case TestType.WebSockets:
					{
						var subscriptionId = Environment.GetEnvironmentVariable("ZOOM_WEBSOCKET_SUBSCRIPTIONID", EnvironmentVariableTarget.User);
						return new WebSocketsTestSuite(connectionInfo, subscriptionId, proxy, loggerFactory);
					}
				default: throw new Exception("Unknwon test type");
			};
		}
	}
}
