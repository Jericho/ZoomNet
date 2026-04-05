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

			// Get the unique meeting Ids
			var meetingIds = paginatedRecordings.Records
				.SelectMany(record => record.RecordingFiles)
				.Select(file => file.MeetingId)
				.Distinct();

			var b = await client.CloudRecordings.GetParsedTranscriptAsync("pFx7uR3%2BT5yGDjps0dnPWA%3D%3D", cancellationToken).ConfigureAwait(false);

			// DOWNLOAD THE FILES
			foreach (var meetingId in meetingIds)
			{
				var files = await client.CloudRecordings.DownloadAllMeetingRecordingFilesAsync(meetingId, false, 5, cancellationToken).ConfigureAwait(false);

				await log.WriteLineAsync($"Meeting {meetingId}").ConfigureAwait(false);
				foreach (var file in files)
				{
					await log.WriteLineAsync($"    - Downloaded {file.Info.FileType}").ConfigureAwait(false);
				}

			}
		}
	}
}
