using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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

		public TestSuite(IConnectionInfo connectionInfo, IWebProxy proxy, ILoggerFactory loggerFactory, Type[] tests)
		{
			ConnectionInfo = connectionInfo;
			Proxy = proxy;
			LoggerFactory = loggerFactory;
			Tests = tests;
		}

		public virtual async Task<ResultCodes> RunTestsAsync()
		{
			// Configure cancellation
			var cts = new CancellationTokenSource();
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				cts.Cancel();
			};

			// Configure ZoomNet client
			var client = new ZoomClient(ConnectionInfo, Proxy, null, LoggerFactory.CreateLogger<ZoomClient>());

			// Get my user and permisisons
			var myUser = await client.Users.GetCurrentAsync(cts.Token).ConfigureAwait(false);
			var myPermissions = await client.Users.GetCurrentPermissionsAsync(cts.Token).ConfigureAwait(false);
			Array.Sort(myPermissions); // Sort permissions alphabetically for convenience

			// Execute the async tests in parallel (with max degree of parallelism)
			var results = await Tests.ForEachAsync(
				async testType =>
				{
					var log = new StringWriter();

					try
					{
						var integrationTest = (IIntegrationTest)Activator.CreateInstance(testType);
						await integrationTest.RunAsync(myUser, myPermissions, client, log, cts.Token).ConfigureAwait(false);
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

			// Prompt user to press a key in order to allow reading the log in the console
			var promptLog = new StringWriter();
			await promptLog.WriteLineAsync("\n\n**************************************************").ConfigureAwait(false);
			await promptLog.WriteLineAsync("Press any key to exit").ConfigureAwait(false);
			ConsoleUtils.Prompt(promptLog.ToString());

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
