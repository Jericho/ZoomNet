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
		Task<PaginatedResponseWithToken<DashboardMeetingParticipant>> GetMeetingParticipantsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllMeetingParticipantsQosAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 1, string pageToken = null, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<DashboardParticipant>> GetWebinarParticipantsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

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
		/// <param name="from">
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

		/// <summary>
		/// A Cloud Room Connector allows H.323/SIP endpoints to connect to a Zoom meeting. <br/>
		/// Use this API to get the hour by hour CRC Port usage for a specified period of time. <br/>
		/// We will provide the report for a maximum of one month.For example, if “from” is set to “2017-08-05” and “to” is set to “2017-10-10”, we will adjust “from” to “2017-09-10”.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="CrcPortMetrics">Report </see> of metrics on CRC usage.</returns>
		Task<CrcPortMetrics> GetCrcPortUsageAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get <a href="https://support.zoom.us/hc/en-us/articles/204654719-Dashboard#h_cc7e9749-1c70-4afb-a9a2-9680654821e4">metrics</a> on how users are utilizing the Zoom Chat Client.
		/// </summary>
		/// <param name="from">
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
		/// <returns>
		/// An array of <see cref="ImMetric">chat room usage metrics</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<ImMetric>> GetImMetricsAsync(DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve survey results from <a href="https://support.zoom.us/hc/en-us/articles/115005855266-End-of-Meeting-Feedback-Survey#h_e30d552b-6d8e-4e0a-a588-9ca8180c4dbf">Zoom meetings client feedback.</a>.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="ClientFeedbackMetricsReport"/> report with metrics on client feedback.</returns>
		Task<ClientFeedbackMetricsReport> GetClientFeedbackMetricsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get Top 25 issues of Zoom rooms.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="IssuesOfZoomRoomsReport"/> report with the list of top issues in Zoom rooms.</returns>
		Task<IssuesOfZoomRoomsReport> GetIssuesOfZoomRoomsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get information on top 25 Zoom Rooms with issues in a month. The month specified with the “from” and “to” range should fall within the last six months.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="ZoomRoomWithIssuesReport"/> report with the list of Zoom rooms with issues.</returns>
		Task<ZoomRoomWithIssuesReport> GetZoomRoomsWithIssuesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get information about the issues that occurred on the Top 25 Zoom Rooms with issues in an account.
		/// </summary>
		/// <param name="zoomRoomId">The Zoom room id.</param>
		/// <param name="from">
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
		/// <returns>
		/// An array of <see cref="ZoomRoomIssueDetails">Zoom room issue details</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<ZoomRoomIssueDetails>> GetIssuesOfZoomRoomAsync(string zoomRoomId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve detailed information on a <a href="https://support.zoom.us/hc/en-us/articles/115005855266-End-of-Meeting-Feedback-Survey#h_e30d552b-6d8e-4e0a-a588-9ca8180c4dbf">Zoom meetings client feedback.</a>.
		/// </summary>
		/// <param name="feedbackId">The Zoom room id.</param>
		/// <param name="from">
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
		/// <returns>
		/// An array of <see cref="ClientFeedbackDetail">client feedback details</see>.
		/// </returns>
		Task<PaginatedResponseWithTokenAndDateRange<ClientFeedbackDetail>> GetZoomMeetingsClientFeedbackAsync(string feedbackId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// If the <a href="https://support.zoom.us/hc/en-us/articles/115005855266">End of Meeting Feedback Survey</a> option is enabled, attendees will be prompted with a survey window where they can tap either the Thumbs Up or Thumbs Down button that indicates their Zoom meeting experience. <br/>
		/// With this API, you can get information on the attendees' meeting satisfaction. Specify a monthly date range for the query using the from and to query parameters.
		/// The month should fall within the last six months. <br />
		/// To get information on the survey results with negative experiences(indicated by Thumbs Down), use <see cref="GetZoomMeetingsClientFeedbackAsync"/>.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="ClientSatisfactionReport"/> report with a list of client satisfaction reports.</returns>
		Task<ClientSatisfactionReport> GetClientMeetingSatisfactionMetrics(DateTime from, DateTime to, CancellationToken cancellationToken = default);
	}
}
