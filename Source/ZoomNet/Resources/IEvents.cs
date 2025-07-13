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
		/// <param name="start">The date and time when the event ends.</param>
		/// <param name="end">The date and time when the event starts.</param>
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
		Task<SimpleEvent> CreateSimpleEventAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default);

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
		/// <param name="start">The date and time when the event ends.</param>
		/// <param name="end">The date and time when the event starts.</param>
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
		Task<RecurringEvent> CreateRecurringEventAsync(string name, string description, DateTime start, DateTime end, EventRecurrenceInfo recurrence, TimeZones timeZone, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default);

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
		/// <param name="speakers">A collection of speakers for the session, each represented by a tuple containing the speaker's ID, whether they can edit the session, whether they are displayed in session details, and whether they can act as an alternative host. Can be null.</param>
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
			IEnumerable<(string Id, bool CanEditSession, bool IsDisplayedInSessionDetails, bool CanActAsAlternativeHost)> speakers = null,
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

		/// <summary>
		/// Asynchronously deletes a session associated with the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to which the session belongs. Cannot be null or empty.</param>
		/// <param name="sessionId">The unique identifier of the session to be deleted. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteSessionAsync(string eventId, string sessionId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously updates the details of a session within an event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event containing the session to update. Cannot be null or empty.</param>
		/// <param name="sessionId">The unique identifier of the session to update. Cannot be null or empty.</param>
		/// <param name="name">The new name of the session. If null, the name will not be updated.</param>
		/// <param name="start">The new start time of the session. If null, the start time will not be updated.</param>
		/// <param name="end">The new end time of the session. If null, the end time will not be updated.</param>
		/// <param name="timeZone">The time zone of the session. If null, the time zone will not be updated.</param>
		/// <param name="description">The new description of the session. If null, the description will not be updated.</param>
		/// <param name="type">The type of the session. If null, the type will not be updated.</param>
		/// <param name="speakers">A collection of speakers associated with the session, including their permissions. If null, the speakers will not be updated.</param>
		/// <param name="isFeatured">Indicates whether the session is featured. If null, this property will not be updated.</param>
		/// <param name="isVisibleInLandingPage">Indicates whether the session is visible on the landing page. If null, this property will not be updated.</param>
		/// <param name="isFeaturedInLobby">Indicates whether the session is featured in the lobby. If null, this property will not be updated.</param>
		/// <param name="isVisibleInLobby">Indicates whether the session is visible in the lobby. If null, this property will not be updated.</param>
		/// <param name="isSimulive">Indicates whether the session is simulive. If null, this property will not be updated.</param>
		/// <param name="recordingFileId">The identifier of the recording file associated with the session. If null, the recording file will not be updated.</param>
		/// <param name="isChatInLobbyEnabled">Indicates whether chat is enabled in the lobby for the session. If null, this property will not be updated.</param>
		/// <param name="isLedBySponsor">Indicates whether the session is led by a sponsor. If null, this property will not be updated.</param>
		/// <param name="trackLabels">A collection of track labels associated with the session. If null, the track labels will not be updated.</param>
		/// <param name="audienceLabels">A collection of audience labels associated with the session. If null, the audience labels will not be updated.</param>
		/// <param name="productLabels">A collection of product labels associated with the session. If null, the product labels will not be updated.</param>
		/// <param name="levels">A collection of levels associated with the session. If null, the levels will not be updated.</param>
		/// <param name="alternativeHosts">A collection of alternative hosts for the session. If null, the alternative hosts will not be updated.</param>
		/// <param name="panelists">A collection of panelists for the session. If null, the panelists will not be updated.</param>
		/// <param name="attendanceType">The attendance type of the session. If null, the attendance type will not be updated.</param>
		/// <param name="physicalLocation">The physical location of the session. If null, the location will not be updated.</param>
		/// <param name="allowReservations">Indicates whether reservations are allowed for the session. If null, this property will not be updated.</param>
		/// <param name="maxCapacity">The maximum capacity of the session. If null, the capacity will not be updated.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous update operation.</returns>
		Task UpdateSessionAsync(
			string eventId,
			string sessionId,
			string name = null,
			DateTime? start = null,
			DateTime? end = null,
			TimeZones? timeZone = null,
			string description = null,
			EventSessionType? type = null,
			IEnumerable<(string Id, bool CanEditSession, bool IsDisplayedInSessionDetails, bool CanActAsAlternativeHost)> speakers = null,
			bool? isFeatured = null,
			bool? isVisibleInLandingPage = null,
			bool? isFeaturedInLobby = null,
			bool? isVisibleInLobby = null,
			bool? isSimulive = null,
			string recordingFileId = null,
			bool? isChatInLobbyEnabled = null,
			bool? isLedBySponsor = null,
			IEnumerable<string> trackLabels = null,
			IEnumerable<string> audienceLabels = null,
			IEnumerable<string> productLabels = null,
			IEnumerable<string> levels = null,
			IEnumerable<string> alternativeHosts = null,
			IEnumerable<string> panelists = null,
			EventAttendanceType? attendanceType = null,
			string physicalLocation = null,
			bool? allowReservations = null,
			int? maxCapacity = null,
			CancellationToken cancellationToken = default);

		#endregion

		#region SPEAKERS

		/// <summary>
		/// Asynchronously creates a new speaker for the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to which the speaker will be added. Cannot be null or empty.</param>
		/// <param name="name">The name of the speaker. Cannot be null or empty.</param>
		/// <param name="emailAddress">The email address of the speaker. Optional.</param>
		/// <param name="jobTitle">The job title of the speaker. Optional.</param>
		/// <param name="biography">A brief biography of the speaker. Optional.</param>
		/// <param name="companyName">The name of the company the speaker is associated with. Optional.</param>
		/// <param name="companyWebsite">The website of the company the speaker is associated with. Optional.</param>
		/// <param name="linkedInUrl">The LinkedIn profile URL of the speaker. Optional.</param>
		/// <param name="twitterUrl">The Twitter profile URL of the speaker. Optional.</param>
		/// <param name="youtubeUrl">The YouTube channel URL of the speaker. Optional.</param>
		/// <param name="featuredInEventDetailPage">Indicates whether the speaker is featured on the event detail page. Defaults to <see langword="false"/>.</param>
		/// <param name="visibleInEventDetailPage">Indicates whether the speaker is visible on the event detail page. Defaults to <see langword="true"/>.</param>
		/// <param name="featuredInLobby">Indicates whether the speaker is featured in the event lobby. Defaults to <see langword="false"/>.</param>
		/// <param name="visibleInLobby">Indicates whether the speaker is visible in the event lobby. Defaults to <see langword="true"/>.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="EventSpeaker"/>
		/// object.</returns>
		Task<EventSpeaker> CreateSpeakerAsync(string eventId, string name, string emailAddress = null, string jobTitle = null, string biography = null, string companyName = null, string companyWebsite = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, bool featuredInEventDetailPage = false, bool visibleInEventDetailPage = true, bool featuredInLobby = false, bool visibleInLobby = true, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously deletes a speaker from the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event from which the speaker will be removed. Cannot be null or empty.</param>
		/// <param name="speakerId">The unique identifier of the speaker to be deleted. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteSpeakerAsync(string eventId, string speakerId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves the details of a speaker for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which the speaker details are requested. Cannot be null or empty.</param>
		/// <param name="speakerId">The unique identifier of the speaker whose details are requested. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EventSpeaker"/> object
		/// with the speaker's details.</returns>
		Task<EventSpeaker> GetSpeakerAsync(string eventId, string speakerId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves all speakers for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which to retrieve speakers. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains an array of <see cref="EventSpeaker"/>
		/// objects representing the speakers for the specified event. The array will be empty if no speakers are found.</returns>
		Task<EventSpeaker[]> GetAllSpeakersAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously updates the details of a speaker for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to which the speaker belongs. Cannot be null or empty.</param>
		/// <param name="speakerId">The unique identifier of the speaker to update. Cannot be null or empty.</param>
		/// <param name="name">The updated name of the speaker. If null, the name will not be changed.</param>
		/// <param name="emailAddress">The updated email address of the speaker. If null, the email address will not be changed.</param>
		/// <param name="jobTitle">The updated job title of the speaker. If null, the job title will not be changed.</param>
		/// <param name="biography">The updated biography of the speaker. If null, the biography will not be changed.</param>
		/// <param name="companyName">The updated company name of the speaker. If null, the company name will not be changed.</param>
		/// <param name="companyWebsite">The updated company website of the speaker. If null, the company website will not be changed.</param>
		/// <param name="linkedInUrl">The updated LinkedIn URL of the speaker. If null, the LinkedIn URL will not be changed.</param>
		/// <param name="twitterUrl">The updated Twitter URL of the speaker. If null, the Twitter URL will not be changed.</param>
		/// <param name="youtubeUrl">The updated YouTube URL of the speaker. If null, the YouTube URL will not be changed.</param>
		/// <param name="featuredInEventDetailPage">Indicates whether the speaker should be featured on the event detail page. If null, this setting will not be
		/// changed.</param>
		/// <param name="visibleInEventDetailPage">Indicates whether the speaker should be visible on the event detail page. If null, this setting will not be
		/// changed.</param>
		/// <param name="featuredInLobby">Indicates whether the speaker should be featured in the event lobby. If null, this setting will not be changed.</param>
		/// <param name="visibleInLobby">Indicates whether the speaker should be visible in the event lobby. If null, this setting will not be changed.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="EventSpeaker"/>
		/// object.</returns>
		Task UpdateSpeakerAsync(string eventId, string speakerId, string name = null, string emailAddress = null, string jobTitle = null, string biography = null, string companyName = null, string companyWebsite = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, bool? featuredInEventDetailPage = null, bool? visibleInEventDetailPage = null, bool? featuredInLobby = null, bool? visibleInLobby = null, CancellationToken cancellationToken = default);

		#endregion

		#region TICKET TYPES

		/// <summary>
		/// Create a ticket type for a specified event.
		/// </summary>
		/// <remarks>This API is not allowed for single session and recurring event type.</remarks>
		/// <param name="eventId">The ID of the event.</param>
		/// <param name="name">The name of the ticket type.</param>
		/// <param name="start">The start time of ticket sales.</param>
		/// <param name="end">The end time of ticket sales.</param>
		/// <param name="currencyCode">The currency of the ticket type.</param>
		/// <param name="price">The price of the ticket type. A null value indicates that the ticket is free.</param>
		/// <param name="quantity">The quantity of the ticket type.</param>
		/// <param name="description">The description of the ticket type.</param>
		/// <param name="quantitySold">The total number of tickets sold.</param>
		/// <param name="sessionIds">The list of session IDs allowed for this ticket type. If this value is omitted, this ticket type will be valid for all the sessions in the event.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The unique identifier of the newly created ticket type.</returns>
		Task<string> CreateTicketTypeAsync(string eventId, string name, DateTime start, DateTime end, string currencyCode, double? price = null, int? quantity = null, string description = null, int? quantitySold = null, IEnumerable<string> sessionIds = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a specific ticket type associated with the given event.
		/// </summary>
		/// <remarks>Use this method to remove a ticket type from an event. Ensure that the ticket type is not in use
		/// or associated with active sales before calling this method.</remarks>
		/// <param name="eventId">The unique identifier of the event to which the ticket type belongs. Cannot be null or empty.</param>
		/// <param name="ticketTypeId">The unique identifier of the ticket type to delete. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DeleteTicketTypeAsync(string eventId, string ticketTypeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Lists all ticket types associated with an event.
		/// </summary>
		/// <param name="eventId">The ID of the event.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="EventTicketType"/> objects.</returns>
		Task<EventTicketType[]> GetAllTicketTypesAsync(string eventId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a ticket type for a specified event.
		/// </summary>
		/// <remarks>This API is not allowed for single session and recurring event type.</remarks>
		/// <param name="eventId">The ID of the event.</param>
		/// <param name="ticketTypeId">The unique identifier of the ticket type to delete. Cannot be null or empty.</param>
		/// <param name="name">The name of the ticket type.</param>
		/// <param name="start">The start time of ticket sales.</param>
		/// <param name="end">The end time of ticket sales.</param>
		/// <param name="currencyCode">The currency of the ticket type.</param>
		/// <param name="price">The price of the ticket type. A null value indicates that the ticket is free.</param>
		/// <param name="quantity">The quantity of the ticket type.</param>
		/// <param name="description">The description of the ticket type.</param>
		/// <param name="quantitySold">The total number of tickets sold.</param>
		/// <param name="sessionIds">The list of session IDs allowed for this ticket type. If this value is omitted, this ticket type will be valid for all the sessions in the event.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task UpdateTicketTypeAsync(string eventId, string ticketTypeId, string name = null, DateTime? start = null, DateTime? end = null, string currencyCode = null, double? price = null, int? quantity = null, string description = null, int quantitySold = 0, IEnumerable<string> sessionIds = null, CancellationToken cancellationToken = default);

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

		/// <summary>
		/// Asynchronously deletes a ticket for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event associated with the ticket. Cannot be null or empty.</param>
		/// <param name="ticketId">The unique identifier of the ticket to be deleted. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteTicketAsync(string eventId, string ticketId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves the event ticket specified by the event and ticket identifiers.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which the ticket is requested. Cannot be null or empty.</param>
		/// <param name="ticketId">The unique identifier of the ticket to retrieve. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EventTicket"/>
		/// associated with the specified identifiers.</returns>
		Task<EventTicket> GetTicketAsync(string eventId, string ticketId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves a paginated list of event tickets for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which tickets are being retrieved. Cannot be null or empty.</param>
		/// <param name="ticketTypeId">The optional identifier of the ticket type to filter the results. If null, tickets of all types are retrieved.</param>
		/// <param name="recordsPerPage">The number of tickets to include per page. Must be a positive integer. Defaults to 30.</param>
		/// <param name="pagingToken">The token used to retrieve the next set of results in a paginated response. If null, the first page is retrieved.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains a <see
		/// cref="PaginatedResponseWithToken{EventTicket}"/> with the list of event tickets and a token for the next page of
		/// results.</returns>
		Task<PaginatedResponseWithToken<EventTicket>> GetAllTicketsAsync(string eventId, string ticketTypeId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates the details of a ticket for a specified event asynchronously.
		/// </summary>
		/// <remarks>This method allows partial updates of ticket information. Only non-null parameters will be
		/// updated.</remarks>
		/// <param name="eventId">The unique identifier of the event associated with the ticket.</param>
		/// <param name="ticketId">The unique identifier of the ticket to be updated.</param>
		/// <param name="firstName">The first name of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="lastName">The last name of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="address">The address of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="city">The city of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="state">The state of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="zip">The ZIP code of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="country">The country of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="phone">The phone number of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="industry">The industry of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="jobTitle">The job title of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="organization">The organization of the ticket holder. Can be null to leave unchanged.</param>
		/// <param name="comments">Additional comments about the ticket. Can be null to leave unchanged.</param>
		/// <param name="externalTicketId">An external identifier for the ticket. Can be null to leave unchanged.</param>
		/// <param name="customQuestions">A collection of custom question and answer pairs related to the ticket. Can be null to leave unchanged.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous update operation.</returns>
		Task UpdateTicketAsync(string eventId, string ticketId, string firstName = null, string lastName = null, string address = null, string city = null, string state = null, string zip = null, string country = null, string phone = null, string industry = null, string jobTitle = null, string organization = null, string comments = null, string externalTicketId = null, IEnumerable<KeyValuePair<string, string>> customQuestions = null, CancellationToken cancellationToken = default);

		#endregion

		#region VIDEO_ON_DEMAND

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
