using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to view various metrics.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/dashboards">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IDashboards
	{
		/// <summary>
		/// Retrieve data on total live or past meetings that occurred during a specified period of time.
		/// Only data from within the last 6 months will be returned.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMeetingMetrics">meetings</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<DashboardMeetingMetrics>> GetAllMeetingsAsync(DateTime from, DateTime to, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="DashboardMeetingMetrics" />.
		/// </returns>
		Task<DashboardMeetingMetrics> GetMeetingAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of participants from live or past meetings.
		/// If you do not provide the type query parameter, the default value will be set to live and thus,
		/// you will only see metrics for participants in a live meeting, if any meeting is currently being conducted.To view metrics on past meeting participants,
		/// provide the appropriate value for type.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMeetingParticipant">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<DashboardMeetingParticipant>> GetMeetingParticipantsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the quality of service for participants from live or past meetings.
		/// This data indicates the connection quality for sending/receiving video, audio, and shared content.
		/// If nothing is being sent or received at that time, no information will be shown in the fields.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="participantId">The participant id.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="DashboardMeetingParticipantQos"/> quality of service metrics for the participant.</returns>
		Task<DashboardMeetingParticipantQos> GetMeetingParticipantQosAsync(string meetingId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of meeting participants from live or past meetings along with the quality of service they receive during the meeting such as connection quality for sending/receiving video, audio, and shared content.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMeetingParticipantQos">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllMeetingParticipantQosAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 1, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the sharing and recording details of participants from live or past meetings.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="ParticipantSharingDetails">sharing details for the meetings participants.</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllMeetingParticipantSharingDetailsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve data on total live or past webinars that occurred during a specified period of time.
		/// Only data from within the last 6 months will be returned.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="type">The type of webinar. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMetricsBase">webinars.</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<DashboardMetricsBase>> GetAllWebinarsAsync(DateTime from, DateTime to, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the details of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID or meeting UUID. If given the webinar ID it will take the last webinar instance.</param>
		/// <param name="type">The type of webinar. Allowed values: Past, PastOne, Live.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="DashboardMetricsBase" />.
		/// </returns>
		Task<DashboardMetricsBase> GetWebinarAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of participants from live or past webinars.
		/// </summary>
		/// <param name="webinarId">The webinar ID or webinar UUID. If given the webinar ID it will take the last webinar instance.</param>
		/// <param name="type">The type of webinar. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardParticipant">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<DashboardParticipant>> GetWebinarParticipantsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the quality of service for participants from live or past webinars.
		/// This data indicates the connection quality for sending/receiving video, audio, and shared content.
		/// If nothing is being sent or received at that time, no information will be shown in the fields.
		/// </summary>
		/// <param name="webinarId">The webinar ID or webinar UUID. If given the webinar ID it will take the last webinar instance.</param>
		/// <param name="participantId">The participant id.</param>
		/// <param name="type">The type of webinars. Allowed values: Past, PastOne, Live.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="DashboardMeetingParticipantQos"/> quality of service metrics for the participant.</returns>
		Task<DashboardMeetingParticipantQos> GetWebinarParticipantQosAsync(string webinarId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of webinar participants from live or past webinars along with the quality of service they receive during the webinar such as connection quality for sending/receiving video, audio, and shared content.
		/// </summary>
		/// <param name="webinarId">The webinar ID or webinar UUID. If given the webinar ID it will take the last webinar instance.</param>
		/// <param name="type">The type of webinars. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMeetingParticipantQos">participants</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllWebinarParticipantQosAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 1, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the sharing and recording details of participants from live or past webinars.
		/// </summary>
		/// <param name="webinarId">The webinar ID or webinar UUID. If given the webinar ID it will take the last webinar instance.</param>
		/// <param name="type">The type of webinars. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="ParticipantSharingDetails">sharing details for the webinars participants.</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllWebinarParticipantSharingDetailsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// List information on all Zoom Rooms in an account.
		/// </summary>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="ZoomRoom"/> Zoom rooms.</returns>
		Task<PaginatedResponseWithToken<ZoomRoom>> GetAllZoomRoomsAsync(int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// The Zoom Rooms dashboard metrics lets you know the type of configuration a Zoom room has and details on the meetings held in that room.
		/// </summary>
		/// <param name="roomId">The Zoom room id.</param>
		/// <param name = "from" >
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="ZoomRoom"/> Zoom room with details on current and past meetings.</returns>
		Task<ZoomRoom> GetRoomDetailsAsync(string roomId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);
	}
}
