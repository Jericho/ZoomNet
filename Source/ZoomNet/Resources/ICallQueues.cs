using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage call queues.
	/// </summary>
	public interface ICallQueues
	{
		/// <summary>
		/// Get call queue details.
		/// </summary>
		/// <param name="callQueueId">The ID of the call queue.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// Details about a <see cref="CallQueue"/>.
		/// </returns>
		public Task<CallQueue> GetAsync(string callQueueId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get call queues.
		/// </summary>
		/// <param name="department">Filter by department.</param>
		/// <param name="costCenter">Filter by cost center.</param>
		/// <param name="siteId">Filter by site id.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A paginated response of <see cref="CallQueue"/>.
		/// </returns>
		public Task<PaginatedResponseWithToken<CallQueue>> GetAllAsync(string department = null, string costCenter = null, string siteId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get members currently in call queues.
		/// </summary>
		/// <param name="callQueueId">The ID of the call queue.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of current call queue membership entries.</returns>
		public Task<PaginatedResponseWithToken<CallQueueMember>> GetMembersAsync(string callQueueId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
