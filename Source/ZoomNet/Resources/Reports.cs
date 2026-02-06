using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Reports : IReports
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Reports" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Reports(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ReportParticipant>> GetMeetingParticipantsAsync(string meetingId, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"report/meetings/{Utils.EncodeUUID(meetingId)}/participants")
				.WithArgument("include_fields", "registrant_id")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ReportParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<PastMeeting>> GetMeetingsAsync(string userId, DateOnly from, DateOnly to, ReportMeetingType type, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			VerifyReportDatesRange(from, to);
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"report/users/{userId}/meetings")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<PastMeeting>("meetings");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ReportParticipant>> GetWebinarParticipantsAsync(string webinarId, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				   .GetAsync($"report/webinars/{Utils.EncodeUUID(webinarId)}/participants")
				   .WithArgument("include_fields", "registrant_id")
				   .WithArgument("page_size", recordsPerPage)
				   .WithArgument("next_page_token", pageToken)
				   .WithCancellationToken(cancellationToken)
				   .AsPaginatedResponseWithToken<ReportParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ReportHost>> GetHostsAsync(DateOnly from, DateOnly to, ReportHostType type = ReportHostType.Active, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			VerifyReportDatesRange(from, to);
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("report/users")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ReportHost>("users");
		}

		/// <inheritdoc/>
		public Task<DailyUsageReport> GetDailyUsageReportAsync(int year, int month, string groupId = null, CancellationToken cancellationToken = default)
		{
			return _client.GetAsync("report/daily")
				.WithArgument("year", year)
				.WithArgument("month", month)
				.WithArgument("groupId", groupId)
				.WithCancellationToken(cancellationToken)
				.AsObject<DailyUsageReport>();
		}

		private static void VerifyReportDatesRange(DateOnly from, DateOnly to)
		{
			if (to < from)
			{
				throw new ArgumentOutOfRangeException(nameof(to), $"Should be greater than or equal to {from.ToZoomFormat()}.");
			}

			if (to.DayNumber - from.DayNumber > 30)
			{
				throw new ArgumentOutOfRangeException(nameof(to), "The date range should not exceed one month.");
			}
		}
	}
}
