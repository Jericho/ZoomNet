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
			result.Id.ShouldBe(5638296721);
			result.Topic.ShouldBe("My Meeting");
			result.UserName.ShouldBe("Jill Chill");
			result.UserEmail.ShouldBe("jchill@example.com");
			result.Duration.ShouldBe(60);
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
			result.NextPageToken.ShouldBe("Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3");
			result.Records.Length.ShouldBe(1);
			result.Records[0].DisplayName.ShouldBe("Jill Chill");
			result.Records[0].Email.ShouldBe("jchill@example.com");
			result.Records[0].Duration.ShouldBe(259);
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
			result.Length.ShouldBe(1);
			result[0].Uuid.ShouldBe("Vg8IdgluR5WDeWIkpJlElQ==");
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
			result.Length.ShouldBe(1);
			result[0].ParticipantName.ShouldBe("Jill Chill");
			result[0].ParticipantEmail.ShouldBe("jchill@example.com");
			result[0].Details.Length.ShouldBe(1);
			result[0].Details[0].Question.ShouldBe("How are you?");
			result[0].Details[0].Answer.ShouldBe("Good");
			result[0].Details[0].PollId.ShouldBe("QalIoKWLTJehBJ8e1xRrbQ");
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
