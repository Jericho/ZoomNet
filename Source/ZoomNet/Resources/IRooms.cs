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
		Task<PaginatedResponseWithToken<Room>> GetAllAsync(string parentLocationId = null, RoomLocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new room.
		/// </summary>
		/// <param name="name">The name of the room.</param>
		/// <param name="type">The type of the room.</param>
		/// <param name="parentLocationId">Location ID of the lowest level location in the location hierarchy where the Zoom Room is to be added. For instance if the structure of the location hierarchy is set up as “country, states, city, campus, building, floor”, a room can only be added under the floor level location.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="Room">Room</see>.</returns>
		Task<Room> CreateAsync(string name, RoomType type, string parentLocationId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The asyc task.</returns>
		Task DeleteAsync(string roomId, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<RoomLocation>> GetAllLocationsAsync(string parentLocationId = null, RoomLocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the location hierarchial structure(s) applied on the Zoom Rooms in an account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The room location structure expressed as an array of <see cref="RoomLocationType">room location type</see>.</returns>
		Task<RoomLocationType[]> GetLocationStructureAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the location hierarchial structure(s) applied on the Zoom Rooms in an account.
		/// </summary>
		/// <param name="structure">Location structure.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateLocationStructureAsync(IEnumerable<RoomLocationType> structure, CancellationToken cancellationToken = default);

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
		/// <remarks>
		/// A few imporant notes to keep in mind:
		/// - You must <see cref="UpdateLocationStructureAsync">define the location structure</see> before creating a location. I haven't seen this mentioned in the documentation and I struggled for a long to figure out why I couldn't create any new locations until I realized that I hadn't defined a structure. As soon as I defined my desired structure, I was able to create new locations without any problem.
		/// - When you create a top-level location (such as a country for exmaple), you must specify your account ID for the parentId.
		/// </remarks>
		Task<RoomLocation> CreateLocationAsync(string name, string parentId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a room location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteLocationAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Change the assigned parent location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="parentId">Location ID of the new Parent Location under which you the child location will be positioned.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task MoveLocationASync(string locationId, string parentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the alert settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The alert settings.</returns>
		Task<(RoomLocationAlertSettings AlertSettings, RoomLocationNotificationSettings NotificationSettings)> GetLocationAlertSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the general settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The meeting settings.</returns>
		Task<(RoomLocationSecuritySettings SecuritySettings, RoomLocationSettings RoomSettings)> GetLocationSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the signage settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The signage settings.</returns>
		Task<RoomLocationSignageSettings> GetLocationSignageSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the scheduling display settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The scheduling display settings.</returns>
		Task<RoomLocationSchedulingDisplaySettings> GetLocationSchedulingDisplaySettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the information such as the location name, address, support email, and more.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The location profile information.</returns>
		Task<(RoomLocationBasicProfile Basic, RoomLocationSetupProfile Setup)> GetLocationProfileAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the basic profile for a room location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="address">The address of the location.</param>
		/// <param name="description">The description of the location.</param>
		/// <param name="name">The name of the location.</param>
		/// <param name="codeIsRequiredToExit">Indicates whether a code is required to exit out of your Zoom Rooms application to switch between other apps.</param>
		/// <param name="passcode">The 1-16 digit number or characters used to secure your Zoom Rooms application.</param>
		/// <param name="supportEmail">The email address to be used for reporting Zoom Room issues.</param>
		/// <param name="supportPhone">The phone number to be used for reporting Zoom Room issues.</param>
		/// <param name="timezone">Timezone (only returned for location type - city).</param>
		/// <param name="applyBackgroundImageToAllDisplays">Indicates if the same background image is applied to all displays of the Zoom Room. If the value of the this field is true, the backgroundImageInfos parameter can only contain background image information of zoom_rooms_display1.</param>
		/// <param name="backgroundImageInfos">The background image info for each display.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateLocationProfileAsync(string locationId, string address = null, string description = null, string name = null, bool? codeIsRequiredToExit = null, string passcode = null, string supportEmail = null, string supportPhone = null, TimeZones? timezone = null, bool? applyBackgroundImageToAllDisplays = null, IEnumerable<RoomLocationBackgroundImageInfo> backgroundImageInfos = null, CancellationToken cancellationToken = default);
	}
}
