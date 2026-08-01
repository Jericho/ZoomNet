using System;
using System.Collections.Generic;
using System.IO;
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
		#region ZOOM ROOMS

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
		/// <param name="locationId">Location ID of the lowest level location in the location hierarchy where the Zoom Room is to be added. For instance if the structure of the location hierarchy is set up as “country, states, city, campus, building, floor”, a room can only be added under the floor level location.</param>
		/// <param name="calendarId">The calendar resource's ID.</param>
		/// <param name="tagIds">The tag IDs that will be associated with the Zoom Room.</param>
		/// <param name="userId">The user ID of the user assigned to a Personal Zoom Room. Note: this field will only be accepted when the zoom_room_type is PersonalZoomRoom.</param>
		/// <param name="isProDevice">Indicates whether the Personal Zoom Room will consume a Zoom Rooms license and have access to "Pro" features. Note: this field will only be present for Personal Zoom Rooms.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="Room">Room</see>.</returns>
		Task<Room> CreateAsync(string name, RoomType type, string locationId = null, string calendarId = null, string[] tagIds = null, string userId = null, bool? isProDevice = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The asyc task.</returns>
		Task DeleteAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Change a Zoom Room's location​.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="locationId">Unique identifier of the location where Zoom Room is to be assigned.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The asyc task.</returns>
		/// <remarks>The Zoom Room can be assigned only to the lowest level location available in the hierarchy.</remarks>
		Task MoveAsync(string roomId, string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get detailed information on a specific Zoom Room in a Zoom account.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="regenerateActivationCode">Whether to regenerate an activation code for a Zoom Room. Default is false.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The room profile information.</returns>
		Task<(RoomBasicProfile Basic, string DeviceProfileId, RoomSetupProfile Setup)> GetProfileAsync(string roomId, bool regenerateActivationCode = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Displays the specified emergency content on all Zoom Rooms' displays in the accounts.
		/// </summary>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="accountIds">An enumeration of account identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DisplayEmergencyContentToAccountsAsync(string content, IEnumerable<string> accountIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Displays the specified emergency content on all Zoom Rooms' displays in the specified locations.
		/// </summary>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="locationIds">An enumeration of location identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DisplayEmergencyContentToLocationsAsync(string content, IEnumerable<string> locationIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Displays the specified emergency content on the Zoom Rooms digital signage display.
		/// </summary>
		/// <param name="content">The emergency content to be displayed.</param>
		/// <param name="roomIds">An enumeration of Zoom room identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DisplayEmergencyContentToRoomsAsync(string content, IEnumerable<string> roomIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove the specified emergency content from all Zoom Rooms' displays in the accounts.
		/// </summary>
		/// <param name="accountIds">An enumeration of account identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task RemoveEmergencyContentFromAccountsAsync(IEnumerable<string> accountIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove the specified emergency content from all Zoom Rooms' displays in the specified locations.
		/// </summary>
		/// <param name="locationIds">An enumeration of location identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task RemoveEmergencyContentFromLocationsAsync(IEnumerable<string> locationIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove the specified emergency content from the Zoom Rooms digital signage display.
		/// </summary>
		/// <param name="roomIds">An enumeration of Zoom room identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task RemoveEmergencyContentFromRoomsAsync(IEnumerable<string> roomIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the alert settings applied to the specified Zoom Room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The alert settings.</returns>
		Task<(RoomAlertSettings AlertSettings, RoomNotificationSettings NotificationSettings)> GetAlertSettingsAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the general settings applied to the specified Zoom Room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The meeting settings.</returns>
		Task<(RoomSecuritySettings SecuritySettings, RoomSettings RoomSettings)> GetSettingsAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the signage settings applied to the specified Zoom Room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The signage settings.</returns>
		Task<RoomSignageSettings> GetSignageSettingsAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the scheduling display settings applied to the specified Zoom Room.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The scheduling display settings.</returns>
		Task<RoomSchedulingDisplaySettings> GetSchedulingDisplaySettingsAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves paginated sensor data for a specified room and optional device, filtered by sensor type and date range.
		/// </summary>
		/// <remarks>This method supports filtering by device, sensor type, and date range to narrow down the results.
		/// Use the <paramref name="pageToken"/> parameter to retrieve subsequent pages of data.</remarks>
		/// <param name="roomId">The unique identifier of the room for which sensor data is requested. This parameter cannot be null or empty.</param>
		/// <param name="deviceId">The unique identifier of the device within the room. If null, data from all devices in the room is included.</param>
		/// <param name="sensorType">The type of sensor data to retrieve. If null, data from all sensor types is included.</param>
		/// <param name="from">The start of the date range for the sensor data. If null, data is retrieved from the earliest available date.</param>
		/// <param name="to">The end of the date range for the sensor data. If null, data is retrieved up to the latest available date.</param>
		/// <param name="recordsPerPage">The maximum number of records to include in each page of the response. Must be greater than zero.</param>
		/// <param name="pageToken">A token indicating the page of results to retrieve. If null, the first page is returned.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A paginated response containing sensor data for the specified room and filters, along with metadata such as the
		/// next page token and the date range covered.</returns>
		Task<PaginatedResponseWithTokenAndDateRange<RoomSensorData>> GetSensorDataAsync(string roomId, string deviceId = null, RoomSensorType? sensorType = null, DateOnly? from = null, DateOnly? to = null, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the URL for accessing the virtual controller associated with a specified room.
		/// </summary>
		/// <param name="roomId">The unique identifier of the room for which the virtual controller URL is requested. Must not be null or empty.</param>
		/// <param name="preAuthenticatedLink">A value indicating whether the returned URL should be pre-authenticated. If <see langword="true"/>, the URL will
		/// contain a time-limited authentication token (10 minute lifetime) that will permit to the virtual controller without requiring login to Zoom admin portal. Default is false.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the virtual controller URL as a
		/// string.</returns>
		Task<string> GetVirtualControllerUrlAsync(string roomId, bool preAuthenticatedLink = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of device profiles for a specified room.
		/// </summary>
		/// <param name="roomId">The unique identifier of the room for which device profiles are being requested. This parameter cannot be null or
		/// empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Optional; defaults to <see langword="default"/>.</param>
		/// <returns>A <see cref="PaginatedResponseWithTokenAndDateRange{RoomDeviceProfile}"/> containing the device profiles for the
		/// specified room. The response includes pagination details, a continuation token, and the date range of the
		/// profiles.</returns>
		Task<RoomDeviceProfile[]> GetDeviceProfilesAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of signage content items for the specified resource type.
		/// </summary>
		/// <remarks>Use the <paramref name="pagingToken"/> from the response to retrieve additional pages of results.
		/// If <paramref name="folderId"/> is specified, only content items within the specified folder are
		/// returned.</remarks>
		/// <param name="resourceType">The type of digital signage resource.</param>
		/// <param name="folderId">The identifier of the folder to filter the content items. If <see langword="null"/>, content items from all
		/// folders are included.</param>
		/// <param name="recordsPerPage">The maximum number of records to include per page in the response. Must be a positive integer. Defaults to 30.</param>
		/// <param name="pagingToken">A token used to retrieve the next page of results. If <see langword="null"/>, the first page of results is
		/// returned.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="PaginatedResponseWithToken{T}"/> containing a collection of <see cref="SignageContentItem"/> objects
		/// and a paging token for retrieving subsequent pages.</returns>
		Task<PaginatedResponseWithToken<SignageContentItem>> GetSignageContentsAsync(SignageResourceType resourceType, string folderId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		#endregion

		#region ZOOM ROOM CALENDAR

		/// <summary>
		/// Add a calendar resource to a Microsoft Exchange or Office 365 calendar service in a Zoom Room account.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="resourceEmail">The email address of the calendar resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation, containing the added calendar resource.</returns>
		/// <remarks>This API does not support adding a calendar resource to a Google calendar service.</remarks>
		Task<CalendarResource> AddCalendarResourceAsync(string serviceId, string resourceEmail, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a calendar resource from a Microsoft Exchange or Office 365 calendar service in a Zoom Room account.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="resourceId">The unique identifier of the calendar resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteCalendarResourceAsync(string serviceId, string resourceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a Zoom Room account's calendar service.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteCalendarServiceAsync(string serviceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of the existing calendar services in a Zoom Room account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation, containing a paginated response with the calendar services.</returns>
		Task<PaginatedResponseWithToken<CalendarService>> GetCalendarServicesAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of calendar resources for a given calendar service.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation, containing a paginated response with the calendar resources.</returns>
		Task<PaginatedResponseWithToken<CalendarResource>> GetCalendarResourcesAsync(string serviceId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a calendar resource for a given calendar service.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="resourceId">The unique identifier of the calendar resource.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation, containing the calendar resource.</returns>
		Task<CalendarResource> GetCalendarResourceAsync(string serviceId, string resourceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Kick off the sync process for Zoom Room account's calendar service.
		/// </summary>
		/// <param name="serviceId">The unique identifier of the calendar service.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task StartCalendarServiceSyncProcessAsync(string serviceId, CancellationToken cancellationToken = default);

		#endregion

		#region ZOOM ROOM CONTENT

		/// <summary>
		/// Create a new folder in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="folderName">The name of the folder to create.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly created folder.</returns>
		Task<string> CreateDigitalSignageContentFolderAsync(string folderName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upload a media file to a folder in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="contentName">The name of the media content.</param>
		/// <param name="fileData">The stream containing the media file data.</param>
		/// <param name="folderId">The ID of the folder to add the media file to.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly created media content.</returns>
		Task<string> UploadDigitalSignageMediaAsync(string contentName, Stream fileData, string folderId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new playlist in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistName">The name of the playlist to create.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly created playlist.</returns>
		Task<string> CreateDigitalSignageContentPlaylistAsync(string playlistName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new Zoom Rooms Digital Signage content URL.
		/// </summary>
		/// <param name="contentName">The name of the content.</param>
		/// <param name="url">The URL of the content.</param>
		/// <param name="folderId">The ID of the folder to add the URL to.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly created content URL.</returns>
		Task<string> CreateDigitalSignageUrlAsync(string contentName, string url, string folderId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new folder in the Zoom Rooms Digital Signage content library for background images.
		/// </summary>
		/// <param name="folderName">The name of the folder to create.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly created folder.</returns>
		Task<string> CreateBackgroundImageFolderAsync(string folderName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upload an image to Zoom Room Content Library.
		/// </summary>
		/// <param name="fileName">The name of the image file.</param>
		/// <param name="fileData">The stream containing the image file data.</param>
		/// <param name="folderId">The ID of the folder to add the image to.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The ID of the newly uploaded background image.</returns>
		Task<string> UploadBackgroundImageAsync(string fileName, Stream fileData, string folderId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a folder from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="folderId">The ID of the folder to delete.</param>
		/// <param name="removeFromLibraryOnly">If set to true, the folder and all its content item(s) will be removed from the content library only. If set to false, the folder and all its content item(s) will be removed from the content library and from any playlists and/or Zoom Room content lists that reference the content library item(s).</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <remarks>All digital signage content items in this folder will also be deleted.</remarks>
		Task DeleteDigitalSignageContentFolderAsync(string folderId, bool removeFromLibraryOnly = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a content item from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="contentId">The ID of the content to delete.</param>
		/// <param name="removeFromLibraryOnly">If set to true, the content item will be removed from the content library only. If set to false, the content item will be removed from the content library and from any playlists and/or Zoom Room content lists that reference the content library item(s).</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteDigitalSignageContentItemAsync(string contentId, bool removeFromLibraryOnly = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a playlist from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The ID of the playlist to delete.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteDigitalSignageContentPlaylistAsync(string playlistId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a background image from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="imageId">The ID of the background image to delete.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteBackgroundImageAsync(string imageId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a folder from the Zoom Rooms Digital Signage content library for background images.
		/// </summary>
		/// <param name="folderId">The ID of the folder to delete.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteBackgroundImageFolderAsync(string folderId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a folder from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="folderId">The ID of the folder to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The requested digital signage content folder.</returns>
		Task<DigitalSignageContentFolder> GetDigitalSignageContentFolderAsync(string folderId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a content item from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="contentId">The ID of the content item to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The requested digital signage content item.</returns>
		Task<DigitalSignageContentItem> GetDigitalSignageContentItemAsync(string contentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a playlist from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The ID of the playlist to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The requested digital signage content playlist.</returns>
		Task<DigitalSignageContentPlaylist> GetDigitalSignageContentPlaylistAsync(string playlistId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the items in a playlist from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The ID of the playlist to retrieve items from.</param>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the digital signage content items.</returns>
		Task<PaginatedResponseWithToken<DigitalSignageContentPlaylistItem>> GetDigitalSignageContentPlaylistItemsAsync(string playlistId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a background image from the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="imageId">The ID of the background image to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The requested background image.</returns>
		Task<ZoomRoomBackGroundImage> GetBackgroundImageAsync(string imageId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a folder from the Zoom Rooms Digital Signage content library for background images.
		/// </summary>
		/// <param name="folderId">The ID of the folder to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The requested background image folder.</returns>
		Task<DigitalSignageContentFolder> GetBackgroundImageFolderAsync(string folderId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom-provided default background images from the background image library.
		/// </summary>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom-provided default background images.</returns>
		Task<PaginatedResponseWithToken<ZoomRoomBackGroundImage>> GetDefaultBackgroundImagesAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom Rooms Digital Signage content folders from the content library.
		/// </summary>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom Rooms Digital Signage content folders.</returns>
		Task<PaginatedResponseWithToken<DigitalSignageContentItem>> GetDigitalSignageContentItemsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of rooms where a playlist is published.
		/// </summary>
		/// <param name="playlistId">The unique identifier of the playlist.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of room IDs where the playlist is published.</returns>
		Task<string[]> GetDigitalSignagePublishedRoomsAsync(string playlistId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom Rooms Digital Signage content playlists from the content library.
		/// </summary>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom Rooms Digital Signage content playlists.</returns>
		Task<PaginatedResponseWithToken<DigitalSignageContentPlaylist>> GetDigitalSignageContentPlaylistsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom Rooms Digital Signage content playlists from the content library.
		/// </summary>
		/// <param name="folderId">The ID of the folder to retrieve background images from.</param>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom Rooms Digital Signage background images.</returns>
		Task<PaginatedResponseWithToken<ZoomRoomBackGroundImage>> GetBackgroundImagesAsync(string folderId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom Rooms Digital Signage content folders from the content library for background images.
		/// </summary>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom Rooms Digital Signage content folders for background images.</returns>
		Task<PaginatedResponseWithToken<DigitalSignageContentFolder>> GetBackgroundImageFoldersAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the list of Zoom Rooms Digital Signage content folders from the content library.
		/// </summary>
		/// <param name="recordsPerPage">The number of records to retrieve per page.</param>
		/// <param name="pagingToken">The token for the page to retrieve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A paginated response containing the Zoom Rooms Digital Signage content folders.</returns>
		Task<PaginatedResponseWithToken<DigitalSignageContentFolder>> GetDigitalSignageContentFoldersAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the name of a folder in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="folderId">The unique identifier of the folder.</param>
		/// <param name="folderName">The new name for the folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignageContentFolderAsync(string folderId, string folderName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the name and expiration settings of a digital signage item in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="contentId">The unique identifier of the digital signage item.</param>
		/// <param name="contentName">The new name for the digital signage item.</param>
		/// <param name="url">The new URL for the digital signage item. Please note: This field will be ignored for image or video content items.</param>
		/// <param name="expires">Indicates whether the digital signage item should have an expiration date.</param>
		/// <param name="expirationDate">The expiration date for the digital signage item.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignageItemAsync(string contentId, string contentName = null, string url = null, bool? expires = null, DateTime? expirationDate = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the media file of a digital signage item in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="contentId">The unique identifier of the digital signage item.</param>
		/// <param name="contentName">The new name for the digital signage item.</param>
		/// <param name="fileData">The new media file for the digital signage item.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignageMediaAsync(string contentId, string contentName, Stream fileData, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the name of a playlist in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The unique identifier of the playlist.</param>
		/// <param name="playlistName">The new name for the playlist.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignageContentPlaylistAsync(string playlistId, string playlistName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the items in a playlist in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The unique identifier of the playlist.</param>
		/// <param name="items">The items to update in the playlist. Note: The playlist contents are entirely replaced with the supplied items.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignageContentPlaylistItemsAsync(string playlistId, IEnumerable<(string Id, int Duration)> items, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the list of rooms where a playlist is published in the Zoom Rooms Digital Signage content library.
		/// </summary>
		/// <param name="playlistId">The unique identifier of the playlist.</param>
		/// <param name="roomIds">The list of room IDs where the playlist should be published.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateDigitalSignagePublishedRoomsAsync(string playlistId, IEnumerable<string> roomIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the background image of a Zoom Room.
		/// </summary>
		/// <param name="fileId">The unique identifier of the background image file.</param>
		/// <param name="fileName">The new name for the background image file.</param>
		/// <param name="fileData">The new background image file data.</param>
		/// <param name="folderId">The unique identifier of the folder where the background image should be stored.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateBackgroundImageAsync(string fileId, string fileName, Stream fileData, string folderId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the name of a folder in the Zoom Rooms Digital Signage content library for background images.
		/// </summary>
		/// <param name="folderId">The unique identifier of the folder.</param>
		/// <param name="folderName">The new name for the folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateBackgroundImageFolderAsync(string folderId, string folderName, CancellationToken cancellationToken = default);

		#endregion

		#region ZOOM ROOM DEVICES

		/// <summary>
		/// List information about the devices that are being used for a specific Zoom Room in an account.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An aray of <see cref="RoomDevice"/>.</returns>
		Task<RoomDevice[]> GetAllDevicesAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Returns information about a Zoom Room devices.
		/// </summary>
		/// <param name="roomId">The Zoom Room's ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		/// <remarks>
		/// I haven't been able to test this functionality.
		/// The response to this endpoint is always empty.
		/// </remarks>
		Task GetDevicesInformationAsync(string roomId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new Zoom Room device profile.
		/// </summary>
		/// <param name="roomId">The room unique identifier.</param>
		/// <param name="enableAudioProcessing">Whether to enable audio processing.</param>
		/// <param name="autoAdjustMicrophoneLevel">Whether to enable microphone level auto adjust.</param>
		/// <param name="cameraId">The camera's device ID.</param>
		/// <param name="enableEchoCancellation">Whether to enable echo cancellation.</param>
		/// <param name="microphoneId">The microphone's device ID.</param>
		/// <param name="name">The device profile's name.</param>
		/// <param name="noiseSuppressionType">The noise suppression setting.</param>
		/// <param name="speakerId">The speaker's device ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		/// <remarks>
		/// I haven't been able to test this functionality because I get the following error message:
		/// "Unable to create device profile because there is no microphone/speaker/camera available in the following Zoom Room: aDLGFI6hRvaXkISCUXzUOA.".
		/// </remarks>
		Task CreateDeviceProfileAsync(string roomId, bool? enableAudioProcessing = null, bool? autoAdjustMicrophoneLevel = null, string cameraId = null, bool? enableEchoCancellation = null, string microphoneId = null, string name = null, RoomDeviceNoiseSuppressionType? noiseSuppressionType = null, string speakerId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a Zoom Room device profile.
		/// </summary>
		/// <param name="roomId">The Zoom Room's ID.</param>
		/// <param name="deviceProfileId">The Zoom Room device profile's ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteDeviceProfileAsync(string roomId, string deviceProfileId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Returns information about a Zoom Room devices.
		/// </summary>
		/// <param name="roomId">The Zoom Room's ID.</param>
		/// <param name="deviceProfileId">The Zoom Room device profile's ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The device profile.</returns>
		Task<RoomDeviceProfile> GetDeviceProfileAsync(string roomId, string deviceProfileId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upgrade the version of your installed Zoom Rooms app on your Mac or Windows device.
		/// </summary>
		/// <param name="roomId">Unique Identifier of the Zoom Room.</param>
		/// <param name="deviceId">Unique Identifier of the Mac or the Windows device.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpgradeAppVersionAsync(string roomId, string deviceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Downgrade the version of your installed Zoom Rooms app on your Mac or Windows device.
		/// </summary>
		/// <param name="roomId">Unique Identifier of the Zoom Room.</param>
		/// <param name="deviceId">Unique Identifier of the Mac or the Windows device.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DowngradeAppVersionAsync(string roomId, string deviceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancel an ongoing upgrade or downgrade processof the version of your installed Zoom Rooms app on your Mac or Windows device.
		/// </summary>
		/// <param name="roomId">Unique Identifier of the Zoom Room.</param>
		/// <param name="deviceId">Unique Identifier of the Mac or the Windows device.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task CancelAppVersionChangeAsync(string roomId, string deviceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a Zoom Room device.
		/// </summary>
		/// <param name="roomId">Unique Identifier of the Zoom Room.</param>
		/// <param name="deviceId">Unique Identifier of the Mac or the Windows device.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteDeviceAsync(string roomId, string deviceId, CancellationToken cancellationToken = default);

		#endregion

		#region ZOOM ROOM LOCATIONS

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
		Task<(RoomAlertSettings AlertSettings, RoomNotificationSettings NotificationSettings)> GetLocationAlertSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the general settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The meeting settings.</returns>
		Task<(RoomSecuritySettings SecuritySettings, RoomSettings RoomSettings)> GetLocationSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the signage settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The signage settings.</returns>
		Task<RoomSignageSettings> GetLocationSignageSettingsAsync(string locationId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the scheduling display settings applied to Zoom Rooms located in a specific location.
		/// </summary>
		/// <param name="locationId">The location unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The scheduling display settings.</returns>
		Task<RoomSchedulingDisplaySettings> GetLocationSchedulingDisplaySettingsAsync(string locationId, CancellationToken cancellationToken = default);

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

		#endregion

		#region ZOOM ROOMS TAGS

		/// <summary>
		/// Create a new Zoom Rooms Tag.
		/// </summary>
		/// <param name="name">The name of the Tag.</param>
		/// <param name="description">The short description of the Zoom Rooms Tag.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The unique identifier of the newly create tag.</returns>
		Task<string> CreateTagAsync(string name, string description, CancellationToken cancellationToken = default);

		/// <summary>
		/// Edit an existing tag.
		/// </summary>
		/// <param name="tagId">Tag unique identifier.</param>
		/// <param name="name">The name of the Tag.</param>
		/// <param name="description">The short description of the Zoom Rooms Tag.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateTagAsync(string tagId, string name = null, string description = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Assign multiple tags to a Zoom Room.
		/// </summary>
		/// <param name="roomId">The unique identifier of the Zoom Room.</param>
		/// <param name="tagIds">The Tag IDs to assign to the Zoom Room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task AssignTagsToRoom(string roomId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Assign multiple tags to Zoom Rooms by location ID.
		/// </summary>
		/// <param name="locationId">The unique identifier of the location where all Zoom Rooms under this location to be assigned with tags.</param>
		/// <param name="tagIds">The Tag IDs to assign to all the Zoom Rooms in the given location.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task AssignTagsToRoomsInLocation(string locationId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Un-assign a tag from a Zoom Room.
		/// </summary>
		/// <param name="roomId">The unique identifier of the Zoom Room.</param>
		/// <param name="tagId">The Tag ID to unassign from the Zoom Room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UnAssignTagFromRoom(string roomId, string tagId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all room tags on your account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="RoomTag">tags</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<RoomTag>> GetAllTagsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a new Zoom Rooms Tag.
		/// </summary>
		/// <param name="tagId">The unique identifier the tag to be deleted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteTagAsync(string tagId, CancellationToken cancellationToken = default);

		#endregion
	}
}
