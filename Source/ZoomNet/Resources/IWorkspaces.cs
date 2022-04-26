using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage workspaces.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/methods/#tag/Workspaces">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IWorkspaces
	{
		/// <summary>
		/// Retrieve all workspaces for a location.
		/// </summary>
		/// <param name="locationId">The location Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Workspace">workspaces</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Workspace>> GetAllAsync(string locationId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
