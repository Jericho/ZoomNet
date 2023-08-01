using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage locations.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/rooms-location/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface ILocations
	{
		/// <summary>
		/// Retrieve all locations on your account.
		/// </summary>
		/// <param name="parentLocationId">
		/// A unique identifier for the parent location.
		/// For instance, if a Zoom Room is located in Floor 1 of Building A, the location of Building A will be the parent location of Floor 1.
		/// Use this parameter to filter the response by a specific location hierarchy level.
		/// </param>
		/// <param name="type">Use this parameter to filter the response by the type of location.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Location">locations</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Location>> GetAllAsync(string parentLocationId = null, LocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
