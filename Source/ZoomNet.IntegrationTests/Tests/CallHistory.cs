using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class CallHistory : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			await log.WriteLineAsync("\n***** CALL HISTORY *****\n").ConfigureAwait(false);

			var from = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));
			var to = DateTime.UtcNow;

			var accountCallHistory = await client.CallHistory.GetAccountCallHistoryAsync(from, to, cancellationToken: cancellationToken);
			await log.WriteLineAsync($"There are {accountCallHistory.TotalRecords} items in the account call history").ConfigureAwait(false);

			if (accountCallHistory.Records.Any())
			{
				var firstCallElement = await client.CallHistory.GetCallElementAsync(accountCallHistory.Records[0].Id, cancellationToken: cancellationToken);
				await log.WriteLineAsync($"The first call element was betwee {firstCallElement.CallerName} and {firstCallElement.CalleeName}").ConfigureAwait(false);
			}
		}
	}
}
