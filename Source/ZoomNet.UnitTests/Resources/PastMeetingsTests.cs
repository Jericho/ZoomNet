using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class PastMeetingsTests
	{
		// This JSON string is necessary because "GET /past_meetings/{meetingId}/files" is obsolete and no longer documented which prevents us from generating a sample JSON
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
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__GET);

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
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__GET);

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
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__participants_GET);

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

		#endregion

		#region GetInstancesAsync Tests

		[Fact]
		public async Task GetInstancesAsync_WithValidMeetingId_ReturnsInstances()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "instances"))
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__instances_GET);

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

		#endregion

		#region GetPollResultsAsync Tests

		[Fact]
		public async Task GetPollResultsAsync_WithValidMeetingId_ReturnsPollResults()
		{
			// Arrange
			var meetingId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__polls_GET);

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

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAsync_WithSpecialCharactersInId_WorksCorrectly()
		{
			// Arrange
			var meetingId = "abc+123==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_meetings", meetingId))
				.Respond("application/json", EndpointsResource.past_meetings__meetingId__GET);

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

		#endregion
	}
}
