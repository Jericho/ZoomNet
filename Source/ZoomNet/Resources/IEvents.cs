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
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="isRestricted">Indicates whether the event is restricted or not.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="Event"/>.</returns>
		Task<Event> CreateSimpleAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default);
		//Task<Event> CreateSimpleAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, accessLevel, meetingType, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);

		//Task<Event> CreateConferenceAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, accessLevel, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);
		//Task<Event> CreateRecurringAsync(string name, string description, DateTime? start, DateTime? end, DateTime? lobbyStart, DateTime? lobbyEnd, RecurrenceInfo recurrence, accessLevel, IEnumerable<string tags, string hubId, string contactName, IEnumerable<Country> blockedCountries = null, attendenceType, string tagLine = null, CancellationToken cancellationToken = default);

		/*

recurrence
object
Information about recurring sessions.

Show Child Attributes
access_level
string
required
PRIVATE_UNRESTRICTED - Private and unrestricted.
PRIVATE_RESTRICTED - Private and restricted.
meeting_type
string
This value is required only for a single session event.

MEETING - Meeting.
WEBINAR - Webinar.
categories
array string[] …20
enum
The category of the event.

Education & Family.
Business & Networking.
Entertainment & Visual Arts.
Food & Drinks.
Fitness & Health.
Home & Lifestyle
Community & Spirituality.
Other
Education & Family
Business & Networking
Entertainment & Visual Arts
Food & Drink
tags
array string[] …200
The tags for the event.

calendar
array object[] …6
The start and end time of the event in UTC. The format should be yyyy-MM-ddTHH:mm:ssZ.

Event's start and end time.

Show Child Attributes
hub_id
string
required
The ID of the event hub.

start_time
deprecated
string
date-time
The start time of the event in UTC. The format should be yyyy-MM-ddTHH:mm:ssZ this is read only field.

end_time
deprecated
string
date-time
The end time of the event in UTC. The format should be yyyy-MM-ddTHH:mm:ssZ this is a read only field.

contact_name
string
The contact person's name for the event.

lobby_start_time
string
date-time
The start time of the lobby in UTC. The format should be yyyy-MM-ddTHH:mm:ssZ.

lobby_end_time
string
date-time
The end time of the lobby in UTC. The format should be yyyy-MM-ddTHH:mm:ssZ.

blocked_countries
array string[] …200
Attendees from the countries listed here will not be allowed to register to the event.

attendance_type
string
enum
required
The type of attendee experience for the event.

VIRTUAL - virtual attendees only.
IN-PERSON - in-person attendees only.
HYBRID - both in-person and virtual attendees.
virtual
in-person
hybrid
physical_location
deprecated
string
The physical location of the event. This field is applicable for hybrid and in-person events only.

tagline
string
This field displays under the event detail page image.
		 * */
	}
}
