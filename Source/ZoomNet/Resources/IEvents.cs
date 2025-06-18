using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage events.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/zoom-events/api/#tag/Events">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IEvents
	{
		/// <summary>
		/// Retrieves a list of all hubs.
		/// </summary>
		/// <param name="userRole">The role of the user, which determines the scope of hubs that can be retrieved.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains a collection of hubs accessible to the
		/// specified user role.</returns>
		Task<Hub[]> GetAllHubsAsync(UserRoleType userRole, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a list of hub hosts.
		/// </summary>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Event">events</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<HubHost>> GetAllHubHostsAsync(string hubId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a list of video/recording resources of a hub.
		/// </summary>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="folderId">Optional folder ID to filter the videos by a specific folder.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Event">events</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<HubVideo>> GetAllHubVideosAsync(string hubId, string folderId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes the host from the specified hub.
		/// </summary>
		/// <param name="hubId">The unique identifier of the hub from which the host will be removed. Cannot be null or empty.</param>
		/// <param name="userId">The unique identifier of the user to be removed from the hub. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous operation. The task completes when the host is successfully removed.</returns>
		Task RemoveHostFromHubAsync(string hubId, string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve summary information about all meetings of the specified type for a user.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Event">events</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a simple event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="description">The description of the event.</param>
		/// <param name="start">The start time of the event in UTC.</param>
		/// <param name="end">The end time of the event in UTC.</param>
		/// <param name="timeZone">The timezone of the event.</param>
		/// <param name="meetingType">The type of the meeting.</param>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="isRestricted">Indicates whether the event is restricted or not.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="SimpleEvent"/>.</returns>
		Task<SimpleEvent> CreateSimpleAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default);
		//Task<Event> CreateSimpleAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, accessLevel, meetingType, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);

		//Task<Event> CreateConferenceAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, accessLevel, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);
		//Task<Event> CreateRecurringAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, RecurrenceInfo recurrence, accessLevel, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all registrants for the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which registrants are to be retrieved. This parameter cannot be null or empty.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation. When completed, the task contains a collection of registrants for
		/// the specified event.</returns>
		Task<PaginatedResponseWithToken<EventRegistrant>> GetAllRegistrantsAsync(string eventId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
