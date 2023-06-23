using Formitable.BetterStack.Logger.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.IntegrationTests.TestSuites;
using ZoomNet.Utilities;

namespace ZoomNet.IntegrationTests
{
	public class Program
	{
		private enum TestType
		{
			Api,
			WebSockets,
			Chatbot,
			GraphQL,
		}

		private enum ConnectionType
		{
			OAuthAuthorizationCode, // Gets authorization code and sets refresh token.
			OAuthRefreshToken,      // Gets and sets refresh token and access token.
			OAuthClientCredentials, // Gets and sets access token. For cleanliness, it should use a different access token environment variable so they don't cross contaminate.
			OAuthServerToServer,    // Gets the account id and access token and sets access token. Same as above.
		}

		// ====================================================================================================
		// Modify the following values to configure the integration tests
		// ----------------------------------------------------------------------------------------------------

		// Do you want to proxy requests through a tool such as Fiddler? Very useful for debugging.
		private static readonly bool _useProxy = true;

		// By default Fiddler Classic uses port 8888 and Fiddler Everywhere uses port 8866
		private static readonly int _proxyPort = 8888;

		// What tests do you want to run
		private static readonly TestType _testType = TestType.Api;

		// Which connection type do you want to use?
		private static readonly ConnectionType _connectionType = ConnectionType.OAuthServerToServer;

		// ====================================================================================================

		public static async Task Main()
		{
			var serializerContextPath = Path.Combine(Path.GetDirectoryName(GetThisFilePath()), "..\\ZoomNet\\Json\\ZoomNetJsonSerializerContext.cs");
			var additionalSerializableTypes = new[]
			{
				typeof(System.Text.Json.Nodes.JsonObject),
			};
			await UpdateJsonSerializerContextAsync("ZoomNet", "ZoomNet.Models", serializerContextPath, additionalSerializableTypes).ConfigureAwait(false);

			var builder = Host.CreateApplicationBuilder();

			ConfigureLogging(builder.Logging);
			ConfigureServices(builder.Services);

			var host = builder.Build();
			await host.StartAsync().ConfigureAwait(false);
		}

		private static void ConfigureLogging(ILoggingBuilder logging)
		{
			logging.ClearProviders();

			var betterStackToken = Environment.GetEnvironmentVariable("BETTERSTACK_TOKEN");
			if (!string.IsNullOrEmpty(betterStackToken))
			{
				logging.AddBetterStackLogger(options =>
				{
					options.SourceToken = betterStackToken;
					options.Context["source"] = "ZoomNet_integration_tests";
					options.Context["ZoomNet-Version"] = ZoomClient.Version;
				});
			}

			logging.AddSimpleConsole(options =>
			{
				options.SingleLine = true;
				options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
			});

			// During API testing we want to filter out logs below Information level to avoid being overwhelmed with too many messages.
			// However, during WebSocket testing we want to see all the Trace messages.
			var consoleMinLoggingLevel = _testType == TestType.WebSockets ? LogLevel.Trace : LogLevel.Information;

			// Set a global minimum log level and then override it for the console provider.
			logging
				.SetMinimumLevel(LogLevel.Debug)
				.AddFilter<ConsoleLoggerProvider>(logLevel => logLevel >= consoleMinLoggingLevel);
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			// As far as I know, Zoom only supports ClientCredentials when invoking the methods on the ChatBot endpoint
			if (_testType == TestType.Chatbot && _connectionType != ConnectionType.OAuthClientCredentials)
			{
				throw new Exception("Zoom only support client credentials when invoking the ChatBot endpoint.");
			}

			// Configure the proxy if desired
			var proxy = _useProxy ? new WebProxy($"http://localhost:{_proxyPort}") : null;

			// Configure options
			var clientOptions = new ZoomClientOptions
			{
				// Trigger a 'Trace' log (rather than the default 'Debug') when a successful call is made.
				// This is to ensure that we don't get overwhelmed by too many debug messages in the console.
				LogLevelSuccessfulCalls = LogLevel.Trace,
				LogLevelFailedCalls = LogLevel.Error,
			}.WithCanadaBaseUrl();

			// Get the connection info
			var connectionInfo = GetConnectionInfo(_connectionType, _testType);

			services.AddZoomNet(connectionInfo, proxy, clientOptions);

			services.AddSingleton<TestSuite>(serviceProvider =>
			{
				var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
				var zoomClient = serviceProvider.GetRequiredService<IZoomClient>();

				// Create the test suite based on the selected test type
				switch (_testType)
				{
					case TestType.Api: return new ApiTestSuite(zoomClient, loggerFactory);
					case TestType.Chatbot: return new ChatbotTestSuite(zoomClient, loggerFactory);
					case TestType.WebSockets:
						{
							var subscriptionId = Environment.GetEnvironmentVariable("ZOOM_WEBSOCKET_SUBSCRIPTIONID", EnvironmentVariableTarget.User);
							return new WebSocketsTestSuite(connectionInfo, subscriptionId, proxy, loggerFactory);
						}
					case TestType.GraphQL: return new GraphQLTestSuite(connectionInfo, proxy, loggerFactory);
					default: throw new Exception("Unknwon test type");
				}
			});

			services.AddHostedService<TestsRunner>();
		}

