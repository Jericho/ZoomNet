using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Dashboards : IDashboards
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dashboards" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Dashboards(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<DashboardMeetingMetrics>> GetAllMeetingsAsync(DateOnly from, DateOnly to, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("metrics/meetings")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<DashboardMeetingMetrics>("meetings");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingMetrics> GetMeetingAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/meetings/{Utils.EncodeUUID(meetingId)}")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMeetingMetrics>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipant>> GetMeetingParticipantsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/meetings/{Utils.EncodeUUID(meetingId)}/participants")
				.WithArgument("include_fields", "registrant_id")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardMeetingParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingParticipantQos> GetMeetingParticipantQosAsync(string meetingId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/meetings/{Utils.EncodeUUID(meetingId)}/participants/{participantId}/qos")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMeetingParticipantQos>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllMeetingParticipantsQosAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 1, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 10);

			return _client
				.GetAsync($"metrics/meetings/{Utils.EncodeUUID(meetingId)}/participants/qos")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardMeetingParticipantQos>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllMeetingParticipantSharingDetailsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/meetings/{Utils.EncodeUUID(meetingId)}/participants/sharing")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ParticipantSharingDetails>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<DashboardMetricsBase>> GetAllWebinarsAsync(DateOnly from, DateOnly to, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("metrics/webinars")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<DashboardMetricsBase>("webinars");
		}

		/// <inheritdoc/>
		public Task<DashboardMetricsBase> GetWebinarAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/webinars/{Utils.EncodeUUID(webinarId)}")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMetricsBase>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardParticipant>> GetWebinarParticipantsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/webinars/{Utils.EncodeUUID(webinarId)}/participants")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingParticipantQos> GetWebinarParticipantQosAsync(string webinarId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
			  .GetAsync($"metrics/webinars/{Utils.EncodeUUID(webinarId)}/participants/{participantId}/qos")
			  .WithArgument("type", type.ToEnumString())
			  .WithCancellationToken(cancellationToken)
			  .AsObject<DashboardMeetingParticipantQos>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllWebinarParticipantQosAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 1, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 10);

			return _client
			  .GetAsync($"metrics/webinars/{Utils.EncodeUUID(webinarId)}/participants/qos")
			  .WithArgument("type", type.ToEnumString())
			  .WithArgument("page_size", recordsPerPage)
			  .WithArgument("next_page_token", pageToken)
			  .WithCancellationToken(cancellationToken)
			  .AsPaginatedResponseWithToken<DashboardMeetingParticipantQos>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllWebinarParticipantSharingDetailsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
			  .GetAsync($"metrics/webinars/{Utils.EncodeUUID(webinarId)}/participants/sharing")
			  .WithArgument("type", type.ToEnumString())
			  .WithArgument("page_size", recordsPerPage)
			  .WithArgument("next_page_token", pageToken)
			  .WithCancellationToken(cancellationToken)
			  .AsPaginatedResponseWithToken<ParticipantSharingDetails>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ZoomRoom>> GetAllZoomRoomsAsync(int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("metrics/zoomrooms")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ZoomRoom>("zoom_rooms");
		}

		/// <inheritdoc/>
		public Task<ZoomRoom> GetRoomDetailsAsync(string zoomRoomId, DateOnly from, DateOnly to, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/zoomrooms/{zoomRoomId}")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsObject<ZoomRoom>();
		}

		/// <inheritdoc/>
		public Task<CrcPortMetrics> GetCrcPortUsageAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/crc")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithCancellationToken(cancellationToken)
				.AsObject<CrcPortMetrics>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ImMetric>> GetImMetricsAsync(DateOnly from, DateOnly to, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("metrics/im")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ImMetric>("users");
		}

		/// <inheritdoc/>
		public Task<ClientFeedbackMetricsReport> GetClientFeedbackMetricsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/client/feedback")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithCancellationToken(cancellationToken)
				.AsObject<ClientFeedbackMetricsReport>();
		}

		/// <inheritdoc/>
		public Task<IssuesOfZoomRoomsReport> GetIssuesOfZoomRoomsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/zoomrooms/issues")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithCancellationToken(cancellationToken)
				.AsObject<IssuesOfZoomRoomsReport>();
		}

		/// <inheritdoc/>
		public Task<ZoomRoomWithIssuesReport> GetZoomRoomsWithIssuesAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/issues/zoomrooms")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithCancellationToken(cancellationToken)
				.AsObject<ZoomRoomWithIssuesReport>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ZoomRoomIssueDetails>> GetIssuesOfZoomRoomAsync(string zoomRoomId, DateOnly from, DateOnly to, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/issues/zoomrooms/{zoomRoomId}")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ZoomRoomIssueDetails>("issue_details");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ClientFeedbackDetail>> GetZoomMeetingsClientFeedbackAsync(string feedbackId, DateOnly from, DateOnly to, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"metrics/client/feedback/{feedbackId}")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ClientFeedbackDetail>("client_feedback_details");
		}

		/// <inheritdoc/>
		public Task<ClientSatisfactionReport> GetClientMeetingSatisfactionMetricsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/client/satisfaction")
				.WithArgument("from", from.ToZoomFormat())
				.WithArgument("to", to.ToZoomFormat())
				.WithCancellationToken(cancellationToken)
				.AsObject<ClientSatisfactionReport>();
		}
	}
}
