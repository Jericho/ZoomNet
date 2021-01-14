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
		/// Retrieve all rooms on your account.
		/// </summary>
		/// <param name="status">Use this parameter to filter the response by the status of room.</param>
		/// <param name="type">Use this parameter to filter the response by the type of room.</param>
		/// <param name="unassignedRooms">Use this query parameter with a value of `true` if you would like to see Zoom Rooms in your account that have not been assigned to anyone yet.</param>
		/// <param name="locationId">Parent location ID of the Zoom Room.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Room">rooms</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Room>> GetAllAsync(RoomStatus? status = null, RoomType? type = null, bool unassignedRooms = false, string locationId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds a room to your account.
		/// </summary>
		/// <param name="name">The name of the new room.</param>
		/// <param name="type">The type of the new room.</param>
		/// <param name="locationId">Parent location ID of the new room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The added <see cref="Room">room</see>.</returns>
		Task<Room> AddAsync(string name, RoomType type, string locationId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a room from your account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task DeleteAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets detailed information on a specific Zoom Room.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="RoomProfile">room profile</see>.</returns>
		Task<RoomProfile> GetProfileAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update basic information on a specific Zoom Room in a Zoom account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="name">The name of the room.</param>
		/// <param name="supportEmail">The support email address of the room.</param>
		/// <param name="supportPhone">The support phone number of the room.</param>
		/// <param name="roomPasscode">The passcode for the room.</param>
		/// <param name="requiredCodeToExit">A value determining if the room requires a code to leave the Zoom application.</param>
		/// <param name="hideRoomInContacts">A value determining if the room should be hidden from the contacts list.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateProfileAsync(string roomId, string name = null, string supportEmail = null, string supportPhone = null, string roomPasscode = null, bool? requiredCodeToExit = null, bool? hideRoomInContacts = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// List information about the devices that are being used for a specific Zoom Room in an account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="Device">devices</see></returns>
		Task<Device[]> GetDevicesAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates the location of the Zoom Room.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="locationId">Parent location ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateLocationAsync(string roomId, string locationId, CancellationToken cancellationToken = default);
	}
}
