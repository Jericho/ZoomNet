using System;
using System.Collections.Generic;
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

			//GET ALL MEETINGS
			var now = DateTime.UtcNow;
			var pastMeetings = await client.Reports.GetMeetingsAsync(myUser.Id, now.Subtract(TimeSpan.FromDays(30)), now, ReportMeetingType.Past, 30, null, cancellationToken);

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
