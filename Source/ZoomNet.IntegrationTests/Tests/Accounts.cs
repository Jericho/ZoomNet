using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Accounts : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** ACCOUNTS *****\n").ConfigureAwait(false);

			// GET ALL THE ACCOUNTS
			var paginatedAccounts = await client.Accounts.GetAllAsync(100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedAccounts.TotalRecords} sub accounts under the main account").ConfigureAwait(false);

			// GET SETTINGS
			if (paginatedAccounts.Records.Any())
			{
				var accountId = paginatedAccounts.Records.First().Id;

				var meetingAuthenticationSettings = await client.Accounts.GetMeetingAuthenticationSettingsAsync(accountId, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Meeting authentication settings retrieved").ConfigureAwait(false);

				var recordingAuthenticationSettings = await client.Accounts.GetRecordingAuthenticationSettingsAsync(accountId, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Recording authentication settings retrieved").ConfigureAwait(false);
			}
		}
	}
}
