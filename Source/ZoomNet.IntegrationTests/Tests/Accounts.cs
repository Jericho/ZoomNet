using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Accounts : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			await log.WriteLineAsync("\n***** ACCOUNTS *****\n").ConfigureAwait(false);

			if (client.HasPermission("account:read:list_sub_accounts:master"))
			{
				// GET ALL THE ACCOUNTS
				var paginatedAccounts = await client.Accounts.GetAllAsync(100, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"There are {paginatedAccounts.TotalRecords} sub accounts under the main account").ConfigureAwait(false);

				// GET SETTINGS
				if (paginatedAccounts.Records.Length > 0)
				{
					var accountId = paginatedAccounts.Records[0].Id;

					var meetingAuthenticationSettings = await client.Accounts.GetMeetingAuthenticationSettingsAsync(accountId, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync("Meeting authentication settings retrieved").ConfigureAwait(false);

					var recordingAuthenticationSettings = await client.Accounts.GetRecordingAuthenticationSettingsAsync(accountId, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync("Recording authentication settings retrieved").ConfigureAwait(false);
				}
			}

			/*
			 Commenting out because, despite the fact that the account has the permission, the API returns a HTTP 400 with the following payload:
			{"code":4711,"message":"Invalid access token, does not contain scopes:[account:write:virtual_background_files:master]."}

			I think the message is misleading. The issue is not so much that my S2S OAuth app doesn't have to necessary scope but rather that this
			endpoint is restricted to master account (i.e.: account with sub-accounts). Just a guess on my part though.
			
			// UPLOAD A BACKGROUND FILE
			if (client.HasPermission("account:write:virtual_background_files:master"))
			{
				var (fileStream, fileName) = Utils.GetRandomImage();
				var virtualBackground = await client.Accounts.UploadVirtualBackgroundFileAsync(myUser.AccountId, fileName, fileStream, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"File {virtualBackground.Name} uploaded").ConfigureAwait(false);
			}
			*/
		}
	}
}
