using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage events.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/zoom-events/api/#tag/Events">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IEvents
	{
		/// <summary>
		/// Retrieve summary information about all meetings of the specified type for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="type">The type of meetings. Allowed values: Scheduled, Live, Upcoming.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingSummary">meeting summaries</see>.
		/// </returns>
		/// <remarks>
		/// To obtain the full details about a given meeting you must invoke <see cref="Meetings.GetAsync(long, string, bool, CancellationToken)"/>.
		/// </remarks>
		Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
