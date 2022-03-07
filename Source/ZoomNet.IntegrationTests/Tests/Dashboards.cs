using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Dashboards : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** DASHBOARDS *****\n").ConfigureAwait(false);

			// GET ALL THE MEETINGS
			var pastMeetingsStats = await client.Dashboards.GetAllMeetingsAsync(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, DashboardMeetingType.Past, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {pastMeetingsStats.TotalRecords} meetings in the last month").ConfigureAwait(false);
		}
	}
}
