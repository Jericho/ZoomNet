using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Reports : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** REPORTS *****\n").ConfigureAwait(false);

			//GET ALL MEETINGS
			var totalMeetings = await client
				.Meetings.GetAllAsync(userId, MeetingListType.Scheduled, 30, 1, cancellationToken)
				.ConfigureAwait(false);

			var scheduledMeetings = totalMeetings.Records.OfType<ScheduledMeeting>()
				.Where(x => x.StartTime.AddMinutes(x.Duration) < DateTime.UtcNow.AddDays(-1));

			int totalParticipants = 0;

			foreach (var meeting in scheduledMeetings)
			{
				try
				{
					var paginatedParticipants = await client.Reports.GetMeetingParticipantsAsync(meeting.Id.ToString(), null, 30, null, cancellationToken);
					totalParticipants += paginatedParticipants.TotalRecords;
				}

				//Meeting doesn't exist
				catch (ZoomException e)
				{
					totalParticipants += 0;
				}
			}
			
			await log.WriteLineAsync($"There are {totalMeetings.TotalRecords} meetings with a total of {totalParticipants} participants for meeting this user.").ConfigureAwait(false);

		}
	}
}
