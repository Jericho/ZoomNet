using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class TrackingFields : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** TRACKING FIELDS *****\n").ConfigureAwait(false);

			// GET ALL THE TRACKING FIELDS
			var trackingFields = await client.TrackingFields.GetAllAsync(cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {trackingFields.Length} tracking fields").ConfigureAwait(false);
		}
	}
}
