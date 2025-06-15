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
	}
}
