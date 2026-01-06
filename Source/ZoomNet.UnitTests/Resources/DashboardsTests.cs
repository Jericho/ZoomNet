using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class DashboardsTests
	{
		private const string MEETINGS_JSON = @"{
		  ""from"": ""2022-04-01"",
		  ""to"": ""2022-04-07"",
		  ""next_page_token"": ""Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3"",
		  ""page_count"": 1,
		  ""page_size"": 30,
		  ""total_records"": 1,
		  ""meetings"": [
			{
			  ""host"": ""Jill Chill"",
			  ""audio_quality"": ""good"",
			  ""custom_keys"": [
				{
				  ""key"": ""Host Nation"",
				  ""value"": ""US""
				}
			  ],
			  ""dept"": ""Developers"",
			  ""duration"": ""00:56"",
			  ""email"": ""jchill@example.com"",
			  ""end_time"": ""2022-01-04T07:50:47Z"",
			  ""has_3rd_party_audio"": true,
			  ""has_archiving"": true,
			  ""has_pstn"": true,
			  ""has_recording"": true,
			  ""has_screen_share"": true,
			  ""has_sip"": true,
			  ""has_video"": true,
			  ""has_voip"": true,
			  ""has_manual_captions"": true,
			  ""has_automated_captions"": true,
			  ""id"": 93201235621,
			  ""participants"": 2,
			  ""screen_share_quality"": ""good"",
			  ""session_key"": ""ABC36jaBI145"",
			  ""start_time"": ""2022-01-04T08:04:27Z"",
			  ""topic"": ""Share Now"",
			  ""tracking_fields"": [
				{
				  ""field"": ""Meeting purpose."",
				  ""value"": ""Support""
				}
			  ],
			  ""user_type"": ""Licensed"",
			  ""uuid"": ""gm8s9L+PTEC+FG3sFbd1Cw=="",
			  ""video_quality"": ""good"",
			  ""has_poll"": false,
			  ""has_qa"": false,
			  ""has_survey"": false,
			  ""avg_jointime_cost"": 4.55
			}
		  ]
		}";

		private const string SINGLE_MEETING_JSON = @"{
			""host"": ""API"",
			""custom_keys"": [
				{
					""key"": ""Host Nation"",
					""value"": ""US""
				}
			],
			""dept"": ""Developers"",
			""duration"": ""02:21"",
			""email"": ""user@example.com"",
			""end_time"": ""2022-03-01T10:17:35Z"",
			""has_3rd_party_audio"": true,
			""has_archiving"": true,
			""has_pstn"": true,
			""has_recording"": true,
			""has_screen_share"": true,
			""has_sip"": true,
			""has_video"": true,
			""has_voip"": true,
			""has_manual_captions"": true,
			""has_automated_captions"": true,
			""id"": 575734086,
			""in_room_participants"": 2,
			""participants"": 2,
			""start_time"": ""2022-03-01T10:15:14Z"",
			""topic"": ""API Meeting"",
			""user_type"": ""Licensed"",
			""uuid"": ""gaqOKVN9RAaDHKYWEcASXg=="",
			""has_meeting_summary"": true,
			""has_poll"": true,
			""has_qa"": true,
			""has_survey"": false,
			""avg_jointime_cost"": 3.98
		}";

		private const string PARTICIPANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token456"",
			""participants"": [
				{
					""id"": ""participant1"",
					""user_id"": ""user123"",
					""name"": ""John Doe"",
					""user_email"": ""john@example.com"",
					""join_time"": ""2023-01-15T10:05:00Z"",
					""leave_time"": ""2023-01-15T10:55:00Z"",
					""duration"": 50,
					""attentiveness_score"": 85
				}
			]
		}";

		private const string PARTICIPANT_QOS_JSON = @"{
			""user_id"": ""user123"",
			""user_name"": ""John Doe"",
			""device"": ""Desktop"",
			""ip_address"": ""192.168.1.1"",
			""location"": ""New York, US"",
			""join_time"": ""2023-01-15T10:05:00Z"",
			""leave_time"": ""2023-01-15T10:55:00Z"",
			""pc_name"": ""DESKTOP-ABC"",
			""domain"": ""example.com"",
			""mac_addr"": ""00:11:22:33:44:55"",
			""harddisk_id"": ""disk123"",
			""version"": ""5.13.0""
		}";

		private const string WEBINARS_JSON = @"{
			""from"": ""2022-01-01"",
			""to"": ""2022-01-30"",
			""next_page_token"": ""Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3"",
			""page_count"": 1,
			""page_size"": 30,
			""total_records"": 1,
			""webinars"": [
				{
					""host"": ""user@example.com"",
					""custom_keys"": [
						{
							""key"": ""key1"",
							""value"": ""value1""
						}
					],
					""dept"": ""Developers"",
					""duration"": ""55:01"",
					""email"": ""user@example.com"",
					""end_time"": ""2022-01-13T07:00:46Z"",
					""has_3rd_party_audio"": true,
					""has_archiving"": true,
					""has_pstn"": true,
					""has_recording"": true,
					""has_screen_share"": true,
					""has_sip"": true,
					""has_video"": true,
					""has_voip"": true,
					""has_manual_captions"": true,
					""has_automated_captions"": true,
					""id"": 99264817135,
					""participants"": 1,
					""start_time"": ""2022-01-13T05:27:11Z"",
					""topic"": ""my webinar"",
					""user_type"": ""Licensed"",
					""uuid"": ""NknQtFSgSUSEYt2a9gc13A=="",
					""audio_quality"": ""good"",
					""video_quality"": ""good"",
					""screen_share_quality"": ""good"",
					""has_poll"": false,
					""has_survey"": false
				}
			]
		}";

		private const string ZOOM_ROOMS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""room_token"",
			""zoom_rooms"": [
				{
					""id"": ""room123"",
					""room_name"": ""Conference Room A"",
					""email"": ""room-a@example.com"",
					""account_type"": ""Licensed"",
					""status"": ""Available""
				}
			]
		}";

		private const string ZOOM_ROOM_DETAILS_JSON = @"{
			""id"": ""room123"",
			""room_name"": ""Conference Room A"",
			""email"": ""room-a@example.com"",
			""account_type"": ""Licensed"",
			""status"": ""Available"",
			""from"": ""2023-01-01"",
			""to"": ""2023-01-31""
		}";

		private const string CRC_PORT_METRICS_JSON = @"{
			""from"": ""2023-01-01"",
			""to"": ""2023-01-31"",
			""crc_ports_usage"": [
				{
					""date_time"": ""2023-01-15"",
					""crc_ports_hour_usage"": {
						""hour_0"": 5,
						""hour_1"": 3
					}
				}
			]
		}";

		private const string IM_METRICS_JSON = @"{
			""from"": ""2023-01-01"",
			""to"": ""2023-01-31"",
			""page_size"": 30,
			""next_page_token"": ""im_token"",
			""users"": [
				{
					""user_id"": ""user123"",
					""user_name"": ""John Doe"",
					""email"": ""john@example.com"",
					""total_send"": 100,
					""total_receive"": 150
				}
			]
		}";

		private const string CLIENT_FEEDBACK_JSON = @"{
			""from"": ""2023-01-01"",
			""to"": ""2023-01-31"",
			""total_participants"": 1000,
			""total_feedback"": 50
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
				.Respond("application/json", MEETINGS_JSON);

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
				.Respond("application/json", MEETINGS_JSON);

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
				.Respond("application/json", SINGLE_MEETING_JSON);

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
				.Respond("application/json", SINGLE_MEETING_JSON);

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
				.Respond("application/json", PARTICIPANTS_JSON);

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
				.Respond("application/json", PARTICIPANT_QOS_JSON);

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

			var qosListJson = @"{
				""page_size"": 5,
				""next_page_token"": ""qos_token"",
				""participants"": [
					{
						""user_id"": ""user123"",
						""user_name"": ""John Doe""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants", "qos"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", qosListJson);

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
			var sharingJson = @"{
				""page_size"": 30,
				""next_page_token"": ""share_token"",
				""participants"": [
					{
						""user_id"": ""user123"",
						""user_name"": ""John Doe"",
						""share_amount"": 15
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "meetings", meetingId, "participants", "sharing"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", "30")
				.Respond("application/json", sharingJson);

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
				.Respond("application/json", WEBINARS_JSON);

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
				.Respond("application/json", SINGLE_MEETING_JSON);

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
				.Respond("application/json", PARTICIPANTS_JSON);

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
				.Respond("application/json", PARTICIPANT_QOS_JSON);

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
			var qosListJson = @"{
				""page_size"": 1,
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "webinars", webinarId, "participants", "qos"))
				.WithQueryString("type", "live")
				.WithQueryString("page_size", "1")
				.Respond("application/json", qosListJson);

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
				.Respond("application/json", ZOOM_ROOMS_JSON);

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
				.Respond("application/json", ZOOM_ROOM_DETAILS_JSON);

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
				.Respond("application/json", CRC_PORT_METRICS_JSON);

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
				.Respond("application/json", CLIENT_FEEDBACK_JSON);

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
			var issuesJson = @"{
				""from"": ""2023-01-01"",
				""to"": ""2023-01-31"",
				""zoom_rooms"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "zoomrooms", "issues"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", issuesJson);

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
			var issuesJson = @"{
				""from"": ""2023-01-01"",
				""to"": ""2023-01-31"",
				""zoom_rooms"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "issues", "zoomrooms"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", issuesJson);

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
			var issueDetailsJson = @"{
				""from"": ""2023-01-01"",
				""to"": ""2023-01-31"",
				""page_size"": 30,
				""next_page_token"": """",
				""issue_details"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "issues", "zoomrooms", zoomRoomId))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", issueDetailsJson);

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
			var feedbackDetailsJson = @"{
				""from"": ""2023-01-01"",
				""to"": ""2023-01-31"",
				""page_size"": 30,
				""next_page_token"": """",
				""client_feedback_details"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "client", "feedback", feedbackId))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "30")
				.Respond("application/json", feedbackDetailsJson);

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
			var satisfactionJson = @"{
				""from"": ""2023-01-01"",
				""to"": ""2023-01-31"",
				""satisfaction_percent"": 85
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("metrics", "client", "satisfaction"))
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.Respond("application/json", satisfactionJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var dashboards = new Dashboards(client);

			// Act
			var result = await dashboards.GetClientMeetingSatisfactionMetrics(from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
