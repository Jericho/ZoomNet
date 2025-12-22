using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PastMeetingsTests
	{
		private const string PAST_MEETING_JSON = @"{
			""uuid"": ""4444AAAiAAAAAiAiAiiAii=="",
			""id"": 1234567890,
			""host_id"": ""9cm3aBCdEfGhiJkL4m"",
			""type"": 2,
			""topic"": ""My Past Meeting"",
			""user_name"": ""John Doe"",
			""user_email"": ""john@example.com"",
			""start_time"": ""2023-06-01T10:00:00Z"",
			""end_time"": ""2023-06-01T11:30:00Z"",
			""duration"": 90,
			""total_minutes"": 180,
			""participants_count"": 4
		}";

		private const string PARTICIPANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""participants"": [
				{
					""id"": ""participant1"",
					""name"": ""Alice Johnson"",
					""user_email"": ""alice@example.com"",
					""join_time"": ""2023-06-01T10:05:00Z"",
					""leave_time"": ""2023-06-01T11:25:00Z"",
					""duration"": 80,
					""attentiveness_score"": 90,
					""failover"": false
				},
				{
					""id"": ""participant2"",
					""name"": ""Bob Smith"",
					""user_email"": ""bob@example.com"",
					""join_time"": ""2023-06-01T10:10:00Z"",
					""leave_time"": ""2023-06-01T11:20:00Z"",
					""duration"": 70,
					""attentiveness_score"": 85,
					""failover"": false
				}
			]
		}";

		private const string PAST_INSTANCES_JSON = @"{
			""meetings"": [
				{
					""uuid"": ""instance1=="",
					""start_time"": ""2023-05-01T10:00:00Z""
				},
				{
					""uuid"": ""instance2=="",
					""start_time"": ""2023-05-15T10:00:00Z""
				},
				{
					""uuid"": ""instance3=="",
					""start_time"": ""2023-05-29T10:00:00Z""
				}
			]
		}";

		private const string POLL_RESULTS_JSON = @"{
			""questions"": [
				{
					""file_name"": ""Alice Johnson"",
					""email"": ""alice@example.com"",
					""question_details"": [
						{
							""question"": ""What is your favorite color?"",
							""answer"": ""Blue""
						},
						{
							""question"": ""Which programming languages do you know?"",
							""answer"": ""C#;JavaScript;Python""
						}
					]
				},
				{
					""file_name"": ""Bob Smith"",
					""email"": ""bob@example.com"",
					""question_details"": [
						{
							""question"": ""What is your favorite color?"",
							""answer"": ""Red""
						},
						{
							""question"": ""Which programming languages do you know?"",
							""answer"": ""Java;C++;Python""
						}
					]
				}
			]
		}";

		private const string MEETING_FILES_JSON = @"{
			""in_meeting_files"": [
				{
					""file_name"": ""document.pdf"",
					""file_size"": 1024000,
					""download_url"": ""https://zoom.us/rec/download/abc123"",
					""file_extension"": ""pdf""
				},
				{
					""file_name"": ""presentation.pptx"",
					""file_size"": 2048000,
					""download_url"": ""https://zoom.us/rec/download/def456"",
					""file_extension"": ""pptx""
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public PastMeetingsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync_WithValidMeetingId_ReturnsDetails()
		{
			// Arrange
			var meetingId = "1234567890";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId))
				.Respond("application/json", PAST_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
			result.Id.ShouldBe(1234567890);
			result.Topic.ShouldBe("My Past Meeting");
			result.UserName.ShouldBe("John Doe");
			result.UserEmail.ShouldBe("john@example.com");
			result.Duration.ShouldBe(90);
		}

		[Fact]
		public async Task GetAsync_WithUuid_ReturnsDetails()
		{
			// Arrange
			var meetingUuid = "4444AAAiAAAAAiAiAiiAii==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingUuid))
				.Respond("application/json", PAST_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetAsync(meetingUuid, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Uuid.ShouldBe("4444AAAiAAAAAiAiAiiAii==");
		}

		[Fact]
		public async Task GetAsync_WithDifferentMeeting_ReturnsCorrectDetails()
		{
			// Arrange
			var meetingId = "9876543210";
			var differentMeetingJson = @"{
				""uuid"": ""different_uuid=="",
				""id"": 9876543210,
				""host_id"": ""host123"",
				""type"": 2,
				""topic"": ""Different Meeting"",
				""user_name"": ""Jane Doe"",
				""user_email"": ""jane@example.com"",
				""start_time"": ""2023-07-01T14:00:00Z"",
				""end_time"": ""2023-07-01T15:00:00Z"",
				""duration"": 60,
				""total_minutes"": 120,
				""participants_count"": 2
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId))
				.Respond("application/json", differentMeetingJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(9876543210);
			result.Topic.ShouldBe("Different Meeting");
			result.UserName.ShouldBe("Jane Doe");
		}

		#endregion

		#region GetParticipantsAsync Tests

		[Fact]
		public async Task GetParticipantsAsync_DefaultParameters_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "1234567890";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId, "participants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetParticipantsAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.Length.ShouldBe(2);
			result.Records[0].DisplayName.ShouldBe("Alice Johnson");
			result.Records[0].Email.ShouldBe("alice@example.com");
			result.Records[0].Duration.ShouldBe(80);
			result.Records[1].DisplayName.ShouldBe("Bob Smith");
			result.Records[1].Email.ShouldBe("bob@example.com");
			result.Records[1].Duration.ShouldBe(70);
		}

		[Fact]
		public async Task GetParticipantsAsync_WithPagination_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "1234567890";
			var recordsPerPage = 10;
			var pagingToken = "customToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId, "participants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetParticipantsAsync(meetingId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetParticipantsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var meetingId = "1234567890";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(async () =>
			{
				await pastMeetings.GetParticipantsAsync(meetingId, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		[Fact]
		public async Task GetParticipantsAsync_EmptyParticipants_ReturnsEmptyArray()
		{
			// Arrange
			var meetingId = "1234567890";
			var emptyParticipantsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId, "participants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyParticipantsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetParticipantsAsync(meetingId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetParticipantsAsync_MaxRecordsPerPage_ReturnsParticipants()
		{
			// Arrange
			var meetingId = "1234567890";
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId, "participants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetParticipantsAsync(meetingId, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region GetInstancesAsync Tests

		[Fact]
		public async Task GetInstancesAsync_WithValidMeetingId_ReturnsInstances()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "instances"))
				.Respond("application/json", PAST_INSTANCES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetInstancesAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
			result[0].Uuid.ShouldBe("instance1==");
			result[1].Uuid.ShouldBe("instance2==");
			result[2].Uuid.ShouldBe("instance3==");
		}

		[Fact]
		public async Task GetInstancesAsync_EmptyInstances_ReturnsEmptyArray()
		{
			// Arrange
			var meetingId = 1234567890L;
			var emptyInstancesJson = @"{""meetings"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "instances"))
				.Respond("application/json", emptyInstancesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetInstancesAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetInstancesAsync_SingleInstance_ReturnsOneInstance()
		{
			// Arrange
			var meetingId = 1234567890L;
			var singleInstanceJson = @"{
				""meetings"": [
					{
						""uuid"": ""single_instance=="",
						""start_time"": ""2023-06-15T10:00:00Z""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "instances"))
				.Respond("application/json", singleInstanceJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetInstancesAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Uuid.ShouldBe("single_instance==");
		}

		#endregion

		#region GetPollResultsAsync Tests

		[Fact]
		public async Task GetPollResultsAsync_WithValidMeetingId_ReturnsPollResults()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", POLL_RESULTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetPollResultsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].ParticipantName.ShouldBe("Alice Johnson");
			result[0].ParticipantEmail.ShouldBe("alice@example.com");
			result[0].Details.Length.ShouldBe(2);
			result[1].ParticipantName.ShouldBe("Bob Smith");
			result[1].ParticipantEmail.ShouldBe("bob@example.com");
			result[1].Details.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetPollResultsAsync_EmptyPollResults_ReturnsEmptyArray()
		{
			// Arrange
			var meetingId = 1234567890L;
			var emptyPollResultsJson = @"{""questions"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", emptyPollResultsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetPollResultsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetPollResultsAsync_SingleParticipant_ReturnsOneResult()
		{
			// Arrange
			var meetingId = 1234567890L;
			var singleParticipantJson = @"{
				""questions"": [
					{
						""file_name"": ""Carol White"",
						""email"": ""carol@example.com"",
						""question_details"": [
							{
								""question"": ""Are you satisfied?"",
								""answer"": ""Yes""
							}
						]
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", singleParticipantJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetPollResultsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].ParticipantName.ShouldBe("Carol White");
			result[0].Details.Length.ShouldBe(1);
		}

		#endregion

		#region GetFilesAsync Tests

		[Fact]
		public async Task GetFilesAsync_WithValidMeetingId_ReturnsFiles()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "files"))
				.Respond("application/json", MEETING_FILES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await pastMeetings.GetFilesAsync(meetingId, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Name.ShouldBe("document.pdf");
			result[0].Size.ShouldBe(1024000);
			result[1].Name.ShouldBe("presentation.pptx");
			result[1].Size.ShouldBe(2048000);
		}

		[Fact]
		public async Task GetFilesAsync_EmptyFiles_ReturnsEmptyArray()
		{
			// Arrange
			var meetingId = 1234567890L;
			var emptyFilesJson = @"{""in_meeting_files"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "files"))
				.Respond("application/json", emptyFilesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await pastMeetings.GetFilesAsync(meetingId, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetFilesAsync_SingleFile_ReturnsOneFile()
		{
			// Arrange
			var meetingId = 1234567890L;
			var singleFileJson = @"{
				""in_meeting_files"": [
					{
						""file_name"": ""notes.txt"",
						""file_size"": 5120,
						""download_url"": ""https://zoom.us/rec/download/xyz789"",
						""file_extension"": ""txt""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "files"))
				.Respond("application/json", singleFileJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await pastMeetings.GetFilesAsync(meetingId, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Name.ShouldBe("notes.txt");
			result[0].Size.ShouldBe(5120);
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetParticipantsAsync_WithNullPagingToken_WorksCorrectly()
		{
			// Arrange
			var meetingId = "1234567890";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId, "participants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetParticipantsAsync(meetingId, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAsync_WithSpecialCharactersInId_WorksCorrectly()
		{
			// Arrange
			var meetingId = "abc+123==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId))
				.Respond("application/json", PAST_MEETING_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetInstancesAsync_WithMultipleInstances_ReturnsAllInstances()
		{
			// Arrange
			var meetingId = 1234567890L;
			var manyInstancesJson = @"{
				""meetings"": [
					{""uuid"": ""inst1=="", ""start_time"": ""2023-01-01T10:00:00Z""},
					{""uuid"": ""inst2=="", ""start_time"": ""2023-01-08T10:00:00Z""},
					{""uuid"": ""inst3=="", ""start_time"": ""2023-01-15T10:00:00Z""},
					{""uuid"": ""inst4=="", ""start_time"": ""2023-01-22T10:00:00Z""},
					{""uuid"": ""inst5=="", ""start_time"": ""2023-01-29T10:00:00Z""}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "instances"))
				.Respond("application/json", manyInstancesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetInstancesAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(5);
		}

		[Fact]
		public async Task GetPollResultsAsync_WithMultipleAnswers_ReturnsAllAnswers()
		{
			// Arrange
			var meetingId = 1234567890L;
			var multipleAnswersJson = @"{
				""questions"": [
					{
						""file_name"": ""User1"",
						""email"": ""user1@example.com"",
						""question_details"": [
							{""question"": ""Rate our service"", ""answer"": ""5""},
							{""question"": ""Would you recommend?"", ""answer"": ""Yes""},
							{""question"": ""Any comments?"", ""answer"": ""Great service!""}
						]
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", multipleAnswersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastMeetings = new PastMeetings(client);

			// Act
			var result = await pastMeetings.GetPollResultsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Details.Length.ShouldBe(3);
		}

		#endregion
	}
}
