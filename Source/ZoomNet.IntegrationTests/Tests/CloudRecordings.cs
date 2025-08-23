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
			await log.WriteLineAsync("\n***** CLOUD RECORDINGS *****\n").ConfigureAwait(false);

			// GET ALL THE RECORDINGS FOR A GIVEN USER
			var paginatedRecordings = await client.CloudRecordings.GetRecordingsForUserAsync(myUser.Id, false, null, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {myUser.Id} has {paginatedRecordings.TotalRecords} recordings stored in the cloud").ConfigureAwait(false);

			// GROUP THE RECORDINGS BY MEETING
			var recordingFilesGroupedByMeeting = paginatedRecordings.Records
				.SelectMany(record => record.RecordingFiles)
				.GroupBy(recordingFile => recordingFile.MeetingId);

			// DOWNLOAD THE FILES
			foreach (var group in recordingFilesGroupedByMeeting)
			{
				const int ttl = 60 * 5; // 5 minutes
				var recordingInfo = await client.CloudRecordings.GetRecordingInformationAsync(group.Key, ttl, cancellationToken).ConfigureAwait(false);

				foreach (var recordingFile in group)
				{
					var stream = await client.CloudRecordings.DownloadFileAsync(recordingFile.DownloadUrl, recordingInfo.DownloadAccessToken, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Downloaded {recordingFile.DownloadUrl}").ConfigureAwait(false);
				}

			}
		}
	}
}
