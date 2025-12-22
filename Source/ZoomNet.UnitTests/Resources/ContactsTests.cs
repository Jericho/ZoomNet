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
	public class ContactsTests
	{
		private const string CONTACTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""contacts"": [
				{
					""id"": ""contact1"",
					""email"": ""user1@example.com"",
					""first_name"": ""John"",
					""last_name"": ""Doe"",
					""presence_status"": ""Available"",
					""phone_number"": ""555-1234"",
					""sip_phone_number"": ""123456@sip.example.com"",
					""direct_numbers"": [""555-5678""],
					""extension_number"": ""1001""
				},
				{
					""id"": ""contact2"",
					""email"": ""user2@example.com"",
					""first_name"": ""Jane"",
					""last_name"": ""Smith"",
					""presence_status"": ""Do_Not_Disturb"",
					""phone_number"": ""555-9012"",
					""sip_phone_number"": ""654321@sip.example.com"",
					""direct_numbers"": [""555-3456""],
					""extension_number"": ""1002""
				}
			]
		}";

		private const string SINGLE_CONTACT_JSON = @"{
			""id"": ""contact1"",
			""email"": ""user@example.com"",
			""first_name"": ""John"",
			""last_name"": ""Doe"",
			""presence_status"": ""Available"",
			""phone_number"": ""555-1234"",
			""sip_phone_number"": ""123456@sip.example.com"",
			""direct_numbers"": [""555-5678"", ""555-7890""],
			""extension_number"": ""1001"",
			""im_group_id"": ""group123"",
			""im_group_name"": ""Team Group"",
			""dept"": ""Engineering"",
			""job_title"": ""Software Engineer"",
			""location"": ""New York""
		}";

		private readonly ITestOutputHelper _outputHelper;

		public ContactsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync_InternalContacts()
		{
			// Arrange
			var recordsPerPage = 30;
			var pagingToken = "token123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("type", "company")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(ContactType.Internal, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("contact1");
			result.Records[0].EmailAddress.ShouldBe("user1@example.com");
			result.Records[0].FirstName.ShouldBe("John");
			result.Records[0].LastName.ShouldBe("Doe");
		}

		[Fact]
		public async Task GetAllAsync_ExternalContacts()
		{
			// Arrange
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("type", "external")
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(ContactType.External, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllAsync_DefaultParameters()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("type", "company")
				.WithQueryString("page_size", "30")
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => contacts.GetAllAsync(ContactType.Internal, recordsPerPage: 100, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetAllAsync_RecordsPerPageTooHigh_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act & Assert - Max is 50 for contacts
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => contacts.GetAllAsync(ContactType.Internal, recordsPerPage: 51, cancellationToken: TestContext.Current.CancellationToken));
		}

		#endregion

		#region SearchAsync Tests

		[Fact]
		public async Task SearchAsync_WithKeyword()
		{
			// Arrange
			var keyword = "john";
			var recordsPerPage = 10;
			var pagingToken = "searchtoken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("contacts"))
				.WithQueryString("search_key", keyword)
				.WithQueryString("query_presence_status", true.ToString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.SearchAsync(keyword, true, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task SearchAsync_WithoutPresenceStatus()
		{
			// Arrange
			var keyword = "smith";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("contacts"))
				.WithQueryString("search_key", keyword)
				.WithQueryString("query_presence_status", false.ToString())
				.WithQueryString("page_size", "1")
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.SearchAsync(keyword, false, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SearchAsync_DefaultParameters()
		{
			// Arrange
			var keyword = "test";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("contacts"))
				.WithQueryString("search_key", keyword)
				.WithQueryString("query_presence_status", true.ToString())
				.WithQueryString("page_size", "1")
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.SearchAsync(keyword, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SearchAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var keyword = "test";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => contacts.SearchAsync(keyword, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task SearchAsync_RecordsPerPageTooHigh_ThrowsException()
		{
			// Arrange
			var keyword = "test";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act & Assert - Max is 25 for search
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => contacts.SearchAsync(keyword, recordsPerPage: 26, cancellationToken: TestContext.Current.CancellationToken));
		}

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync_WithContactId()
		{
			// Arrange
			var contactId = "contact1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts", contactId))
				.WithQueryString("contactid", contactId)
				.WithQueryString("query_presence_status", true.ToString())
				.Respond("application/json", SINGLE_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAsync(contactId, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("contact1");
			result.EmailAddress.ShouldBe("user@example.com");
			result.FirstName.ShouldBe("John");
			result.LastName.ShouldBe("Doe");
		}

		[Fact]
		public async Task GetAsync_WithoutPresenceStatus()
		{
			// Arrange
			var contactId = "contact2";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts", contactId))
				.WithQueryString("contactid", contactId)
				.WithQueryString("query_presence_status", false.ToString())
				.Respond("application/json", SINGLE_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAsync(contactId, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAsync_DefaultPresenceStatus()
		{
			// Arrange
			var contactId = "contact3";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts", contactId))
				.WithQueryString("contactid", contactId)
				.WithQueryString("query_presence_status", true.ToString())
				.Respond("application/json", SINGLE_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAsync(contactId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAllAsync_EmptyContacts()
		{
			// Arrange
			var emptyContactsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""contacts"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("type", "company")
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyContactsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
			result.NextPageToken.ShouldBeEmpty();
		}

		[Fact]
		public async Task SearchAsync_NoResults()
		{
			// Arrange
			var keyword = "nonexistent";
			var emptyContactsJson = @"{
				""page_size"": 1,
				""next_page_token"": """",
				""contacts"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("contacts"))
				.WithQueryString("search_key", keyword)
				.Respond("application/json", emptyContactsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.SearchAsync(keyword, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetAllAsync_MinimumRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllAsync_MaximumRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", "me", "contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SearchAsync_MaximumRecordsPerPage()
		{
			// Arrange
			var keyword = "test";
			var recordsPerPage = 25;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("contacts"))
				.WithQueryString("search_key", keyword)
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.SearchAsync(keyword, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
