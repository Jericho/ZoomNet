using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class UsersTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public UsersTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests (Obsolete with page number)

		[Fact]
#pragma warning disable CS0618
		public async Task GetAllAsync_WithPageNumber_DefaultParameters()
		{
			// Arrange
			var recordsPerPage = 30;
			var pageNumber = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users"))
				.WithQueryString("status", "active")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", pageNumber.ToString())
				.Respond("application/json", EndpointsResource.users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAllAsync(recordsPerPage: recordsPerPage, page: pageNumber, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.PageNumber.ShouldBe(1);
			result.PageCount.ShouldBe(1);
			result.RecordsPerPage.ShouldBe(30);
			result.TotalRecords.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllAsync_WithPageNumber_InactiveUsers()
		{
			// Arrange
			var recordsPerPage = 30;
			var pageNumber = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users"))
				.WithQueryString("status", "inactive")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", pageNumber.ToString())
				.Respond("application/json", EndpointsResource.users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAllAsync(UserStatus.Inactive, recordsPerPage: recordsPerPage, page: pageNumber, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllAsync_WithPageNumber_WithRoleFilter()
		{
			// Arrange
			var roleId = "role123";
			var recordsPerPage = 30;
			var pageNumber = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users"))
				.WithQueryString("status", "active")
				.WithQueryString("role_id", roleId)
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", pageNumber.ToString())
				.Respond("application/json", EndpointsResource.users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAllAsync(roleId: roleId, recordsPerPage: recordsPerPage, page: pageNumber, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}
#pragma warning restore CS0618

		#endregion

		#region GetAllAsync Tests (With paging token)

		[Fact]
		public async Task GetAllAsync_WithPagingToken_DefaultParameters()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users"))
				.WithQueryString("status", "active")
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAllAsync(pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
		}

		[Fact]
		public async Task GetAllAsync_WithPagingToken()
		{
			// Arrange
			var pagingToken = "nextPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users"))
				.WithQueryString("status", "active")
				.WithQueryString("page_size", "30")
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.users_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAllAsync(pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync_MinimalParameters()
		{
			// Arrange
			var email = "newuser@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users"))
				.Respond("application/json", EndpointsResource.users_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.CreateAsync(email, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Email.ShouldBe("john.doe@example.com");
		}

		[Fact]
		public async Task CreateAsync_WithAllParameters()
		{
			// Arrange
			var email = "newuser@example.com";
			var firstName = "New";
			var lastName = "User";
			var password = "SecurePass123!";
			var type = UserType.Licensed;
			var createType = UserCreateType.Auto;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users"))
				.Respond("application/json", EndpointsResource.users_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.CreateAsync(email, firstName, lastName, password, type, createType, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_SsoType()
		{
			// Arrange
			var email = "ssouser@example.com";
			var createType = UserCreateType.SSo;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users"))
				.Respond("application/json", EndpointsResource.users_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.CreateAsync(email, createType: createType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region UpdateAsync Tests

		[Fact]
		public async Task UpdateAsync_FirstNameOnly()
		{
			// Arrange
			var userId = "user123";
			var firstName = "Updated";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("users", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdateAsync(userId, firstName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithPhoneNumbers()
		{
			// Arrange
			var userId = "user123";
			var phoneNumbers = new[]
			{
				new PhoneNumber { Country = Country.United_States_of_America, CountryCode = "+1", Number = "555-1234", Type = PhoneType.Office },
				new PhoneNumber { Country = Country.Canada, CountryCode = "+1", Number = "555-5678", Type = PhoneType.Mobile }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("users", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdateAsync(userId, phoneNumbers: phoneNumbers, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithCustomAttributes()
		{
			// Arrange
			var userId = "user123";
			var customAttributes = new[]
			{
				new CustomAttribute { Key = "key1", Name = "name1", Value = "value1" },
				new CustomAttribute { Key = "key2", Name = "name2", Value = "value2" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("users", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdateAsync(userId, customAttributes: customAttributes, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithAllParameters()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("users", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdateAsync(
				userId,
				firstName: "John",
				lastName: "Doe",
				company: "Acme Corp",
				department: "Engineering",
				groupId: "group123",
				hostKey: "123456",
				jobTitle: "Engineer",
				language: "en-US",
				location: "San Francisco",
				manager: "manager@example.com",
				phoneNumbers: new[] { new PhoneNumber { Country = Country.United_States_of_America, Number = "555-1234" } },
				pmi: "1234567890",
				pronouns: "he/him",
				pronounsDisplay: PronounDisplayType.Ask,
				timezone: TimeZones.America_Los_Angeles,
				type: UserType.Licensed,
				usePmi: true,
				personalMeetingRoomName: "johns-room",
				cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId))
				.Respond("application/json", EndpointsResource.users__userId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("user123");
			result.FirstName.ShouldBe("John");
			result.LastName.ShouldBe("Doe");
			result.Email.ShouldBe("john.doe@example.com");
		}

		[Fact]
		public async Task GetAsync_ByEmail()
		{
			// Arrange
			var email = "john.doe@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", email))
				.Respond("application/json", EndpointsResource.users__userId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAsync(email, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region DeleteAsync Tests

		[Fact]
		public async Task DeleteAsync()
		{
			// Arrange
			var userId = "user123";
			var transferEmail = "transfer@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId))
				.WithQueryString("action", "delete")
				.WithQueryString("transfer_email", transferEmail)
				.WithQueryString("transfer_meetings", "true")
				.WithQueryString("transfer_webinars", "true")
				.WithQueryString("transfer_recordings", "true")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteAsync(userId, transferEmail, true, true, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_NoTransfers()
		{
			// Arrange
			var userId = "user123";
			var transferEmail = "transfer@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId))
				.WithQueryString("action", "delete")
				.WithQueryString("transfer_email", transferEmail)
				.WithQueryString("transfer_meetings", "false")
				.WithQueryString("transfer_webinars", "false")
				.WithQueryString("transfer_recordings", "false")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteAsync(userId, transferEmail, false, false, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region DisassociateAsync Tests

		[Fact]
		public async Task DisassociateAsync()
		{
			// Arrange
			var userId = "user123";
			var transferEmail = "transfer@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId))
				.WithQueryString("action", "disassociate")
				.WithQueryString("transfer_email", transferEmail)
				.WithQueryString("transfer_meetings", "true")
				.WithQueryString("transfer_webinars", "false")
				.WithQueryString("transfer_recordings", "true")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DisassociateAsync(userId, transferEmail, true, false, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Assistant Tests

		[Fact]
		public async Task GetAssistantsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "assistants"))
				.Respond("application/json", EndpointsResource.users__userId__assistants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAssistantsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
		}

		[Fact]
		public async Task AddAssistantsByIdAsync()
		{
			// Arrange
			var userId = "user123";
			var assistantIds = new[] { "assistant1", "assistant2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "assistants"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.AddAssistantsByIdAsync(userId, assistantIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddAssistantsByIdAsync_EmptyList_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var assistantIds = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => users.AddAssistantsByIdAsync(userId, assistantIds, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task AddAssistantsByEmailAsync()
		{
			// Arrange
			var userId = "user123";
			var assistantEmails = new[] { "assistant1@example.com", "assistant2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "assistants"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.AddAssistantsByEmailAsync(userId, assistantEmails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddAssistantsByEmailAsync_EmptyList_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var assistantEmails = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => users.AddAssistantsByEmailAsync(userId, assistantEmails, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task DeleteAssistantAsync()
		{
			// Arrange
			var userId = "user123";
			var assistantId = "assistant1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "assistants", assistantId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteAssistantAsync(userId, assistantId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAllAssistantsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "assistants"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteAllAssistantsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Scheduler Tests

		[Fact]
		public async Task GetSchedulersAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "schedulers"))
				.Respond("application/json", EndpointsResource.users__userId__schedulers_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetSchedulersAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteSchedulerAsync()
		{
			// Arrange
			var userId = "user123";
			var schedulerId = "scheduler1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "schedulers", schedulerId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteSchedulerAsync(userId, schedulerId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAllSchedulersAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "schedulers"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteAllSchedulersAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Profile Picture Tests

		[Fact]
		public async Task UploadProfilePictureAsync()
		{
			// Arrange
			var userId = "user123";
			var fileName = "profile.jpg";
			var pictureData = new MemoryStream(Encoding.UTF8.GetBytes("fake image data"));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "picture"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UploadProfilePictureAsync(userId, fileName, pictureData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteProfilePictureAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "picture"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteProfilePictureAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Settings Tests

		[Fact]
		public async Task GetSettingsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "settings"))
				.Respond("application/json", EndpointsResource.users__userId__settings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetSettingsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetMeetingAuthenticationSettingsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "settings"))
				.WithQueryString("option", "meeting_authentication")
				.Respond("application/json", EndpointsResource.users__userId__settings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetMeetingAuthenticationSettingsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequireAuthentication.ShouldBeTrue();
			result.AuthenticationOptions.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRecordingAuthenticationSettingsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "settings"))
				.WithQueryString("option", "recording_authentication")
				.Respond("application/json", EndpointsResource.users__userId__settings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetRecordingAuthenticationSettingsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSecuritySettingsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "settings"))
				.WithQueryString("option", "meeting_security")
				.Respond("application/json", EndpointsResource.users__userId__settings_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetSecuritySettingsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Status Management Tests

		[Fact]
		public async Task DeactivateAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeactivateAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task ReactivateAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.ReactivateAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Password and Security Tests

		[Fact]
		public async Task ChangePasswordAsync()
		{
			// Arrange
			var userId = "user123";
			var password = "NewSecurePassword123!";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "password"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.ChangePasswordAsync(userId, password, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetPermissionsAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "permissions"))
				.Respond("application/json", EndpointsResource.users__userId__permissions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetPermissionsAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
		}

		[Fact]
		public async Task RevokeSsoTokenAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "token"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.RevokeSsoTokenAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Email and Name Validation Tests

		[Fact]
		public async Task CheckEmailInUseAsync()
		{
			// Arrange
			var email = "existing@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", "email"))
				.WithQueryString("email", email)
				.Respond("application/json", EndpointsResource.users_email_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.CheckEmailInUseAsync(email, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBeTrue();
		}

		[Fact]
		public async Task ChangeEmailAsync()
		{
			// Arrange
			var userId = "user123";
			var newEmail = "newemail@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "email"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.ChangeEmailAsync(userId, newEmail, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CheckPersonalMeetingRoomNameInUseAsync()
		{
			// Arrange
			var name = "johns-room";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", "vanity_name"))
				.WithQueryString("vanity_name", name)
				.Respond("application/json", EndpointsResource.users_vanity_name_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.CheckPersonalMeetingRoomNameInUseAsync(name, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBeTrue();
		}

		#endregion

		#region Account Management Tests

		[Fact]
		public async Task SwitchAccountAsync()
		{
			// Arrange
			var userId = "user123";
			var currentAccountId = "account1";
			var newAccountId = "account2";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("accounts", currentAccountId, "users", userId, "account"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.SwitchAccountAsync(userId, currentAccountId, newAccountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Access Token Tests

		[Fact]
		public async Task GetAccessTokenAsync_DefaultTtl()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "token"))
				.WithQueryString("type", "zak")
				.Respond("application/json", EndpointsResource.users__userId__token_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAccessTokenAsync(userId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("zak_token_123");
		}

		[Fact]
		public async Task GetAccessTokenAsync_CustomTtl()
		{
			// Arrange
			var userId = "user123";
			var ttl = 3600;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "token"))
				.WithQueryString("type", "zak")
				.WithQueryString("ttl", ttl.ToString())
				.Respond("application/json", EndpointsResource.users__userId__token_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetAccessTokenAsync(userId, ttl, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("zak_token_456");
		}

		#endregion

		#region Virtual Background Tests

		[Fact]
		public async Task UploadVirtualBackgroundAsync()
		{
			// Arrange
			var userId = "user123";
			var fileName = "background.jpg";
			var pictureData = new MemoryStream(Encoding.UTF8.GetBytes("fake background data"));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "settings", "virtual_backgrounds"))
				.Respond("application/json", EndpointsResource.users__userId__settings_virtual_backgrounds_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.UploadVirtualBackgroundAsync(userId, fileName, pictureData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("bg123");
		}

		[Fact]
		public async Task DeleteVirtualBackgroundAsync()
		{
			// Arrange
			var userId = "user123";
			var fileId = "bg123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("users", userId, "settings", "virtual_backgrounds"))
				.WithQueryString("file_ids", fileId)
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.DeleteVirtualBackgroundAsync(userId, fileId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Presence Status Tests

		[Fact]
		public async Task UpdatePresenceStatusAsync_Available()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "presence_status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdatePresenceStatusAsync(userId, PresenceStatus.Available, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdatePresenceStatusAsync_DoNotDisturb_WithDuration()
		{
			// Arrange
			var userId = "user123";
			var duration = 60;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("users", userId, "presence_status"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			await users.UpdatePresenceStatusAsync(userId, PresenceStatus.DoNotDisturb, duration, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdatePresenceStatusAsync_Unknown_ThrowsException()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act & Assert
			var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(() => users.UpdatePresenceStatusAsync(userId, PresenceStatus.Unknown, cancellationToken: TestContext.Current.CancellationToken));
			exception.ParamName.ShouldBe("status");
			exception.Message.ShouldContain("Unknown");
		}

		[Fact]
		public async Task GetPresenceStatusAsync()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "presence_status"))
				.Respond("application/json", EndpointsResource.users__userId__presence_status_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var users = new Users(client);

			// Act
			var result = await users.GetPresenceStatusAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
