using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Meetings : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
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
			var cleanUpTasks = paginatedScheduledMeetings.Records
				.Union(paginatedLiveMeetings.Records)
				.Where(m => m.Topic.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldMeeting =>
				{
					await client.Meetings.DeleteAsync(userId, oldMeeting.Id, null, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Meeting {oldMeeting.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			var settings = new MeetingSettings()
			{
				ApprovalType = MeetingApprovalType.Manual
			};
			var trackingFields = new Dictionary<string, string>()
			{
				{ "field1", "value1"},
				{ "field2", "value2"}
			};

			// Instant meeting
			var newInstantMeeting = await client.Meetings.CreateInstantMeetingAsync(userId, "ZoomNet Integration Testing: instant meeting", "The agenda", "p@ss!w0rd", settings, trackingFields, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} created").ConfigureAwait(false);

			await client.Meetings.EndAsync(newInstantMeeting.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} ended").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(userId, newInstantMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} deleted").ConfigureAwait(false);

			// Scheduled meeting
			var start = DateTime.UtcNow.AddMonths(1);
			var duration = 30;
			var newScheduledMeeting = await client.Meetings.CreateScheduledMeetingAsync(userId, "ZoomNet Integration Testing: scheduled meeting", "The agenda", start, duration, "p@ss!w0rd", settings, trackingFields, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {newScheduledMeeting.Id} created").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(userId, newScheduledMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {newScheduledMeeting.Id} deleted").ConfigureAwait(false);

			// Recurring meeting
			var recurrenceInfo = new RecurrenceInfo()
			{
				EndTimes = 2,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Friday },
				Type = RecurrenceType.Weekly
			};
			var newRecurringMeeting = await client.Meetings.CreateRecurringMeetingAsync(userId, "ZoomNet Integration Testing: recurring meeting", "The agenda", start, duration, recurrenceInfo, "p@ss!w0rd", settings, trackingFields, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} created").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(userId, newRecurringMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} deleted").ConfigureAwait(false);
		}
	}
}
