using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class CloudRecordings : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CLOUD RECORDINGS *****\n").ConfigureAwait(false);

			// GET ALL THE RECORDINGS FOR A GIVEN USER
			var paginatedRecordings = await client.CloudRecordings.GetRecordingsForUserAsync(myUser.Id, false, null, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {myUser.Id} has {paginatedRecordings.TotalRecords} recordings stored in the cloud").ConfigureAwait(false);

			// DOWNLOAD THE FILES
			foreach (var recordingFile in paginatedRecordings.Records.SelectMany(record => record.RecordingFiles))
			{
				var stream = await client.CloudRecordings.DownloadFileAsync(recordingFile, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Downloaded {recordingFile.DownloadUrl}").ConfigureAwait(false);
			}
		}
	}
}
