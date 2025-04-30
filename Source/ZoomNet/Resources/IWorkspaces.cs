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

		/// <summary>
		/// Create a new workspace.
		/// </summary>
		/// <param name="name">The workspace's name.</param>
		/// <param name="type">The type of workspace</param>
		/// <param name="locationId">Location ID of the lowest level location in the location hierarchy where the workspace is to be added. For instance if the structure of the location hierarchy is set up as “country, states, city, campus, building, floor”, a workspace can only be added under the floor level location.</param>
		/// <param name="calendarId">The calendar resource's ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The unique identifier of the newly create workspace.</returns>
		Task<string> CreateAsync(string name, WorkspaceType type = WorkspaceType.Desk, string locationId = null, string calendarId = null, CancellationToken cancellationToken = default);
	}
}
