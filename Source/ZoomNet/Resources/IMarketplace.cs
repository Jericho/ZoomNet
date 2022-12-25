using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to to access information from Zoom. Also, allows you to build private services or public applications on the Zoom App Marketplace.
	/// </summary>
	/// <remarks>
	/// See Zoom documentation <a href="https://marketplace.zoom.us/docs/api-reference/marketplace-api/">here</a> for more information.
	/// </remarks>
	public interface IMarketplace
	{
		/// <summary>
		/// Retrieve all public apps.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="AppInfo">apps</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<AppInfo>> GetPublicAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all apps created by this account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="AppInfo">apps</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<AppInfo>> GetCreatedAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
