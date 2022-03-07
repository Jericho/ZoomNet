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
			var totalMeetings = await client
				.Meetings.GetAllAsync(myUser.Id, MeetingListType.Scheduled, 30, null, cancellationToken)
				.ConfigureAwait(false);

			var pastInstances = new List<PastInstance>();

			foreach (var meeting in totalMeetings.Records)
			{
				var pastMeetingInstances = await client.PastMeetings.GetInstancesAsync(meeting.Id, cancellationToken);

				foreach (var instance in pastMeetingInstances)
				{
					if (instance.StartedOn < DateTime.UtcNow.AddDays(-1) && !instance.Uuid.StartsWith("/"))
					{
						pastInstances.Add(instance);
					}
				}
			}

			int totalParticipants = 0;

			foreach (var meeting in pastInstances)
			{
				var paginatedParticipants = await client.Reports.GetMeetingParticipantsAsync(meeting.Uuid, 30, null, cancellationToken);
				totalParticipants += paginatedParticipants.TotalRecords;
			}

			await log.WriteLineAsync($"There are {pastInstances.Count} past instances of meetings with a total of {totalParticipants} participants for this user.").ConfigureAwait(false);
		}
	}
}
