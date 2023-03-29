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
		Task<PaginatedResponseWithToken<UserCallLog>> GetAsync(string userId, DateTime? from = null, DateTime? to = null, CallLogType type = CallLogType.All, string phoneNumber = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
