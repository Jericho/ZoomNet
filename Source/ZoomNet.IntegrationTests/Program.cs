using Logzio.DotNet.NLog;
using Microsoft.Extensions.DependencyInjection;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests
{
	public class Program
	{
		public static async Task<int> Main(string[] args)
		{
			/*
			 * Handy code to generate the 'JsonSerializable' attributes for ZoomNetJsonSerializerContext
			 *
			var baseNamespace = "ZoomNet.Models";
			var allTypes = System.Reflection.Assembly
				.GetAssembly(typeof(ZoomClient))
				.GetTypes()
				.Where(t => t.IsClass || t.IsEnum)
				.Where(t => !string.IsNullOrEmpty(t.Namespace))
				.Where(t => t.Namespace.StartsWith(baseNamespace));

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
					JsonSerializeAttribute = $"[JsonSerializable(typeof({t.FullName}), TypeInfoPropertyName = \"{t.FullName.Remove(0, baseNamespace.Length + 1).Replace(".", "")}\")]",
					JsonSerializeAttributeArray = $"[JsonSerializable(typeof({t.FullName}[]), TypeInfoPropertyName = \"{t.FullName.Remove(0, baseNamespace.Length + 1).Replace(".", "")}Array\")]",
					JsonSerializeAttributeNullable = t.IsEnum ? $"[JsonSerializable(typeof({t.FullName}?), TypeInfoPropertyName = \"{t.FullName.Remove(0, baseNamespace.Length + 1).Replace(".", "")}Nullable\")]" : string.Empty,
				});

			var typesSortedAlphabetically = typesInBaseNamespace.Union(typesInSubNamespace).OrderBy(t => t.Type.FullName);

			var simpleAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttribute)).Select(t => t.JsonSerializeAttribute));
			var arrayAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttributeArray)).Select(t => t.JsonSerializeAttributeArray));
			var nullableAttributes = string.Join("\r\n", typesSortedAlphabetically.Where(t => !string.IsNullOrEmpty(t.JsonSerializeAttributeNullable)).Select(t => t.JsonSerializeAttributeNullable));

			var result = string.Join("\r\n\r\n", new[] { simpleAttributes, arrayAttributes, nullableAttributes });
			*/

			var services = new ServiceCollection();
			ConfigureServices(services);
			await using var serviceProvider = services.BuildServiceProvider();
			var app = serviceProvider.GetService<TestsRunner>();
			return await app.RunAsync().ConfigureAwait(false);
		}

		private static void ConfigureServices(ServiceCollection services)
		{
			services
				.AddLogging(loggingBuilder => loggingBuilder.AddNLog(GetNLogConfiguration()))
				.AddTransient<TestsRunner>();
		}

		private static LoggingConfiguration GetNLogConfiguration()
		{
			// Configure logging
			var nLogConfig = new LoggingConfiguration();

			// Send logs to logz.io
			var logzioToken = Environment.GetEnvironmentVariable("LOGZIO_TOKEN");
			if (!string.IsNullOrEmpty(logzioToken))
			{
				var logzioTarget = new LogzioTarget { Token = logzioToken };
				logzioTarget.ContextProperties.Add(new TargetPropertyWithContext("source", "ZoomNet_integration_tests"));
				logzioTarget.ContextProperties.Add(new TargetPropertyWithContext("ZoomNet-Version", ZoomNet.ZoomClient.Version));

				nLogConfig.AddTarget("Logzio", logzioTarget);
				nLogConfig.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, "Logzio", "*");
			}

			// Send logs to console
			var consoleTarget = new ColoredConsoleTarget();
			nLogConfig.AddTarget("ColoredConsole", consoleTarget);
			nLogConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, "ColoredConsole", "*");

			return nLogConfig;
		}
	}
}
