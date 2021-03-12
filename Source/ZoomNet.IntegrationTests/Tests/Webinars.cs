using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Webinars : IIntegrationTest
	{
		public async Task RunAsync(string userId, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** WEBINARS *****\n").ConfigureAwait(false);

			// GET ALL THE WEBINARS
			var paginatedWebinars = await client.Webinars.GetAllAsync(userId, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedWebinars.TotalRecords} webinars for user {userId}").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedWebinars.Records
				.Union(paginatedWebinars.Records)
				.Where(m => m.Topic.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldWebinar =>
				{
					await client.Webinars.DeleteAsync(oldWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Webinar {oldWebinar.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			var settings = new WebinarSettings()
			{
				ApprovalType = MeetingApprovalType.Manual
			};
			var trackingFields = new Dictionary<string, string>()
			{
				{ "field1", "value1"},
				{ "field2", "value2"}
			};

			// Scheduled webinar
			var start = DateTime.UtcNow.AddMonths(1);
			var duration = 30;
			var newScheduledWebinar = await client.Webinars.CreateScheduledWebinarAsync(userId, "ZoomNet Integration Testing: scheduled webinar", "The agenda", start, duration, "UTC", "p@ss!w0rd", settings, trackingFields, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {newScheduledWebinar.Id} created").ConfigureAwait(false);

			await client.Webinars.DeleteAsync(newScheduledWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {newScheduledWebinar.Id} deleted").ConfigureAwait(false);

			// Recurring webinar
			var recurrenceInfo = new RecurrenceInfo()
			{
				EndTimes = 2,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Friday },
				Type = RecurrenceType.Weekly
			};
			var newRecurringWebinar = await client.Webinars.CreateRecurringWebinarAsync(userId, "ZoomNet Integration Testing: recurring webinar", "The agenda", start, duration, recurrenceInfo, "UTC", "p@ss!w0rd", settings, trackingFields, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} created").ConfigureAwait(false);

			await client.Webinars.DeleteAsync(newRecurringWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} deleted").ConfigureAwait(false);
		}
	}
}
