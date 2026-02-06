using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to view various metrics.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/reports/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IReports
	{
		/// <summary>
		/// Get a list of participants from past meetings with two or more participants. To see a list of participants for meetings with one participant use <see cref="IDashboards.GetMeetingParticipantsAsync"/>.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ReportParticipant">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ReportParticipant>> GetMeetingParticipantsAsync(string meetingId, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list past meetings and webinars for a specified time period. The time range for the report is limited to a month and the month must fall within the past six months.
		/// </summary>
		/// <param name="userId">The user ID or email address of the user.</param>
		/// <param name="from">Start date.</param>
		/// <param name="to">End date.</param>
		/// <param name="type">The meeting type to query for.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PastMeeting">meetings</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<PastMeeting>> GetMeetingsAsync(string userId, DateTime from, DateTime to, ReportMeetingType type = ReportMeetingType.Past, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of participants from past webinars with two or more participants. To see a list of participants for webinars with one participant use <see cref="IDashboards.GetMeetingParticipantsAsync"/>.
		/// </summary>
		/// <param name="webinarId">The webinar ID or webinar UUID. If given the webinar ID it will take the last meeting instance.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ReportParticipant">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ReportParticipant>> GetWebinarParticipantsAsync(string webinarId, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets active/inactive host reports.
		/// </summary>
		/// <remarks>
		/// A user is considered to be an active host during the month specified in the "from" and "to" range, if the user has hosted at least one meeting during this period. If the user didn't host any meetings during this period, the user is considered to be inactive.
		/// The Active Hosts report displays a list of meetings, participants, and meeting minutes for a specific time range, up to one month. The month should fall within the last six months.
		/// The Inactive Hosts report pulls a list of users who were not active during a specific period of time.
		/// Use this method to retrieve an active or inactive host report for a specified period of time. The time range for the report is limited to a month and the month should fall under the past six months.
		/// </remarks>
		/// <param name="from">Start date.</param>
		/// <param name="to">End date.</param>
		/// <param name="type">Type of report.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ReportHost">report items</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ReportHost>> GetHostsAsync(DateTime from, DateTime to, ReportHostType type = ReportHostType.Active, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets daily report to access the account-wide usage of Zoom services for each day in a given month. It lists the number of new users, meetings, participants, and meeting minutes.
		/// </summary>
		/// <param name="year">Year for this report.</param>
		/// <param name="month">Month for this report.</param>
		/// <param name="groupId">The group ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The object of <see cref="DailyUsageReport"></see>.
		/// </returns>
		public Task<DailyUsageReport> GetDailyUsageReportAsync(int year, int month, string groupId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets the phone system operation logs report.
		/// </summary>
		/// <remarks>
		/// The phone system operation logs report allows account owners and admins to view monthly Zoom phone related admin operation details.
		/// </remarks>
		/// <param name="from">Start date.</param>
		/// <param name="to">End date.</param>
		/// <param name="type">Type of report.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="OperationLog">report items</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<OperationLog>> GetOperationLogsReportAsync(DateTime from, DateTime to, ReportPhoneOperationsLogType type = ReportPhoneOperationsLogType.All, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);
	}
}
