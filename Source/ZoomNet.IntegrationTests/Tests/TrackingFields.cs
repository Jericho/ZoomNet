using System.IO;
using System.Linq;
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

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = trackingFields
				.Where(tf => tf.Name.StartsWith("ZoomNet Integration Testing:"))
				.Select(async tf =>
				{
					await client.TrackingFields.DeleteAsync(tf.Id, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Tracking field {tf.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// CREATE A TRACKING FIELD
			var trackingField = await client.TrackingFields.CreateAsync("ZoomNet Integration Testing: My Tracking Field", new[] { "Value 1", "Value 2" }, false, true, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Tracking field {trackingField.Id} created").ConfigureAwait(false);

			await client.TrackingFields.UpdateAsync(trackingField.Id, name: "ZoomNet Integration Testing: UPDATED Tracking Field", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Tracking field {trackingField.Id} updated").ConfigureAwait(false);

			trackingField = await client.TrackingFields.GetAsync(trackingField.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Tracking field {trackingField.Id} retrieved").ConfigureAwait(false);

			await client.TrackingFields.DeleteAsync(trackingField.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Tracking field {trackingField.Id} deleted").ConfigureAwait(false);
		}
	}
}
