using System;
using System.Collections.Generic;
using System.Threading;
using ZoomNet.Models;
using ZoomNet.Resources;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace ZoomNet
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	/// <summary>
	/// Extension methods for pagination helpers on Zoom API resources.
	/// </summary>
	public static class PaginationExtensions
	{
		#region IMeetings Extensions

		/// <summary>
		/// Enumerates all meetings for a user across all pages.
		/// </summary>
		/// <param name="meetings">The meetings resource.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="type">The meeting type.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="timeZone">The time zone.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of meetings.</returns>
		public static IAsyncEnumerable<MeetingSummary> EnumerateAllAsync(
			this IMeetings meetings,
			string userId,
			MeetingListType? type = null,
			DateTime? from = null,
			DateTime? to = null,
			TimeZones? timeZone = null,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => meetings.GetAllAsync(userId, type, from, to, timeZone, recordsPerPage, null, cancellationToken),
				pagingToken => meetings.GetAllAsync(userId, type, from, to, timeZone, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		#endregion

		#region IWebinars Extensions

		/// <summary>
		/// Enumerates all webinars for a user across all pages.
		/// </summary>
		/// <param name="webinars">The webinars resource.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of webinars.</returns>
		public static IAsyncEnumerable<WebinarSummary> EnumerateAllAsync(
			this IWebinars webinars,
			string userId,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => webinars.GetAllAsync(userId, recordsPerPage, null, cancellationToken),
				pagingToken => webinars.GetAllAsync(userId, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		/// <summary>
		/// Enumerates all registrants for a webinar across all pages.
		/// </summary>
		/// <param name="webinars">The webinars resource.</param>
		/// <param name="webinarId">The webinar Id.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="trackingSourceId">The tracking source ID.</param>
		/// <param name="occurrenceId">The occurrence ID.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of registrants.</returns>
		public static IAsyncEnumerable<Registrant> EnumerateAllRegistrantsAsync(
			this IWebinars webinars,
			long webinarId,
			RegistrantStatus status,
			string trackingSourceId = null,
			string occurrenceId = null,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => webinars.GetRegistrantsAsync(webinarId, status, trackingSourceId, occurrenceId, recordsPerPage, null, cancellationToken),
				pagingToken => webinars.GetRegistrantsAsync(webinarId, status, trackingSourceId, occurrenceId, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		#endregion

		#region IUsers Extensions

		/// <summary>
		/// Enumerates all users across all pages.
		/// </summary>
		/// <param name="users">The users resource.</param>
		/// <param name="status">The user status.</param>
		/// <param name="roleId">The role ID.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of users.</returns>
		public static IAsyncEnumerable<User> EnumerateAllAsync(
			this IUsers users,
			UserStatus status = UserStatus.Active,
			string roleId = null,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => users.GetAllAsync(status, roleId, recordsPerPage, null, cancellationToken),
				pagingToken => users.GetAllAsync(status, roleId, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		#endregion

		#region ICloudRecordings Extensions

		/// <summary>
		/// Enumerates all cloud recordings for a user across all pages.
		/// </summary>
		/// <param name="recordings">The cloud recordings resource.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="queryTrash">Query trash.</param>
		/// <param name="from">Start date.</param>
		/// <param name="to">End date.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of recordings.</returns>
		public static IAsyncEnumerable<Recording> EnumerateAllRecordingsAsync(
			this ICloudRecordings recordings,
			string userId,
			bool queryTrash = false,
			DateTime? from = null,
			DateTime? to = null,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => recordings.GetRecordingsForUserAsync(userId, queryTrash, from, to, recordsPerPage, null, cancellationToken),
				pagingToken => recordings.GetRecordingsForUserAsync(userId, queryTrash, from, to, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		#endregion

		#region IReports Extensions

		/// <summary>
		/// Enumerates all meeting participants across all pages.
		/// </summary>
		/// <param name="reports">The reports resource.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of participants.</returns>
		public static IAsyncEnumerable<ReportParticipant> EnumerateAllMeetingParticipantsAsync(
			this IReports reports,
			string meetingId,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => reports.GetMeetingParticipantsAsync(meetingId, recordsPerPage, null, cancellationToken),
				pagingToken => reports.GetMeetingParticipantsAsync(meetingId, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		/// <summary>
		/// Enumerates all meetings for a user across all pages.
		/// </summary>
		/// <param name="reports">The reports resource.</param>
		/// <param name="userId">The user ID or email address.</param>
		/// <param name="from">Start date.</param>
		/// <param name="to">End date.</param>
		/// <param name="type">The meeting type.</param>
		/// <param name="recordsPerPage">The number of records per page.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of past meetings.</returns>
		public static IAsyncEnumerable<PastMeeting> EnumerateAllMeetingsAsync(
			this IReports reports,
			string userId,
			DateTime from,
			DateTime to,
			ReportMeetingType type = ReportMeetingType.Past,
			int recordsPerPage = 300,
			CancellationToken cancellationToken = default)
		{
			return Utilities.PaginationHelpers.EnumerateAllRecordsAsync(
				() => reports.GetMeetingsAsync(userId, from, to, type, recordsPerPage, null, cancellationToken),
				pagingToken => reports.GetMeetingsAsync(userId, from, to, type, recordsPerPage, pagingToken, cancellationToken),
				cancellationToken);
		}

		#endregion
	}
}
