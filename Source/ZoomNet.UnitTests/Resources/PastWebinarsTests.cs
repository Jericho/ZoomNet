using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class PastWebinarsTests
	{
		private const string ABSENTEES_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""registrants"": [
				{
					""id"": ""absentee1"",
					""email"": ""alice@example.com"",
					""first_name"": ""Alice"",
					""last_name"": ""Johnson"",
					""status"": ""approved"",
					""create_time"": ""2023-06-01T09:00:00Z"",
					""join_url"": ""https://zoom.us/w/123456""
				},
				{
					""id"": ""absentee2"",
					""email"": ""bob@example.com"",
					""first_name"": ""Bob"",
					""last_name"": ""Smith"",
					""status"": ""approved"",
					""create_time"": ""2023-06-01T09:15:00Z"",
					""join_url"": ""https://zoom.us/w/789012""
				}
			]
		}";

		private const string PARTICIPANTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token456"",
			""participants"": [
				{
					""id"": ""participant1"",
					""name"": ""Carol White"",
					""user_email"": ""carol@example.com"",
					""join_time"": ""2023-06-01T10:05:00Z"",
					""leave_time"": ""2023-06-01T11:30:00Z"",
					""duration"": 85,
					""failover"": false
				},
				{
					""id"": ""participant2"",
					""name"": ""David Brown"",
					""user_email"": ""david@example.com"",
					""join_time"": ""2023-06-01T10:10:00Z"",
					""leave_time"": ""2023-06-01T11:25:00Z"",
					""duration"": 75,
					""failover"": false
				}
			]
		}";

		private const string PAST_INSTANCES_JSON = @"{
			""webinars"": [
				{
					""uuid"": ""webinar_instance1=="",
					""start_time"": ""2023-05-01T14:00:00Z""
				},
				{
					""uuid"": ""webinar_instance2=="",
					""start_time"": ""2023-05-15T14:00:00Z""
				},
				{
					""uuid"": ""webinar_instance3=="",
					""start_time"": ""2023-05-29T14:00:00Z""
				}
			]
		}";

		private const string POLL_RESULTS_JSON = @"{
			""questions"": [
				{
					""file_name"": ""Carol White"",
					""email"": ""carol@example.com"",
					""question_details"": [
						{
							""question"": ""How satisfied are you with this webinar?"",
							""answer"": ""Very Satisfied""
						},
						{
							""question"": ""Would you recommend this webinar?"",
							""answer"": ""Yes""
						}
					]
				},
				{
					""file_name"": ""David Brown"",
					""email"": ""david@example.com"",
					""question_details"": [
						{
							""question"": ""How satisfied are you with this webinar?"",
							""answer"": ""Satisfied""
						},
						{
							""question"": ""Would you recommend this webinar?"",
							""answer"": ""Yes""
						}
					]
				}
			]
		}";

		private const string QA_RESULTS_JSON = @"{
			""questions"": [
				{
					""file_name"": ""Attendee1"",
					""email"": ""attendee1@example.com"",
					""question_details"": [
						{
							""question"": ""What is the best practice for X?"",
							""answer"": ""The best practice is to do Y""
						}
					]
				},
				{
					""file_name"": ""Attendee2"",
					""email"": ""attendee2@example.com"",
					""question_details"": [
						{
							""question"": ""How do I configure Z?"",
							""answer"": ""You can configure Z by following these steps...""
						}
					]
				}
			]
		}";

		private const string WEBINAR_FILES_JSON = @"{
			""in_meeting_files"": [
				{
					""file_name"": ""webinar_slides.pdf"",
					""file_size"": 5242880,
					""download_url"": ""https://zoom.us/rec/download/webinar123"",
					""file_extension"": ""pdf""
				},
				{
					""file_name"": ""resources.zip"",
					""file_size"": 10485760,
					""download_url"": ""https://zoom.us/rec/download/webinar456"",
					""file_extension"": ""zip""
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public PastWebinarsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAbsenteesAsync Tests

		[Fact]
		public async Task GetAbsenteesAsync_DefaultParameters_ReturnsAbsentees()
		{
			// Arrange
			var webinarUuid = "webinar_uuid_123==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarUuid, "absentees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", ABSENTEES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetAbsenteesAsync(webinarUuid, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("absentee1");
			result.Records[0].Email.ShouldBe("alice@example.com");
			result.Records[0].FirstName.ShouldBe("Alice");
			result.Records[0].LastName.ShouldBe("Johnson");
			result.Records[1].Id.ShouldBe("absentee2");
			result.Records[1].Email.ShouldBe("bob@example.com");
		}

		[Fact]
		public async Task GetAbsenteesAsync_WithPagination_ReturnsAbsentees()
		{
			// Arrange
			var webinarUuid = "webinar_uuid_123==";
			var recordsPerPage = 10;
			var pagingToken = "customToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarUuid, "absentees"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", ABSENTEES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetAbsenteesAsync(webinarUuid, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAbsenteesAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var webinarUuid = "webinar_uuid_123==";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => pastWebinars.GetAbsenteesAsync(webinarUuid, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetAbsenteesAsync_EmptyAbsentees_ReturnsEmptyArray()
		{
			// Arrange
			var webinarUuid = "webinar_uuid_123==";
			var emptyAbsenteesJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""registrants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarUuid, "absentees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyAbsenteesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetAbsenteesAsync(webinarUuid, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetParticipantsAsync Tests

		[Fact]
		public async Task GetParticipantsAsync_DefaultParameters_ReturnsParticipants()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "participants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetParticipantsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token456");
			result.Records.Length.ShouldBe(2);
			result.Records[0].DisplayName.ShouldBe("Carol White");
			result.Records[0].Email.ShouldBe("carol@example.com");
			result.Records[0].Duration.ShouldBe(85);
			result.Records[1].DisplayName.ShouldBe("David Brown");
			result.Records[1].Email.ShouldBe("david@example.com");
			result.Records[1].Duration.ShouldBe(75);
		}

		[Fact]
		public async Task GetParticipantsAsync_WithPagination_ReturnsParticipants()
		{
			// Arrange
			var webinarId = 9876543210L;
			var recordsPerPage = 20;
			var pagingToken = "nextPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "participants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetParticipantsAsync(webinarId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

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
			var webinarId = 9876543210L;
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => pastWebinars.GetParticipantsAsync(webinarId, recordsPerPage: -1, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetParticipantsAsync_EmptyParticipants_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = 9876543210L;
			var emptyParticipantsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""participants"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "participants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyParticipantsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetParticipantsAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		#endregion

		#region GetInstancesAsync Tests

		[Fact]
		public async Task GetInstancesAsync_WithValidWebinarId_ReturnsInstances()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "instances"))
				.Respond("application/json", PAST_INSTANCES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetInstancesAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
			result[0].Uuid.ShouldBe("webinar_instance1==");
			result[1].Uuid.ShouldBe("webinar_instance2==");
			result[2].Uuid.ShouldBe("webinar_instance3==");
		}

		[Fact]
		public async Task GetInstancesAsync_EmptyInstances_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = 9876543210L;
			var emptyInstancesJson = @"{""webinars"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "instances"))
				.Respond("application/json", emptyInstancesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetInstancesAsync(webinarId, TestContext.Current.CancellationToken);

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
			var webinarId = 9876543210L;
			var singleInstanceJson = @"{
				""webinars"": [
					{
						""uuid"": ""single_webinar_instance=="",
						""start_time"": ""2023-06-20T14:00:00Z""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "instances"))
				.Respond("application/json", singleInstanceJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetInstancesAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Uuid.ShouldBe("single_webinar_instance==");
		}

		#endregion

		#region GetPollResultsAsync Tests

		[Fact]
		public async Task GetPollResultsAsync_WithValidWebinarId_ReturnsPollResults()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", POLL_RESULTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetPollResultsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].ParticipantName.ShouldBe("Carol White");
			result[0].ParticipantEmail.ShouldBe("carol@example.com");
			result[0].Details.Length.ShouldBe(2);
			result[1].ParticipantName.ShouldBe("David Brown");
			result[1].ParticipantEmail.ShouldBe("david@example.com");
			result[1].Details.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetPollResultsAsync_EmptyPollResults_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = 9876543210L;
			var emptyPollResultsJson = @"{""questions"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", emptyPollResultsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetPollResultsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		#endregion

		#region GetQuestionsAndAnswersResultsAsync Tests

		[Fact]
		public async Task GetQuestionsAndAnswersResultsAsync_WithValidWebinarId_ReturnsQAResults()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "qa"))
				.Respond("application/json", QA_RESULTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetQuestionsAndAnswersResultsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].ParticipantName.ShouldBe("Attendee1");
			result[0].ParticipantEmail.ShouldBe("attendee1@example.com");
			result[0].Details.Length.ShouldBe(1);
			result[1].ParticipantName.ShouldBe("Attendee2");
			result[1].ParticipantEmail.ShouldBe("attendee2@example.com");
			result[1].Details.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetQuestionsAndAnswersResultsAsync_EmptyQAResults_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = 9876543210L;
			var emptyQAResultsJson = @"{""questions"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "qa"))
				.Respond("application/json", emptyQAResultsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetQuestionsAndAnswersResultsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		#endregion

		#region GetFilesAsync Tests

		[Fact]
		public async Task GetFilesAsync_WithValidWebinarId_ReturnsFiles()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "files"))
				.Respond("application/json", WEBINAR_FILES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetFilesAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Name.ShouldBe("webinar_slides.pdf");
			result[0].Size.ShouldBe(5242880);
			result[1].Name.ShouldBe("resources.zip");
			result[1].Size.ShouldBe(10485760);
		}

		[Fact]
		public async Task GetFilesAsync_EmptyFiles_ReturnsEmptyArray()
		{
			// Arrange
			var webinarId = 9876543210L;
			var emptyFilesJson = @"{""in_meeting_files"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "files"))
				.Respond("application/json", emptyFilesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetFilesAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAbsenteesAsync_WithSpecialCharactersInUuid_WorksCorrectly()
		{
			// Arrange
			var webinarUuid = "special+chars/uuid==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarUuid, "absentees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", ABSENTEES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetAbsenteesAsync(webinarUuid, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetParticipantsAsync_MaxRecordsPerPage_ReturnsParticipants()
		{
			// Arrange
			var webinarId = 9876543210L;
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "participants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", PARTICIPANTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var pastWebinars = new PastWebinars(client);

			// Act
			var result = await pastWebinars.GetParticipantsAsync(webinarId, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
