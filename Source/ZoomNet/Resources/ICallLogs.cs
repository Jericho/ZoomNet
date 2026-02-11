using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage call logs.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/phone/methods/#tag/Call-Logs">Zoom documentation</a> for more information.
	/// </remarks>
	[Obsolete("The CallLogs API endpoints will be deprecated in April 2026. Please use the CallHistory instead. For more information, please refer to https://developers.zoom.us/docs/phone/migrate/")]
	public interface ICallLogs
	{
		/// <summary>
		/// Get call logs for specified user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="type">Type of call log.</param>
		/// <param name="phoneNumber">Phone number for filtering call log.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="UserCallLog" />.
		/// </returns>
		Task<PaginatedResponseWithToken<UserCallLog>> GetAsync(string userId, DateOnly? from = null, DateOnly? to = null, CallLogType type = CallLogType.All, string phoneNumber = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get call logs for an entire account.
		/// </summary>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="type">Type of call log.</param>
		/// <param name="pathType">Filter the API response by path of the call.</param>
		/// <param name="timeType">Enables you to search call logs by start or end time.</param>
		/// <param name="siteId">Unique identifier of the site. Use this query parameter if you have enabled multiple sites and would like to filter the response of this API call by call logs of a specific phone site.</param>
		/// <param name="chargedCallLogs">Whether to filter API responses to include call logs that only have a non-zero charge.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="AccountCallLog" />.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<AccountCallLog>> GetAsync(DateOnly? from = null, DateOnly? to = null, CallLogType type = CallLogType.All, CallLogPathType? pathType = null, CallLogTimeType? timeType = CallLogTimeType.StartTime, string siteId = null, bool chargedCallLogs = false, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
