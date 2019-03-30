using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Logging;
using ZoomNet.Utilities;

namespace ZoomNet.IntegrationTests
{
	class Program
	{
		private const int MAX_ZOOM_API_CONCURRENCY = 5;

		private enum ResultCodes
		{
			Success = 0,
			Exception = 1,
			Cancelled = 1223
		}

		static async Task<int> Main()
		{
			// -----------------------------------------------------------------------------
			// Do you want to proxy requests through Fiddler (useful for debugging)?
			var useFiddler = true;

			// As an alternative to Fiddler, you can display debug information about
			// every HTTP request/response in the console. This is useful for debugging
			// purposes but the amount of information can be overwhelming.
			var debugHttpMessagesToConsole = false;
			// -----------------------------------------------------------------------------

			var apiKey = Environment.GetEnvironmentVariable("ZOOM_APIKEY");
			var apiSecret = Environment.GetEnvironmentVariable("ZOOM_APISECRET");
			var userId = Environment.GetEnvironmentVariable("ZOOM_USERID");
			var client = useFiddler ? new Client(apiKey, apiSecret, new WebProxy("http://localhost:8888")) : new Client(apiKey, apiSecret);

			if (debugHttpMessagesToConsole)
			{
				LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
			}

			var source = new CancellationTokenSource();
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				source.Cancel();
			};

			// Ensure the Console is tall enough and centered on the screen
			Console.WindowHeight = Math.Min(60, Console.LargestWindowHeight);
			ConsoleUtils.CenterConsole();

			// These are the integration tests that we will execute
			var integrationTests = new Func<string, IClient, TextWriter, CancellationToken, Task>[]
			{
			};


			// Execute the async tests in parallel (with max degree of parallelism)
			var results = await integrationTests.ForEachAsync(
				async integrationTest =>
				{
					var log = new StringWriter();

					try
					{
						await integrationTest(userId, client, log, source.Token).ConfigureAwait(false);
						return (TestName: integrationTest.Method.Name, ResultCode: ResultCodes.Success, Message: string.Empty);
					}
					catch (OperationCanceledException)
					{
						await log.WriteLineAsync($"-----> TASK CANCELLED").ConfigureAwait(false);
						return (TestName: integrationTest.Method.Name, ResultCode: ResultCodes.Cancelled, Message: "Task cancelled");
					}
					catch (Exception e)
					{
						var exceptionMessage = e.GetBaseException().Message;
						await log.WriteLineAsync($"-----> AN EXCEPTION OCCURED: {exceptionMessage}").ConfigureAwait(false);
						return (TestName: integrationTest.Method.Name, ResultCode: ResultCodes.Exception, Message: exceptionMessage);
					}
					finally
					{
						await Console.Out.WriteLineAsync(log.ToString()).ConfigureAwait(false);
					}
				}, MAX_ZOOM_API_CONCURRENCY)
			.ConfigureAwait(false);

			// Display summary
			var summary = new StringWriter();
			await summary.WriteLineAsync("\n\n**************************************************").ConfigureAwait(false);
			await summary.WriteLineAsync("******************** SUMMARY *********************").ConfigureAwait(false);
			await summary.WriteLineAsync("**************************************************").ConfigureAwait(false);

			var resultsWithMessage = results
				.Where(r => !string.IsNullOrEmpty(r.Message))
				.ToArray();

			if (resultsWithMessage.Any())
			{
				foreach (var (TestName, ResultCode, Message) in resultsWithMessage)
				{
					const int TEST_NAME_MAX_LENGTH = 25;
					var name = TestName.Length <= TEST_NAME_MAX_LENGTH ? TestName : TestName.Substring(0, TEST_NAME_MAX_LENGTH - 3) + "...";
					await summary.WriteLineAsync($"{name.PadRight(TEST_NAME_MAX_LENGTH, ' ')} : {Message}").ConfigureAwait(false);
				}
			}
			else
			{
				await summary.WriteLineAsync("All tests completed succesfully").ConfigureAwait(false);
			}

			await summary.WriteLineAsync("**************************************************").ConfigureAwait(false);
			await Console.Out.WriteLineAsync(summary.ToString()).ConfigureAwait(false);

			// Prompt user to press a key in order to allow reading the log in the console
			var promptLog = new StringWriter();
			await promptLog.WriteLineAsync("\n\n**************************************************").ConfigureAwait(false);
			await promptLog.WriteLineAsync("Press any key to exit").ConfigureAwait(false);
			Prompt(promptLog.ToString());

			// Return code indicating success/failure
			var resultCode = (int)ResultCodes.Success;
			if (results.Any(result => result.ResultCode != ResultCodes.Success))
			{
				if (results.Any(result => result.ResultCode == ResultCodes.Exception)) resultCode = (int)ResultCodes.Exception;
				else if (results.Any(result => result.ResultCode == ResultCodes.Cancelled)) resultCode = (int)ResultCodes.Cancelled;
				else resultCode = (int)results.First(result => result.ResultCode != ResultCodes.Success).ResultCode;
			}

			return await Task.FromResult(resultCode);
		}

		private static char Prompt(string prompt)
		{
			while (Console.KeyAvailable)
			{
				Console.ReadKey(false);
			}
			Console.Out.WriteLine(prompt);
			var result = Console.ReadKey();
			return result.KeyChar;
		}
	}
}
