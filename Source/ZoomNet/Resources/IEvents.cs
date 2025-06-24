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

		#endregion

		#region COEDITORS

		#endregion

		#region EVENT ACCES

		#endregion

		#region EVENTS

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
		Task<SimpleEvent> CreateSimpleEventAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default);

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
		/// Retrieves all sponsor tiers associated with the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event for which sponsor tiers are to be retrieved. This value cannot be null or empty.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>An array of <see cref="SponsorTier"/> objects representing the sponsor tiers for the specified event. The array
		/// will be empty if no sponsor tiers are found.</returns>
		Task<SponsorTier[]> GetAllSponsorTiersAsync(string eventId, CancellationToken cancellationToken = default);

		#endregion

		#region HUBS

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

		#endregion

		#region TICKETS

		#endregion

		#region VIDEO_ON_DEMAND

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
