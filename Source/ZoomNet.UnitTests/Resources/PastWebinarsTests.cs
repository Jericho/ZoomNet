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
	public class PastWebinarsTests
	{
		// This JSON string is necessary because "GET /past_webinars/{webinarId}/files" is obsolete and no longer documented which prevents us from generating a sample JSON
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
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__absentees_GET);

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
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__participants_GET);

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

		#endregion

		#region GetInstancesAsync Tests

		[Fact]
		public async Task GetInstancesAsync_WithValidWebinarId_ReturnsInstances()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "instances"))
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__instances_GET);

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

		#endregion

		#region GetPollResultsAsync Tests

		[Fact]
		public async Task GetPollResultsAsync_WithValidWebinarId_ReturnsPollResults()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__polls_GET);

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

		#endregion

		#region GetQuestionsAndAnswersResultsAsync Tests

		[Fact]
		public async Task GetQuestionsAndAnswersResultsAsync_WithValidWebinarId_ReturnsQAResults()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", webinarId.ToString(), "qa"))
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__qa_GET);

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
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await pastWebinars.GetFilesAsync(webinarId, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

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

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAbsenteesAsync_WithSpecialCharactersInUuid_WorksCorrectly()
		{
			// Arrange
			var webinarUuid = "special+chars/uuid==";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("past_webinars", ZoomNet.Utilities.Utils.EncodeUUID(webinarUuid), "absentees"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.past_webinars__webinarId__absentees_GET);

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

		#endregion
	}
}
