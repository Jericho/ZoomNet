using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests.Tests
{
	public class CloudRecordings : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** CLOUD RECORDINGS *****\n").ConfigureAwait(false);

			// GET ALL THE RECORDINGS FOR A GIVEN USER
			var paginatedRecordings = await client.CloudRecordings.GetRecordingsForUserAsync(userId, false, null, null, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"User {userId} has {paginatedRecordings.TotalRecords} recordings stored in the cloud").ConfigureAwait(false);
		}
	}
}
