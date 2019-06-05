using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Meetings : IIntegrationTest
	{
		public async Task RunAsync(IClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** MEETINGS *****\n").ConfigureAwait(false);

			// GET ALL THE MEETINGS
			var paginatedScheduledMeetings = await client.Meetings.GetAllAsync(userId, MeetingListType.Scheduled, 100, 1, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedScheduledMeetings.TotalRecords} scheduled meetings").ConfigureAwait(false);

			var paginatedLiveMeetings = await client.Meetings.GetAllAsync(userId, MeetingListType.Live, 100, 1, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedLiveMeetings.TotalRecords} live meetings").ConfigureAwait(false);

			var paginatedUpcomingMeetings = await client.Meetings.GetAllAsync(userId, MeetingListType.Upcoming, 100, 1, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedUpcomingMeetings.TotalRecords} upcoming meetings").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			//var cleanUpTasks = paginatedScheduledMeetings.Records
			//    .Where(m => m.Name.StartsWith("ZoomNet Integration Testing:"))
			//    .Select(async oldMeeting =>
			//    {
			//        await client.Meetings.DeleteAsync(userId, oldAccount.Id, null, cancellationToken).ConfigureAwait(false);
			//        await log.WriteLineAsync($"Meeting {oldMeeting.Id} deleted").ConfigureAwait(false);
			//        await Task.Delay(250).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
			//    });
			//await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);
		}
	}
}
