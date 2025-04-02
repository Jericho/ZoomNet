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
		public Task<PaginatedResponseWithTokenAndDateRange<DashboardMeetingMetrics>> GetAllMeetingsAsync(DateTime from, DateTime to, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync("metrics/meetings")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<DashboardMeetingMetrics>("meetings");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingMetrics> GetMeetingAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/meetings/{meetingId}")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMeetingMetrics>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipant>> GetMeetingParticipantsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/meetings/{meetingId}/participants")
				.WithArgument("include_fields", "registrant_id")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardMeetingParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingParticipantQos> GetMeetingParticipantQosAsync(string meetingId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/meetings/{meetingId}/participants/{participantId}/qos")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMeetingParticipantQos>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllMeetingParticipantsQosAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 1, string pageToken = null, CancellationToken cancellationToken = default)
		{
			if (pageSize < 1 || pageSize > 10)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 10");
			}

			return _client
				.GetAsync($"metrics/meetings/{meetingId}/participants/qos")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardMeetingParticipantQos>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllMeetingParticipantSharingDetailsAsync(string meetingId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/meetings/{meetingId}/participants/sharing")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ParticipantSharingDetails>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<DashboardMetricsBase>> GetAllWebinarsAsync(DateTime from, DateTime to, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync("metrics/webinars")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<DashboardMetricsBase>("webinars");
		}

		/// <inheritdoc/>
		public Task<DashboardMetricsBase> GetWebinarAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/webinars/{webinarId}")
				.WithArgument("type", type.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMetricsBase>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardParticipant>> GetWebinarParticipantsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/webinars/{webinarId}/participants")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardParticipant>("participants");
		}

		/// <inheritdoc/>
		public Task<DashboardMeetingParticipantQos> GetWebinarParticipantQosAsync(string webinarId, string participantId, DashboardMeetingType type = DashboardMeetingType.Live, CancellationToken cancellationToken = default)
		{
			return _client
			  .GetAsync($"metrics/webinars/{webinarId}/participants/{participantId}/qos")
			  .WithArgument("type", type.ToEnumString())
			  .WithCancellationToken(cancellationToken)
			  .AsObject<DashboardMeetingParticipantQos>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<DashboardMeetingParticipantQos>> GetAllWebinarParticipantQosAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 1, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize, max: 10);

			return _client
			  .GetAsync($"metrics/webinars/{webinarId}/participants/qos")
			  .WithArgument("type", type.ToEnumString())
			  .WithArgument("page_size", pageSize)
			  .WithArgument("next_page_token", pageToken)
			  .WithCancellationToken(cancellationToken)
			  .AsPaginatedResponseWithToken<DashboardMeetingParticipantQos>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ParticipantSharingDetails>> GetAllWebinarParticipantSharingDetailsAsync(string webinarId, DashboardMeetingType type = DashboardMeetingType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
			  .GetAsync($"metrics/webinars/{webinarId}/participants/sharing")
			  .WithArgument("type", type.ToEnumString())
			  .WithArgument("page_size", pageSize)
			  .WithArgument("next_page_token", pageToken)
			  .WithCancellationToken(cancellationToken)
			  .AsPaginatedResponseWithToken<ParticipantSharingDetails>("participants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ZoomRoom>> GetAllZoomRoomsAsync(int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync("metrics/zoomrooms")
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ZoomRoom>("zoom_rooms");
		}

		/// <inheritdoc/>
		public Task<ZoomRoom> GetRoomDetailsAsync(string zoomRoomId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/zoomrooms/{zoomRoomId}")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsObject<ZoomRoom>();
		}

		/// <inheritdoc/>
		public Task<CrcPortMetrics> GetCrcPortUsageAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/crc")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsObject<CrcPortMetrics>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ImMetric>> GetImMetricsAsync(DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync("metrics/im")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ImMetric>("users");
		}

		/// <inheritdoc/>
		public Task<ClientFeedbackMetricsReport> GetClientFeedbackMetricsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/client/feedback")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsObject<ClientFeedbackMetricsReport>();
		}

		/// <inheritdoc/>
		public Task<IssuesOfZoomRoomsReport> GetIssuesOfZoomRoomsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/zoomrooms/issues")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsObject<IssuesOfZoomRoomsReport>();
		}

		/// <inheritdoc/>
		public Task<ZoomRoomWithIssuesReport> GetZoomRoomsWithIssuesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/issues/zoomrooms")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsObject<ZoomRoomWithIssuesReport>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ZoomRoomIssueDetails>> GetIssuesOfZoomRoomAsync(string zoomRoomId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/issues/zoomrooms/{zoomRoomId}")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ZoomRoomIssueDetails>("issue_details");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<ClientFeedbackDetail>> GetZoomMeetingsClientFeedbackAsync(string feedbackId, DateTime from, DateTime to, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(pageSize);

			return _client
				.GetAsync($"metrics/client/feedback/{feedbackId}")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<ClientFeedbackDetail>("client_feedback_details");
		}

		/// <inheritdoc/>
		public Task<ClientSatisfactionReport> GetClientMeetingSatisfactionMetrics(DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("metrics/client/satisfaction")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsObject<ClientSatisfactionReport>();
		}
	}
}
