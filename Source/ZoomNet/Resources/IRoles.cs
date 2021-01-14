using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage roles.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/roles/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IRoles
	{
		/// <summary>
		/// Retrieve all roles on your account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Role">roles</see>.
		/// </returns>
		Task<PaginatedResponse<Role>> GetAllAsync(int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);
	}
}
