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
			await log.WriteLineAsync("\n***** REPORTS *****\n").ConfigureAwait(false);

			var now = DateTime.UtcNow;
			var from = DateOnly.FromDateTime(now.Subtract(TimeSpan.FromDays(30)));
			var to = DateOnly.FromDateTime(now);

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

			// GET ALL THE WEBINARS
			var pastWebinars = await client.Webinars.GetAllAsync(myUser.Id, 30, null, cancellationToken).ConfigureAwait(false);

			totalParticipants = 0;
			foreach (var webinar in pastWebinars.Records)
			{
				var paginatedParticipants = await client.Reports.GetWebinarParticipantsAsync(webinar.Uuid, 30, null, cancellationToken);
				totalParticipants += paginatedParticipants.TotalRecords;
			}

			await log.WriteLineAsync($"There are {pastWebinars.Records.Length} past instances of webinar with a total of {totalParticipants} participants for this user.").ConfigureAwait(false);
		}
	}
}
