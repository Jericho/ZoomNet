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
			//===========================================================================
			// This is for debuggin purposes, while investigating this issue: https://devforum.zoom.us/t/getting-meeting-transcript-info-works-with-uuid-but-not-with-meeting-id/138985

			// Fetching transcript info using meeting UUID works as expected
			var bbb = await client.CloudRecordings.GetTranscriptInfoAsync("j3xXxYijTjigTGf4rPci%2Fw%3D%3D", cancellationToken).ConfigureAwait(false);
			if (bbb.DownloadRestrictionReason == TranscriptRestrictionReason.Available) await log.WriteLineAsync($"The meeting transcript info is available here: {bbb.DownloadUrl}").ConfigureAwait(false);
			else await log.WriteLineAsync($"The meeting transcript info is not available because: {bbb.DownloadRestrictionReason.ToEnumString()}").ConfigureAwait(false);

			// According to documentation, using the metingId should work too, but it does not
			bbb = await client.CloudRecordings.GetTranscriptInfoAsync(83584776469, cancellationToken).ConfigureAwait(false);
			if (bbb.DownloadRestrictionReason == TranscriptRestrictionReason.Available) await log.WriteLineAsync($"The meeting transcript info is available here: {bbb.DownloadUrl}").ConfigureAwait(false);
			else await log.WriteLineAsync($"The meeting transcript info is not available because: {bbb.DownloadRestrictionReason.ToEnumString()}").ConfigureAwait(false);
			//===========================================================================



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
