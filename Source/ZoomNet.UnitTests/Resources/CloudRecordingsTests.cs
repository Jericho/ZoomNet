using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class CloudRecordingsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public CloudRecordingsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<Recording>(EndpointsResource.meetings__meetingId__recordings_GET, JsonFormatter.DefaultDeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Uuid.ShouldBe("BOKXuumlTAGXuqwr3bLyuQ==");
			result.Id.ShouldBe(6840331990);
			result.AccountId.ShouldBe("Cx3wERazSgup7ZWRHQM8-w");
			result.HostId.ShouldBe("_0ctZtY0REqWalTmwvrdIw");
			result.Topic.ShouldBe("My Personal Meeting");
			result.StartTime.ToUniversalTime().ShouldBe(new DateTime(2021, 3, 18, 5, 41, 36, DateTimeKind.Utc));
			result.Duration.ShouldBe(20);
			result.TotalSize.ShouldBe(22);
			result.FilesCount.ShouldBe(22);
			result.ShareUrl.ShouldBeNull();
			result.RecordingFiles.ShouldNotBeNull();
			result.RecordingFiles.Length.ShouldBe(1);
			result.RecordingFiles[0].Id.ShouldBe("72576a1f-4e66-4a77-87c4-f13f9808bd76");
			result.RecordingFiles[0].MeetingId.ShouldBe("L0AGOEPVR9m5WSOOs/d+FQ==");
			result.RecordingFiles[0].StartTime.ToUniversalTime().ShouldBe(new DateTime(2021, 3, 18, 5, 41, 36, DateTimeKind.Utc));
			result.RecordingFiles[0].EndTime?.ToUniversalTime().ShouldBe(new DateTime(2021, 3, 18, 5, 41, 36, DateTimeKind.Utc));
			result.RecordingFiles[0].FileType.ShouldBe(RecordingFileType.Video);
			result.RecordingFiles[0].Size.ShouldBe(7220);
			result.RecordingFiles[0].PlayUrl.ShouldBe("https://example.com/rec/play/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			result.RecordingFiles[0].DownloadUrl.ShouldBe("https://example.com/rec/download/Qg75t7xZBtEbAkjdlgbfdngBBBB");
			result.RecordingFiles[0].Status.ShouldBe(RecordingStatus.Completed);
			result.RecordingFiles[0].DeleteTime?.ToUniversalTime().ShouldBe(new DateTime(2021, 3, 18, 5, 41, 36, DateTimeKind.Utc));
			result.RecordingFiles[0].ContentType.ShouldBe(RecordingContentType.SharedScreenWithSpeakerView);
		}

		[Fact]
		public async Task GetRecordingsForUserAsync()
		{
			// Arrange
			var userId = "uET9c2fCR06UoPbeqKed4A";
			var from = new DateOnly(2022, 1, 1);
			var to = new DateOnly(2022, 4, 1);
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, from, to, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(recordsPerPage);
			result.NextPageToken.ShouldBe("Tva2CuIdTgsv8wAnhyAdU3m06Y2HuLQtlh3");
			result.MoreRecordsAvailable.ShouldBeTrue();
			result.TotalRecords.ShouldBe(1);
			result.From.ShouldBe(from);
			result.To.ShouldBe(to);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		/// <summary>
		/// This unit test simulates a scenario where we attempt to download a file but our oAuth token has expired.
		/// In this situation, we expect the token to be refreshed and the download request to be reissued.
		/// See <a href="https://github.com/Jericho/ZoomNet/issues/348">Issue 348</a> for more details.
		/// </summary>
		[Fact]
		public async Task DownloadFileAsync_with_expired_token()
		{
			// Arrange
			var downloadUrl = "http://dummywebsite.com/dummyfile.txt";

			var mockTokenHttp = new MockHttpMessageHandler();
			mockTokenHttp // Issue a new token
				.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond(HttpStatusCode.OK, "application/json", "{\"refresh_token\":\"new refresh token\",\"access_token\":\"new access token\"}");

			var mockHttp = new MockHttpMessageHandler();
			mockHttp // The first time the file is requested, we return "401 Unauthorized" to simulate an expired token.
				.Expect(HttpMethod.Get, downloadUrl)
				.Respond(HttpStatusCode.Unauthorized, new StringContent("{\"code\":123456,\"message\":\"access token is expired\"}"));
			mockHttp // The second time the file is requested, we return "200 OK" with the file content.
				.Expect(HttpMethod.Get, downloadUrl)
				.Respond(HttpStatusCode.OK, new StringContent("This is the content of the file"));

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, mockTokenHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.DownloadFileAsync(downloadUrl, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		/// <summary>
		/// While researching <a href="https://github.com/Jericho/ZoomNet/issues/348">Issue 348</a>, it was discovered
		/// that the OAuth session token took precendence over the alternate token specified when invoking DownloadFileAsync.
		/// This unit test was used to demonstrate the problem and ultimately to demonstrate that it was fixed.
		/// </summary>
		[Fact]
		public async Task DownloadFileAsync_with_alternate_token()
		{
			// Arrange
			var downloadUrl = "http://dummywebsite.com/dummyfile.txt";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(HttpMethod.Get, downloadUrl)
				.With(request => request.Headers.Authorization?.Parameter == "alternate_download_token")
				.Respond(HttpStatusCode.OK, new StringContent("This is the content of the file"));

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(
				"MyClientId",
				"MyClientSecret",
				"MyAccountId",
				accessToken: "Expired_token");

			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, null);

			// Act
			var result = await client.CloudRecordings.DownloadFileAsync(downloadUrl, "alternate_download_token", TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#region Recording Settings Tests

		[Fact]
		public async Task GetRecordingSettingsAsync()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "settings"))
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_settings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingSettingsAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteRecordingFilesAsync_MoveToTrash()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId, "recordings"))
				.WithQueryString("action", "trash")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.DeleteRecordingFilesAsync(meetingId, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteRecordingFilesAsync_Permanent()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId, "recordings"))
				.WithQueryString("action", "delete")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.DeleteRecordingFilesAsync(meetingId, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteRecordingFileAsync_SingleFile_Trash()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordingFileId = "file456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId, "recordings", recordingFileId))
				.WithQueryString("action", "trash")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.DeleteRecordingFileAsync(meetingId, recordingFileId, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteRecordingFileAsync_SingleFile_Permanent()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordingFileId = "file456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("meetings", meetingId, "recordings", recordingFileId))
				.WithQueryString("action", "delete")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.DeleteRecordingFileAsync(meetingId, recordingFileId, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RecoverRecordingFilesAsync_AllFiles()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "status"))
				.WithQueryString("action", "recover")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.RecoverRecordingFilesAsync(meetingId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RecoverRecordingFileAsync_SingleFile()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordingFileId = "file456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", recordingFileId, "status"))
				.WithQueryString("action", "recover")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.RecoverRecordingFileAsync(meetingId, recordingFileId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Registrants Tests

		[Fact]
		public async Task GetRecordingRegistrantsAsync_WithPageNumber()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordsPerPage = 30;
			var page = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", page.ToString())
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await recordings.GetRecordingRegistrantsAsync(meetingId, recordsPerPage, page, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingRegistrantsAsync_WithPagingToken()
		{
			// Arrange
			var meetingId = "meeting123";
			var recordsPerPage = 30;
			var pagingToken = "token123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingRegistrantsAsync(meetingId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingRegistrantsAsync_WithDefaultParameters()
		{
			// Arrange
			var meetingId = "meeting123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingRegistrantsAsync(meetingId, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_QueryTrash_True()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("trash", "true")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, queryTrash: true, recordsPerPage: 50, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_QueryTrash_False()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("trash", "false")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, null, null, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_WithDateRange()
		{
			// Arrange
			var userId = "user123";
			var from = new DateOnly(2022, 1, 1);
			var to = new DateOnly(2022, 4, 1);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("from", "2022-01-01")
				.WithQueryString("to", "2022-04-01")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, from: from, to: to, recordsPerPage: 50, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.From.ShouldBe(from);
			result.To.ShouldBe(to);
		}

		[Fact]
		public async Task AddRegistrantAsync_MinimalFields()
		{
			// Arrange
			var meetingId = 123456789L;
			var email = "user@example.com";
			var firstName = "John";
			var lastName = "Doe";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants"))
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.AddRegistrantAsync(meetingId, email, firstName, lastName,
				null, null, null, null, null, null, null, null, null, null, null, null, null,
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RegistrantId.ShouldBe("3Z7sEm0TQQieLav3c3OD_g");
		}

		[Fact]
		public async Task AddRegistrantAsync_AllFields()
		{
			// Arrange
			var meetingId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants"))
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.AddRegistrantAsync(
				meetingId,
				"user@example.com",
				"John",
				"Doe",
				"123 Main St",
				"New York",
				"US",
				"10001",
				"NY",
				"555-1234",
				"Technology",
				"Tech Corp",
				"Developer",
				"1-3 months",
				"Decision Maker",
				"50-100",
				"Looking forward",
				TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task ApproveRegistrantAsync_SingleRegistrant()
		{
			// Arrange
			var meetingId = 123456789L;
			var registrantId = "reg123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants", "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.ApproveRegistrantAsync(meetingId, registrantId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task ApproveRegistrantsAsync_MultipleRegistrants()
		{
			// Arrange
			var meetingId = 123456789L;
			var registrantIds = new[] { "reg1", "reg2", "reg3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants", "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.ApproveRegistrantsAsync(meetingId, registrantIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantAsync_SingleRegistrant()
		{
			// Arrange
			var meetingId = 123456789L;
			var registrantId = "reg123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants", "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.RejectRegistrantAsync(meetingId, registrantId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantsAsync_MultipleRegistrants()
		{
			// Arrange
			var meetingId = 123456789L;
			var registrantIds = new[] { "reg1", "reg2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("meetings", meetingId, "recordings", "registrants", "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			await recordings.RejectRegistrantsAsync(meetingId, registrantIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Recording Information Tests

		[Fact]
		public async Task GetRecordingInformationAsync_CustomTtl()
		{
			// Arrange
			var meetingId = "meeting123";
			var ttl = 600; // 10 minutes

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("meetings", meetingId, "recordings"))
				.WithQueryString("include_fields", "download_access_token")
				.WithQueryString("ttl", ttl.ToString())
				.Respond("application/json", EndpointsResource.meetings__meetingId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingInformationAsync(meetingId, ttl, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Additional Edge Case and Parameter Variation Tests

		[Fact]
		public async Task GetRecordingsForUserAsync_WithPageNumber_Deprecated()
		{
			// Arrange
			var userId = "user123";
			var from = new DateOnly(2023, 1, 1);
			var to = new DateOnly(2023, 1, 31);
			var recordsPerPage = 50;
			var page = 2;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("trash", "false")
				.WithQueryString("from", "2023-01-01")
				.WithQueryString("to", "2023-01-31")
				.WithQueryString("page_size", "50")
				.WithQueryString("page_number", "2")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await recordings.GetRecordingsForUserAsync(userId, false, from, to, recordsPerPage, page, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_MinimalParameters()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("trash", "false")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, null, null, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_WithMaxRecordsPerPage()
		{
			// Arrange
			var userId = "user123";
			var recordsPerPage = 300; // Max allowed

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, null, null, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_WithOnlyFromDate()
		{
			// Arrange
			var userId = "user123";
			var from = new DateOnly(2023, 6, 1);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("from", "2023-06-01")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, from, null, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingsForUserAsync_WithOnlyToDate()
		{
			// Arrange
			var userId = "user123";
			var to = new DateOnly(2023, 6, 30);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings"))
				.WithQueryString("to", "2023-06-30")
				.Respond("application/json", EndpointsResource.users__userId__recordings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, null, to, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
