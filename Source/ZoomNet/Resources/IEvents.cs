using System;
using System.Collections.Generic;
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
		#region ATTENDEE ACTIONS

		/// <summary>
		/// Retrieves a paginated list of actions performed by a specific attendee for a given event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which attendee actions are being retrieved. Cannot be null or empty.</param>
		/// <param name="attendeeEmailAddress">The email address of the attendee whose actions are being retrieved. Cannot be null or empty.</param>
		/// <param name="recordsPerPage">The maximum number of records to include in each page of results. Must be greater than zero. Defaults to 30.</param>
		/// <param name="pagingToken">An optional token used to retrieve the next page of results. Pass <see langword="null"/> to retrieve the first
		/// page.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of attendee
		/// actions.</returns>
		Task<PaginatedResponseWithToken<AttendeeAction>> GetAllAttendeeActionsAsync(string eventId, string attendeeEmailAddress, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Marks the specified attendees as checked in for the given event.
		/// </summary>
		/// <remarks>You can perform onsite check-in for up to 300 attendees in a single call.</remarks>
		/// <param name="eventId">The unique identifier of the event for which attendees are being checked in.</param>
		/// <param name="attendeeEmailAddresses">A collection of email addresses representing the attendees to check in.</param>
		/// <param name="source">The source of the check-in request, such as "onsite" or "api". This is used for tracking purposes.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>An array with information about check-in failures. If all attendees are successfuly checked-in, this array will be empty.</returns>
		Task<(string Email, string ErrorMessage)[]> CheckInAttendeesAsync(string eventId, IEnumerable<string> attendeeEmailAddresses, string source, CancellationToken cancellationToken = default);

		/// <summary>
		/// Marks the specified attendees as checked in for the given event session.
		/// </summary>
		/// <remarks>You can perform onsite check-in for up to 300 attendees in a single call.</remarks>
		/// <param name="eventId">The unique identifier of the event for which attendees are being checked in.</param>
		/// <param name="sessionId">The unique identifier of the session for which attendees are being checked in.</param>
		/// <param name="attendeeEmailAddresses">A collection of email addresses representing the attendees to check in.</param>
		/// <param name="source">The source of the check-in request, such as "onsite" or "api". This is used for tracking purposes.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>An array with information about check-in failures. If all attendees are successfuly checked-in, this array will be empty.</returns>
		Task<(string Email, string ErrorMessage)[]> CheckInAttendeesAsync(string eventId, string sessionId, IEnumerable<string> attendeeEmailAddresses, string source, CancellationToken cancellationToken = default);

		#endregion

		#region COEDITORS

		#endregion

		#region EVENT ACCES

		#endregion

		#region EVENTS

		/// <summary>Retrieves a paginated list of events based on the specified role, status, and pagination parameters.</summary>
		/// <param name="role">The role of the user requesting the events. Defaults to <see cref="UserRoleType.Host"/>.</param>
		/// <param name="status">The status of the events to retrieve. Defaults to <see cref="EventListStatus.Upcoming"/>.</param>
		/// <param name="recordsPerPage">The maximum number of records to include in each page. Defaults to 30.</param>
		/// <param name="pagingToken">A token representing the position in the paginated sequence. Pass <see langword="null"/> to start from the
		/// beginning.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a <see
		/// cref="PaginatedResponseWithToken{T}"/> object with the list of events and a token for retrieving the next page of
		/// results.</returns>
		Task<PaginatedResponseWithToken<Event>> GetAllAsync(UserRoleType role = UserRoleType.Host, EventListStatus status = EventListStatus.Upcoming, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves an event by its unique identifier.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to retrieve. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Event"/> associated
		/// with the specified <paramref name="eventId"/>.</returns>
		Task<Event> GetAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a simple event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="description">The description of the event.</param>
		/// <param name="calendar">An enumeration of start/end dates. Each item in the enumeration represents one day for the event.</param>
		/// <param name="timeZone">The timezone of the event.</param>
		/// <param name="meetingType">The type of the meeting.</param>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="isRestricted">Indicates whether the event is restricted or not.</param>
		/// <param name="attendanceType">The type of attendee experience for the event.</param>
		/// <param name="categories">The category of the event.</param>
		/// <param name="tags">The tags for the event.</param>
		/// <param name="contactName">Optional contact name for the event. Can be null.</param>
		/// <param name="lobbyStart">The start time of the lobby.</param>
		/// <param name="lobbyEnd">The end time of the lobby.</param>
		/// <param name="blockedCountries">Optional list of countries to block from attending the event. Can be null.</param>
		/// <param name="tagLine">This field displays under the event detail page image.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="SimpleEvent"/>.</returns>
		Task<SimpleEvent> CreateSimpleEventAsync(string name, string description, IEnumerable<(DateTime Start, DateTime End)> calendar, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a conference event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="description">The description of the event.</param>
		/// <param name="calendar">An enumeration of start/end dates. Each item in the enumeration represents one day for the conference.</param>
		/// <param name="timeZone">The timezone of the event.</param>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="isRestricted">Indicates whether the event is restricted or not.</param>
		/// <param name="attendanceType">The type of attendee experience for the event.</param>
		/// <param name="categories">The category of the event.</param>
		/// <param name="tags">The tags for the event.</param>
		/// <param name="contactName">Optional contact name for the event. Can be null.</param>
		/// <param name="lobbyStart">The start time of the lobby.</param>
		/// <param name="lobbyEnd">The end time of the lobby.</param>
		/// <param name="blockedCountries">Optional list of countries to block from attending the event. Can be null.</param>
		/// <param name="tagLine">This field displays under the event detail page image.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="Conference"/>.</returns>
		Task<Conference> CreateConferenceAsync(string name, string description, IEnumerable<(DateTime Start, DateTime End)> calendar, TimeZones timeZone, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create an event with recurring sessions.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="description">The description of the event.</param>
		/// <param name="calendar">An enumeration of start/end dates. Each item in the enumeration represents one day for the conference.</param>
		/// <param name="recurrence">Information about recurring sessions.</param>
		/// <param name="timeZone">The timezone of the event.</param>
		/// <param name="hubId">The ID of the event hub.</param>
		/// <param name="isRestricted">Indicates whether the event is restricted or not.</param>
		/// <param name="attendanceType">The type of attendee experience for the event.</param>
		/// <param name="categories">The category of the event.</param>
		/// <param name="tags">The tags for the event.</param>
		/// <param name="contactName">Optional contact name for the event. Can be null.</param>
		/// <param name="lobbyStart">The start time of the lobby.</param>
		/// <param name="lobbyEnd">The end time of the lobby.</param>
		/// <param name="blockedCountries">Optional list of countries to block from attending the event. Can be null.</param>
		/// <param name="tagLine">This field displays under the event detail page image.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The new <see cref="RecurringEvent"/>.</returns>
		Task<RecurringEvent> CreateRecurringEventAsync(string name, string description, IEnumerable<(DateTime Start, DateTime End)> calendar, RecurrenceInfo recurrence, TimeZones timeZone, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Publishes an event to make it available for attendees.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to be published. This cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task PublishEventAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancels the specified event asynchronously.
		/// </summary>
		/// <remarks>Use this method to cancel an event identified by its unique ID. Ensure that the event ID is valid
		/// and that the event is in a state that allows cancellation.</remarks>
		/// <param name="eventId">The unique identifier of the event to cancel. Cannot be null or empty.</param>
		/// <param name="cancellationMessage">The cancellation message.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task CancelEventAsync(string eventId, string cancellationMessage, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes the event with the specified identifier asynchronously.
		/// </summary>
		/// <remarks>Use this method to remove an event from the system. Ensure that the event ID is valid and that
		/// the  operation is not canceled via the <paramref name="cancellationToken"/>.</remarks>
		/// <param name="eventId">The unique identifier of the event to delete. This value cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DeleteEventAsync(string eventId, CancellationToken cancellationToken = default);

		#endregion

		#region EXHIBITORS

		/// <summary>
		/// Creates an exhibitor for a multi-session and conference type event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to which the exhibitor will be added. Cannot be null or empty.</param>
		/// <param name="name">The name of the exhibitor. Cannot be null or empty.</param>
		/// <param name="contactFullName">The full name of the exhibitor's primary contact. Cannot be null or empty.</param>
		/// <param name="contactEmail">The email address of the exhibitor's primary contact. Must be a valid email address. Cannot be null or empty.</param>
		/// <param name="isSponsor">Indicates whether the exhibitor is a sponsor of the event. If true, the <paramref name="sponsorTierId"/> must also be provided.</param>
		/// <param name="sponsorTierId">The sponsor tier ID for the exhibitor. This is required if <paramref name="isSponsor"/> is true; otherwise, it can be null or empty.</param>
		/// <param name="description">A description of the exhibitor. Can be null or empty.</param>
		/// <param name="sessionIds">A collection of session IDs associated with the exhibitor. Can be null or empty.</param>
		/// <param name="website">The URL of the exhibitor's website. Can be null or empty.</param>
		/// <param name="privacyPolicyUrl">The URL of the exhibitor's privacy policy. Can be null or empty.</param>
		/// <param name="linkedInUrl">The URL of the exhibitor's LinkedIn profile. Can be null or empty.</param>
		/// <param name="twitterUrl">The URL of the exhibitor's Twitter profile. Can be null or empty.</param>
		/// <param name="youtubeUrl">The URL of the exhibitor's YouTube channel. Can be null or empty.</param>
		/// <param name="instagramUrl">The URL of the exhibitor's Instagram profile. Can be null or empty.</param>
		/// <param name="facebookUrl">The URL of the exhibitor's Facebook profile. Can be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created <see
		/// cref="EventExhibitor"/> object.</returns>
		Task<EventExhibitor> CreateExhibitorAsync(string eventId, string name, string contactFullName, string contactEmail, bool isSponsor, string sponsorTierId, string description = null, IEnumerable<string> sessionIds = null, string website = null, string privacyPolicyUrl = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, string instagramUrl = null, string facebookUrl = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes an exhibitor from the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event from which the exhibitor will be deleted. Cannot be null or empty.</param>
		/// <param name="exhibitorId">The unique identifier of the exhibitor to delete. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DeleteExhibitorAsync(string eventId, string exhibitorId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the details of a specific exhibitor for a given event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event. This cannot be null or empty.</param>
		/// <param name="exhibitorId">The unique identifier of the exhibitor. This cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EventExhibitor"/>
		/// object representing the exhibitor's details.</returns>
		Task<EventExhibitor> GetExhibitorAsync(string eventId, string exhibitorId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all exhibitors associated with the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which to retrieve exhibitors.  This value cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>An array of <see cref="EventExhibitor"/> objects representing the exhibitors for the specified event.  The array
		/// will be empty if no exhibitors are found.</returns>
		Task<EventExhibitor[]> GetAllExhibitorsAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all sponsor tiers associated with the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which sponsor tiers are to be retrieved. This value cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>An array of <see cref="SponsorTier"/> objects representing the sponsor tiers for the specified event. The array
		/// will be empty if no sponsor tiers are found.</returns>
		Task<SponsorTier[]> GetAllSponsorTiersAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates the details of an exhibitor for a specified event.
		/// </summary>
		/// <remarks>This method allows partial updates to the exhibitor's details. Any parameter that is null will
		/// leave the corresponding property unchanged.</remarks>
		/// <param name="eventId">The unique identifier of the event to which the exhibitor belongs. Cannot be null or empty.</param>
		/// <param name="exhibitorId">The unique identifier of the exhibitor to update. Cannot be null or empty.</param>
		/// <param name="name">The updated name of the exhibitor. If null, the name will remain unchanged.</param>
		/// <param name="contactFullName">The updated full name of the exhibitor's contact person. If null, the contact name will remain unchanged.</param>
		/// <param name="contactEmail">The updated email address of the exhibitor's contact person. If null, the contact email will remain unchanged.</param>
		/// <param name="isSponsor">Indicates whether the exhibitor is a sponsor. If null, the sponsorship status will remain unchanged.</param>
		/// <param name="sponsorTierId">The unique identifier of the sponsor tier associated with the exhibitor. If null, the sponsor tier will remain
		/// unchanged.</param>
		/// <param name="description">The updated description of the exhibitor. If null, the description will remain unchanged.</param>
		/// <param name="sessionIds">A collection of session IDs associated with the exhibitor. If null, the session associations will remain
		/// unchanged.</param>
		/// <param name="website">The updated website URL of the exhibitor. If null, the website will remain unchanged.</param>
		/// <param name="privacyPolicyUrl">The updated URL of the exhibitor's privacy policy. If null, the privacy policy URL will remain unchanged.</param>
		/// <param name="linkedInUrl">The updated LinkedIn profile URL of the exhibitor. If null, the LinkedIn URL will remain unchanged.</param>
		/// <param name="twitterUrl">The updated Twitter profile URL of the exhibitor. If null, the Twitter URL will remain unchanged.</param>
		/// <param name="youtubeUrl">The updated YouTube channel URL of the exhibitor. If null, the YouTube URL will remain unchanged.</param>
		/// <param name="instagramUrl">The updated Instagram profile URL of the exhibitor. If null, the Instagram URL will remain unchanged.</param>
		/// <param name="facebookUrl">The updated Facebook profile URL of the exhibitor. If null, the Facebook URL will remain unchanged.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task UpdateExhibitorAsync(string eventId, string exhibitorId, string name = null, string contactFullName = null, string contactEmail = null, bool? isSponsor = null, string sponsorTierId = null, string description = null, IEnumerable<string> sessionIds = null, string website = null, string privacyPolicyUrl = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, string instagramUrl = null, string facebookUrl = null, CancellationToken cancellationToken = default);

		#endregion

		#region HUBS

		/// <summary>
		/// Create a new hub host.
		/// </summary>
		/// <param name="hubId">The unique identifier of the hub where the host will be added. Cannot be null or empty.</param>
		/// <param name="emailAddress">The email address of the host.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains the unique identifier of the newly created hub host.</returns>
		Task<string> CreateHubHostAsync(string hubId, string emailAddress, CancellationToken cancellationToken = default);

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

		#endregion

		#region REGISTRANTS

		/// <summary>
		/// Retrieves all registrants for the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which registrants are to be retrieved. This parameter cannot be null or empty.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation. When completed, the task contains a collection of registrants for the specified event.</returns>
		Task<PaginatedResponseWithToken<EventRegistrant>> GetAllRegistrantsAsync(string eventId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all attendees for the specified session.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which registrants are to be retrieved. This parameter cannot be null or empty.</param>
		/// <param name="sessionId">The ID of the session.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation. When completed, the task contains a collection of attendees for the specified session.</returns>
		Task<PaginatedResponseWithToken<EventSessionAttendee>> GetAllSessionAttendeesAsync(string eventId, string sessionId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		#endregion

		#region REPORTS

		#endregion

		#region SESSIONS

		/// <summary>Creates a new event session.</summary>
		/// <remarks>This operation is not available for single session event type.</remarks>
		/// <param name="eventId">The unique identifier of the event to which the session belongs. Cannot be null or empty.</param>
		/// <param name="name">The name of the session. Cannot be null or empty.</param>
		/// <param name="start">The start date and time of the session in the specified time zone.</param>
		/// <param name="end">The end date and time of the session in the specified time zone. Must be later than <paramref name="start"/>.</param>
		/// <param name="timeZone">The time zone in which the session's start and end times are defined.</param>
		/// <param name="description">An optional description of the session. Can be null.</param>
		/// <param name="type">The type of the session, such as a meeting or webinar. Defaults to <see cref="EventSessionType.Meeting"/>.</param>
		/// <param name="isFeatured">Indicates whether the session is featured. Defaults to <see langword="false"/>.</param>
		/// <param name="isVisibleInLandingPage">Indicates whether the session is visible on the event's landing page. Defaults to <see langword="true"/>.</param>
		/// <param name="isFeaturedInLobby">Indicates whether the session is featured in the event lobby. Defaults to <see langword="false"/>.</param>
		/// <param name="isVisibleInLobby">Indicates whether the session is visible in the event lobby. Defaults to <see langword="true"/>.</param>
		/// <param name="isSimulive">Indicates whether the session is a simulive session. Defaults to <see langword="false"/>.</param>
		/// <param name="recordingFileId">The identifier of the recording file associated with the session, if any. Can be null.</param>
		/// <param name="isChatInLobbyEnabled">Indicates whether chat is enabled for the session in the event lobby. Defaults to <see langword="false"/>.</param>
		/// <param name="isLedBySponsor">Indicates whether the session is led by a sponsor. Defaults to <see langword="false"/>.</param>
		/// <param name="trackLabels">A collection of track labels associated with the session. Can be null.</param>
		/// <param name="audienceLabels">A collection of audience labels associated with the session. Can be null.</param>
		/// <param name="productLabels">A collection of product labels associated with the session. Can be null.</param>
		/// <param name="levels">A collection of levels associated with the session. Can be null.</param>
		/// <param name="alternativeHosts">A collection of alternative hosts for the session. Can be null.</param>
		/// <param name="panelists">A collection of panelists for the session. Can be null.</param>
		/// <param name="attendanceType">The type of attendance for the session, such as virtual or in-person. Defaults to <see cref="EventAttendanceType.Virtual"/>.</param>
		/// <param name="physicalLocation">The physical location of the session, if applicable. Can be null.</param>
		/// <param name="allowReservations">Indicates whether reservations are allowed for the session. Defaults to <see langword="false"/>.</param>
		/// <param name="maxCapacity">The maximum number of attendees allowed for the session. Defaults to 0, which indicates no limit.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="EventSession"/>
		/// object.</returns>
		Task<EventSession> CreateSessionAsync(
			string eventId,
			string name,
			DateTime start,
			DateTime end,
			TimeZones timeZone,
			string description = null,
			EventSessionType type = EventSessionType.Meeting,
			//IEnumerable<EventSessionSpeaker> speakers = null,
			bool isFeatured = false,
			bool isVisibleInLandingPage = true,
			bool isFeaturedInLobby = false,
			bool isVisibleInLobby = true,
			bool isSimulive = false,
			string recordingFileId = null,
			bool isChatInLobbyEnabled = false,
			bool isLedBySponsor = false,
			IEnumerable<string> trackLabels = null,
			IEnumerable<string> audienceLabels = null,
			IEnumerable<string> productLabels = null,
			IEnumerable<string> levels = null,
			IEnumerable<string> alternativeHosts = null,
			IEnumerable<string> panelists = null,
			EventAttendanceType attendanceType = EventAttendanceType.Virtual,
			string physicalLocation = null,
			bool allowReservations = false,
			int maxCapacity = 0,
			CancellationToken cancellationToken = default);

		#endregion

		#region SPEAKERS

		#endregion

		#region TICKET TYPES

		Task CreateTicketTypeAsync(string eventId, string name, string currencyCode, bool isFree, double price, int quantity, DateTime start, DateTime end, string description, int quantitySold, IEnumerable<string> sessionIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Lists all ticket types associated with an event.
		/// </summary>
		/// <param name="eventId">The ID of the event.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="EventTicketType"/> objects.</returns>
		Task<EventTicketType[]> GetAllTicketTypesAsync(string eventId, CancellationToken cancellationToken = default);

		#endregion

		#region TICKETS

		/// <summary>
		/// Creates multiple tickets for the specified event asynchronously.
		/// </summary>
		/// <remarks>
		/// The Zoom platform will not let you create tickets for 'Online' events and will return an error message like "No access" which, unfortunately, is not clear at all.
		/// You can add up to 30 tickets in a batch operation.
		/// </remarks>
		/// <param name="eventId">The unique identifier of the event for which tickets are being created. Cannot be null or empty.</param>
		/// <param name="tickets">A collection of <see cref="EventTicket"/> objects representing the tickets to be created. Cannot be null or empty.</param>
		/// <param name="source">An optional string indicating the source of the ticket creation request. Can be null.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an array of <see cref="EventTicket"/>
		/// objects  representing the created tickets.</returns>
		Task<EventTicket[]> CreateTicketsAsync(string eventId, IEnumerable<EventTicket> tickets, string source = null, CancellationToken cancellationToken = default);

		#endregion

		#region VIDEO_ON_DEMAND

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
