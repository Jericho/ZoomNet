using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.IntegrationTests
{
	internal abstract class TestSuite
	{
		private const int MAX_ZOOM_API_CONCURRENCY = 5;
		private const int TEST_NAME_MAX_LENGTH = 25;
		private const string SUCCESSFUL_TEST_MESSAGE = "Completed successfully";

		public ILoggerFactory LoggerFactory { get; init; }

		public IConnectionInfo ConnectionInfo { get; init; }

		public IWebProxy Proxy { get; init; }

		public Type[] Tests { get; init; }

		public bool FetchCurrentUserInfo { get; init; }

		public TestSuite(IConnectionInfo connectionInfo, IWebProxy proxy, ILoggerFactory loggerFactory, Type[] tests, bool fetchCurrentUserInfo)
		{
			ConnectionInfo = connectionInfo;
			Proxy = proxy;
			LoggerFactory = loggerFactory;
			Tests = tests;
			FetchCurrentUserInfo = fetchCurrentUserInfo;
		}

		public virtual async Task<ResultCodes> RunTestsAsync(CancellationToken cancellationToken)
		{
			// Configure ZoomNet client
			var options = new ZoomClientOptions
			{
				// A successful API call will trigger a 'Trace' log (rather than the default 'Debug').
				// This is to ensure that we don't get overwhelmed by too many debug messages in the console.
				LogLevelSuccessfulCalls = LogLevel.Trace
			};
			var client = new ZoomClient(ConnectionInfo, Proxy, options, LoggerFactory.CreateLogger<ZoomClient>());

			// Get my user and permisisons
			User currentUser = null;
			string[] currentUserPermissions = Array.Empty<string>();

			if (FetchCurrentUserInfo)
			{
				currentUser = await client.Users.GetCurrentAsync(cancellationToken).ConfigureAwait(false);
				currentUserPermissions = await client.Users.GetCurrentPermissionsAsync(cancellationToken).ConfigureAwait(false);
				Array.Sort(currentUserPermissions); // Sort permissions alphabetically for convenience
			}

			// Execute the async tests in parallel (with max degree of parallelism)
			var results = await Tests.ForEachAsync(
				async testType =>
				{
					var log = new StringWriter();

					try
					{
						var integrationTest = (IIntegrationTest)Activator.CreateInstance(testType);
						await integrationTest.RunAsync(currentUser, currentUserPermissions, client, log, cancellationToken).ConfigureAwait(false);
						return (TestName: testType.Name, ResultCode: ResultCodes.Success, Message: SUCCESSFUL_TEST_MESSAGE);
					}
					catch (OperationCanceledException)
					{
						await log.WriteLineAsync($"-----> TASK CANCELLED").ConfigureAwait(false);
						return (TestName: testType.Name, ResultCode: ResultCodes.Cancelled, Message: "Task cancelled");
					}
					catch (Exception e)
					{
						var exceptionMessage = e.GetBaseException().Message;
						await log.WriteLineAsync($"-----> AN EXCEPTION OCCURRED: {exceptionMessage}").ConfigureAwait(false);
						return (TestName: testType.Name, ResultCode: ResultCodes.Exception, Message: exceptionMessage);
					}
					finally
					{
						lock (Console.Out)
						{
							Console.Out.WriteLine(log.ToString());
						}
					}
				}, MAX_ZOOM_API_CONCURRENCY)
			.ConfigureAwait(false);

			// Display summary
			var summary = new StringWriter();
			await summary.WriteLineAsync("\n\n**************************************************").ConfigureAwait(false);
			await summary.WriteLineAsync("******************** SUMMARY *********************").ConfigureAwait(false);
			await summary.WriteLineAsync("**************************************************").ConfigureAwait(false);

			var nameMaxLength = Math.Min(results.Max(r => r.TestName.Length), TEST_NAME_MAX_LENGTH);
			foreach (var (TestName, ResultCode, Message) in results.OrderBy(r => r.TestName).ToArray())
			{
				await summary.WriteLineAsync($"{TestName.ToExactLength(nameMaxLength)} : {Message}").ConfigureAwait(false);
			}

			await summary.WriteLineAsync("**************************************************").ConfigureAwait(false);
			await Console.Out.WriteLineAsync(summary.ToString()).ConfigureAwait(false);

			// Return code indicating success/failure
			var resultCode = ResultCodes.Success;
			if (results.Any(result => result.ResultCode != ResultCodes.Success))
			{
				if (results.Any(result => result.ResultCode == ResultCodes.Exception)) return ResultCodes.Exception;
				else if (results.Any(result => result.ResultCode == ResultCodes.Cancelled)) resultCode = ResultCodes.Cancelled;
				else resultCode = results.First(result => result.ResultCode != ResultCodes.Success).ResultCode;
			}

			return resultCode;
		}
	}
}
