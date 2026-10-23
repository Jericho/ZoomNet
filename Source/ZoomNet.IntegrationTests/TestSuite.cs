using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests
{
	internal abstract class TestSuite
	{
		private const int MAX_ZOOM_API_CONCURRENCY = 5;
		private const int TEST_NAME_MAX_LENGTH = 25;

		private const string SUCCESSFUL_TEST_MESSAGE = "Completed successfully";
		private const string TASK_CANCELLED = "Task cancelled";
		private const string SKIPPED_DUE_TO_CANCELATION = "Skipped due to cancelation";

		public ILoggerFactory LoggerFactory { get; init; }

		public IZoomClient Client { get; init; }

		public Type[] Tests { get; init; }

		public bool FetchCurrentUserInfo { get; init; }

		public TestSuite(IZoomClient client, ILoggerFactory loggerFactory, Type[] tests, bool fetchCurrentUserInfo)
		{
			Client = client;
			LoggerFactory = loggerFactory;
			Tests = tests;
			FetchCurrentUserInfo = fetchCurrentUserInfo;
		}

		public virtual async Task<ResultCodes> RunTestsAsync(CancellationToken cancellationToken)
		{
			// Get my user and permisisons
			User currentUser = null;
			string[] currentUserPermissions = Array.Empty<string>();

			if (FetchCurrentUserInfo)
			{
				await Console.Out.WriteLineAsync("Fetching user info...").ConfigureAwait(false);

				currentUser = await Client.Users.GetCurrentAsync(cancellationToken).ConfigureAwait(false);
				currentUserPermissions = await Client.Users.GetCurrentPermissionsAsync(cancellationToken).ConfigureAwait(false);
				Array.Sort(currentUserPermissions); // Sort permissions alphabetically for convenience

				await Console.Out.WriteLineAsync("User info fetched.").ConfigureAwait(false);
			}

			// Execute the async tests in parallel (with max degree of parallelism)
			var results = await Tests.ForEachAsync(
				async testType =>
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return (TestName: testType.Name, ResultCode: ResultCodes.Skipped, Message: SKIPPED_DUE_TO_CANCELATION);
					}

					var lineNumber = -1;
					lock (Console.Out)
					{
						Console.Out.WriteLine($"RUNNING INTEGRATION TEST: {testType.Name}...");
						lineNumber = Console.CursorTop - 1;
					}

					var log = new StringWriter();

					try
					{
						var integrationTest = (IIntegrationTest)Activator.CreateInstance(testType);
						await integrationTest.RunAsync(currentUser, currentUserPermissions, Client, log, cancellationToken).ConfigureAwait(false);
						return (TestName: testType.Name, ResultCode: ResultCodes.Success, Message: SUCCESSFUL_TEST_MESSAGE);
					}
					catch (OperationCanceledException)
					{
						await log.WriteLineAsync($"-----> TASK CANCELLED").ConfigureAwait(false);
						return (TestName: testType.Name, ResultCode: ResultCodes.Cancelled, Message: TASK_CANCELLED);
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
							ClearConsoleLine(lineNumber);
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

		// Source - https://stackoverflow.com/a/8946847
		// Posted by dknaack, modified by community. See post 'Timeline' for change history
		// Retrieved 2026-04-17, License - CC BY-SA 4.0
		private static void ClearCurrentConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}

		private static void ClearConsoleLine(int lineNumber)
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, lineNumber);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}
	}
}