		private static async Task UpdateJsonSerializerContextAsync(string projectName, string baseNamespace, string serializerContextPath, Type[] additionalSerializableTypes)
		{
			var tabIndex = 1; // The number of tabs to prepend to each line in the generated file (for indentation purposes)

			var newSerializerContext = new StringBuilder();
			newSerializerContext
				.AppendLine($"// This file is maintained by {projectName}.IntegrationTests.Program.\r\n")
				.AppendLine("using System.Text.Json.Serialization;\r\n")
				.AppendLine("#pragma warning disable CS0618 // Type or member is obsolete")
				.AppendLine($"namespace {projectName}.Json")
				.AppendLine("{");

			var tabs = new string('\t', tabIndex);
			foreach (var type in additionalSerializableTypes ?? Enumerable.Empty<Type>())
			{
				newSerializerContext.AppendLine($"{tabs}[JsonSerializable(typeof({type.FullName}))]");
			}

			newSerializerContext.AppendLine();

			foreach (var type in additionalSerializableTypes ?? Enumerable.Empty<Type>())
			{
				newSerializerContext.AppendLine($"{tabs}[JsonSerializable(typeof({type.GetFriendlyName()}[]))]");
			}

			newSerializerContext
				.AppendLine()
				.AppendLine(GenerateAttributesForSerializerContext(baseNamespace, tabIndex))
				.AppendLine($"\tinternal partial class {projectName}JsonSerializerContext : JsonSerializerContext")
				.AppendLine("\t{")
				.AppendLine("\t}")
				.AppendLine("}")
				.AppendLine("#pragma warning restore CS0618 // Type or member is obsolete");

			var currentSerializerContext = await File.ReadAllTextAsync(serializerContextPath, CancellationToken.None).ConfigureAwait(false);

			if (newSerializerContext.ToString() != currentSerializerContext)
			{
				await File.WriteAllTextAsync(serializerContextPath, newSerializerContext.ToString(), CancellationToken.None).ConfigureAwait(false);

				throw new Exception("The serializer context has been updated. You must restart the integration tests to ensure the recent changes take effect.");
			}
		}

		private static string GenerateAttributesForSerializerContext(string baseNamespace, int tabIndex = 0)
		{
			// Handy code to generate the 'JsonSerializable' attributes for JsonSerializerContext
			var allTypes = Assembly
				.GetAssembly(typeof(ZoomClient))
				.GetTypes()
				.Where(t => t.IsClass || t.IsEnum)
				.Where(t => !string.IsNullOrEmpty(t.Namespace))
				.Where(t => t.Namespace.StartsWith(baseNamespace))
				.Where(t => !t.IsGenericType)
				.Where(t => t.GetCustomAttribute<CompilerGeneratedAttribute>() == null);

			var typesInBaseNamespace = allTypes
				.Where(t => t.Namespace.Equals(baseNamespace))
				.Select(t => new
				{
					Type = t,
					JsonSerializeAttribute = $"[JsonSerializable(typeof({t.FullName}))]",
					JsonSerializeAttributeArray = $"[JsonSerializable(typeof({t.FullName}[]))]",
					JsonSerializeAttributeNullable = t.IsEnum ? $"[JsonSerializable(typeof({t.FullName}?))]" : string.Empty,
				});

			var typesInSubNamespace = allTypes
				.Where(t => !t.Namespace.Equals(baseNamespace))
				.Select(t => new
				{
					Type = t,
					JsonSerializeAttribute = $"[JsonSerializable(typeof({t.FullName}), TypeInfoPropertyName = \"{t.FullName[(baseNamespace.Length + 1)..].Replace(".", "")}\")]",
					JsonSerializeAttributeArray = $"[JsonSerializable(typeof({t.FullName}[]), TypeInfoPropertyName = \"{t.FullName[(baseNamespace.Length + 1)..].Replace(".", "")}Array\")]",
					JsonSerializeAttributeNullable = t.IsEnum ? $"[JsonSerializable(typeof({t.FullName}?), TypeInfoPropertyName = \"{t.FullName[(baseNamespace.Length + 1)..].Replace(".", "")}Nullable\")]" : string.Empty,
				});

			var typesSortedAlphabetically = typesInBaseNamespace.Union(typesInSubNamespace).OrderBy(t => t.Type.FullName);

			var tabs = string.Concat(Enumerable.Repeat("\t", tabIndex));
			var simpleAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttribute)).Select(t => tabs + t.JsonSerializeAttribute));
			var arrayAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttributeArray)).Select(t => tabs + t.JsonSerializeAttributeArray));
			var nullableAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttributeNullable)).Select(t => tabs + t.JsonSerializeAttributeNullable));

			var result = string.Join("\r\n\r\n", [simpleAttributes, arrayAttributes, nullableAttributes]);
			return result;
		}

		// From: https://stackoverflow.com/questions/47841441/how-do-i-get-the-path-to-the-current-c-sharp-source-code-file
		private static string GetThisFilePath([CallerFilePath] string path = null)
		{
			return path;
		}

		private static IConnectionInfo GetConnectionInfo(ConnectionType connectionType, TestType testType)
		{
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
			}
		}
	}
}
