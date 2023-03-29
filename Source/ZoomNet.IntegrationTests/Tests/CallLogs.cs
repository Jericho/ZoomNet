using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class CallLogs : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CALL LOGS *****\n").ConfigureAwait(false);

			var now = DateTime.UtcNow;
			var from = now.Subtract(TimeSpan.FromDays(30));
			var to = now;

			// GET USER'S CALL LOGS
			var allCallLogs = await client.CallLogs.GetAsync(myUser.Email, from, to, CallLogType.All, null, 100, null, cancellationToken);
			var missedCalllogs = await client.CallLogs.GetAsync(myUser.Email, from, to, CallLogType.Missed, null, 100, null, cancellationToken);
			await log.WriteLineAsync($"All call Logs: {allCallLogs.Records.Length}. Missed call logs: {missedCalllogs.Records.Length}").ConfigureAwait(false);
		}
	}
}
