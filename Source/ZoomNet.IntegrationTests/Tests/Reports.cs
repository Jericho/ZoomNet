using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Reports : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** REPORTS *****\n").ConfigureAwait(false);

			var now = DateTime.UtcNow;
			var from = now.Subtract(TimeSpan.FromDays(30));
			var to = now;

			// GET ALL HOSTS
			var activeHosts = await client.Reports.GetHostsAsync(from, to, ReportHostType.Active, 30, null, cancellationToken);
			var inactiveHosts = await client.Reports.GetHostsAsync(from, to, ReportHostType.Inactive, 30, null, cancellationToken);
			await log.WriteLineAsync($"Active Hosts: {activeHosts.Records.Length}. Inactive Hosts: {inactiveHosts.Records.Length}").ConfigureAwait(false);

			// GET ALL MEETINGS
			var pastMeetings = await client.Reports.GetMeetingsAsync(myUser.Id, from, to, ReportMeetingType.Past, 30, null, cancellationToken);

			int totalParticipants = 0;

			foreach (var meeting in pastMeetings.Records)
			{
				var paginatedParticipants = await client.Reports.GetMeetingParticipantsAsync(meeting.Uuid, 30, null, cancellationToken);
				totalParticipants += paginatedParticipants.TotalRecords;
			}

			await log.WriteLineAsync($"There are {pastMeetings.Records.Length} past instances of meetings with a total of {totalParticipants} participants for this user.").ConfigureAwait(false);
		}
	}
}
