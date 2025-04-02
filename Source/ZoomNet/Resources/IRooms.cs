using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage rooms.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/rooms-location/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IRooms
	{
		/// <summary>
		/// Retrieve all room locations on your account.
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
		/// An array of <see cref="Room">rooms</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<RoomLocation>> GetAllLocationsAsync(string parentLocationId = null, RoomType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all rooms on your account.
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
		/// An array of <see cref="Room">rooms</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Room>> GetAllAsync(string parentLocationId = null, RoomType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the location hierarchial structure(s) applied on the Zoom Rooms in an account.
		/// </summary>
		/// <param name="structure">Location structure.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateRoomsStructureAsync(IEnumerable<RoomType> structure, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new room location.
		/// </summary>
		/// <param name="name">The name of the location.</param>
		/// <param name="parentId">
		/// The unique identifier of the location that is a level higher from the location that is being added.
		/// For example, to add a City named "City 1" as the child location under a State named "State 1", you must provide the location ID of "State 1".
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The newly created <see cref="RoomLocation">room location</see>.</returns>
		Task<RoomLocation> CreateLocationAsync(string name, string parentId = null, CancellationToken cancellationToken = default);
	}
}
