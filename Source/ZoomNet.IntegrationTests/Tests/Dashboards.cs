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
			await log.WriteLineAsync("\n***** DASHBOARDS *****\n").ConfigureAwait(false);

			var now = DateTime.UtcNow;
			var from = DateOnly.FromDateTime(now.AddMonths(-1));
			var to = DateOnly.FromDateTime(now);

			// GET ALL THE MEETINGS
			var pastMeetingsStats = await client.Dashboards.GetAllMeetingsAsync(from, to, DashboardMeetingType.Past, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {pastMeetingsStats.TotalRecords} meetings in the last month").ConfigureAwait(false);
		}
	}
}
