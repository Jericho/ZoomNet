using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class DashboardsTests
	{
		// I made up this JSON response since Zoom doesn't provide an example for IM metrics
		// In fact, it seems like the IM metrics endpoint is completely undocumented
		private const string IM_METRICS_JSON = @"{
		 	""from"":""2025-12-31"",
		 	""to"":""2026-01-31"",
		 	""page_count"":1,
		 	""page_size"":100,
		 	""next_page_token"":"""",
		 	""total_records"":1,
			""users"": [
				{
					""user_id"":""8lzIwvZTSOqjndWPbPqzuA"",
					""user_name"":""John Doe"",
					""email"":""johndoe@example.com"",
					""total_send"":8,
					""total_receive"":0,
					""group_send"":6,
					""group_receive"":0,
					""calls_send"":0,
					""calls_receive"":0,
					""files_send"":2,
					""files_receive"":0,
					""images_send"":4,
					""images_receive"":0,
					""voice_send"":0,
					""voice_receive"":0,
					""videos_send"":0,
					""videos_receive"":0,
					""emoji_send"":0,
					""emoji_receive"":0
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public DashboardsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllMeetingsAsync Tests

		[Fact]
		public async Task GetAllMeetingsAsync_Live()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings"))
				.WithQueryString("type", "live")
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EndpointsResource.metrics_meetings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllMeetingsAsync(from, to, DashboardMeetingType.Live, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetAllMeetingsAsync_Past()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings"))
				.WithQueryString("type", "past")
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_meetings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllMeetingsAsync(from, to, DashboardMeetingType.Past, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region GetMeetingAsync Tests

		[Fact]
		public async Task GetMeetingAsync()
		{
			// Arrange
			var meetingId = "575734086";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId))
				.WithQueryString("type", "live")
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetMeetingAsync(meetingId, DashboardMeetingType.Live, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetMeetingAsync_PastMeeting()
		{
			// Arrange
			var meetingId = "575734086";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId))
				.WithQueryString("type", "past")
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetMeetingAsync(meetingId, DashboardMeetingType.Past, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region GetMeetingParticipantsAsync Tests

		[Fact]
		public async Task GetMeetingParticipantsAsync()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordsPerPage = 30;
			var pageToken = "token456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants"))
				.WithQueryString("include_fields", "registrant_id")
				.WithQueryString("type", "live")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__participants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetMeetingParticipantsAsync(meetingId, DashboardMeetingType.Live, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		#endregion

		#region QoS Tests

		[Fact]
		public async Task GetMeetingParticipantQosAsync()
		{
			// Arrange
			var meetingId = "meeting123";
			var participantId = "participant1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants", participantId, "qos"))
				.WithQueryString("type", "live")
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__participants__participantId__qos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetMeetingParticipantQosAsync(meetingId, participantId, DashboardMeetingType.Live, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllMeetingParticipantsQosAsync()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordsPerPage = 5;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants", "qos"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__participants_qos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllMeetingParticipantsQosAsync(meetingId, DashboardMeetingType.Live, recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Sharing Details Tests

		[Fact]
		public async Task GetAllMeetingParticipantSharingDetailsAsync()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants", "sharing"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.metrics_meetings__meetingId__participants_sharing_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllMeetingParticipantSharingDetailsAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Webinar Tests

		[Fact]
		public async Task GetAllWebinarsAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars"))
				.WithQueryString("type", "live")
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_webinars_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllWebinarsAsync(from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetWebinarAsync()
		{
			// Arrange
			var webinarId = "webinar123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId))
				.WithQueryString("type", "live")
				.Respond("application/json", EndpointsResource.metrics_webinars__webinarId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetWebinarAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetWebinarParticipantsAsync()
		{
			// Arrange
			var webinarId = "webinar123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId, "participants"))
				.WithQueryString("type", "live")
				.Respond("application/json", EndpointsResource.metrics_webinars__webinarId__participants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetWebinarParticipantsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetWebinarParticipantQosAsync()
		{
			// Arrange
			var webinarId = "webinar123";
			var participantId = "participant1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId, "participants", participantId, "qos"))
				.WithQueryString("type", "live")
				.Respond("application/json", EndpointsResource.metrics_webinars__webinarId__participants__participantId__qos_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetWebinarParticipantQosAsync(webinarId, participantId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllWebinarParticipantQosAsync()
		{
			// Arrange
			var webinarId = "webinar123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId, "participants", "qos"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", "1")
				.Respond("application/json", EndpointsResource.metrics_webinars__webinarId__participants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllWebinarParticipantQosAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllWebinarParticipantSharingDetailsAsync()
		{
			// Arrange
			var webinarId = "webinar123";
			var sharingJson = @"{
				""page_size"": 30,
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId, "participants", "sharing"))
				.WithQueryString("type", "live")
				.Respond("application/json", sharingJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllWebinarParticipantSharingDetailsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Zoom Rooms Tests

		[Fact]
		public async Task GetAllZoomRoomsAsync()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "zoomrooms"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.metrics_zoomrooms_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetAllZoomRoomsAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRoomDetailsAsync()
		{
			// Arrange
			var zoomRoomId = "room123";
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "zoomrooms", zoomRoomId))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.metrics_zoomrooms__zoomroomId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetRoomDetailsAsync(zoomRoomId, from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region CRC and Other Metrics Tests

		[Fact]
		public async Task GetCrcPortUsageAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "crc"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_crc_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetCrcPortUsageAsync(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetImMetricsAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "im"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", IM_METRICS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetImMetricsAsync(from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetClientFeedbackMetricsAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "client", "feedback"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_client_feedback_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetClientFeedbackMetricsAsync(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetIssuesOfZoomRoomsAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "zoomrooms", "issues"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_zoomrooms_issues_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetIssuesOfZoomRoomsAsync(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetZoomRoomsWithIssuesAsync()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "issues", "zoomrooms"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_issues_zoomrooms_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetZoomRoomsWithIssuesAsync(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetIssuesOfZoomRoomAsync()
		{
			// Arrange
			var zoomRoomId = "room123";
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "issues", "zoomrooms", zoomRoomId))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.metrics_issues_zoomrooms__zoomroomId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetIssuesOfZoomRoomAsync(zoomRoomId, from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetZoomMeetingsClientFeedbackAsync()
		{
			// Arrange
			var feedbackId = "feedback123";
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "client", "feedback", feedbackId))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.metrics_client_feedback__feedbackId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetZoomMeetingsClientFeedbackAsync(feedbackId, from, to, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetClientMeetingSatisfactionMetrics()
		{
			// Arrange
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "client", "satisfaction"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", EndpointsResource.metrics_client_satisfaction_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetClientMeetingSatisfactionMetricsAsync(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
