using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to access Zoom Phone SMS API endpoints.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#tag/SMS">
	/// Zoom API documentation</a> for more information.
	/// </remarks>
	public interface ISms
	{
		/// <summary>
		/// Get details about an SMS session.
		/// </summary>
		/// <param name="sessionId">SMS session ID.</param>
		/// <param name="from">The start time and date. The date range defined by the <paramref name="to"/> and <paramref name="from"/> parameters should be a month as the response only includes one month's worth of data at once. If unspecified, returns data from the past 30 days.</param>
		/// <param name="to">Required only when the from parameter is specified.</param>
		/// <param name="orderAscending">Order of SMS to return based on creation time. True - ascending, false - descending, null - doesn't sort.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call. Max value 100.</param>
		/// <param name="pagingToken">The page identifier of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="SmsMessage">sms histories</see>.</returns>
		/// <remarks>
		/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#operation/smsSessionDetails">
		/// Zoom endpoint documentation</a> for more information.
		/// </remarks>
		Task<PaginatedResponseWithToken<SmsMessage>> GetSmsSessionDetailsAsync(
			string sessionId, DateTime? from, DateTime? to, bool? orderAscending = true, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
