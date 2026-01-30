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
	public class ContactsTests
	{
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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAllAsync(ContactType.Internal, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(10);
			result.NextPageToken.ShouldBe("R4aF9Oj0fVM2hhezJTEmSKaBSkfesDwGy42");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("v4iyWT1LTfy8QvPG4GTvdg");
			result.Records[0].EmailAddress.ShouldBe("jchill@example.com");
			result.Records[0].FirstName.ShouldBe("Jill");
			result.Records[0].LastName.ShouldBe("Chill");
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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts_GET);

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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts_GET);

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
				.Respond("application/json", EndpointsResource.contacts_GET);

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
			result.Records.Length.ShouldBe(1);
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
				.Respond("application/json", EndpointsResource.contacts_GET);

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
				.Respond("application/json", EndpointsResource.contacts_GET);

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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts__identifier__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var contacts = new Contacts(client);

			// Act
			var result = await contacts.GetAsync(contactId, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("v4iyWT1LTfy8QvPG4GTvdg");
			result.EmailAddress.ShouldBe("jchill@example.com");
			result.FirstName.ShouldBe("Jill");
			result.LastName.ShouldBe("Chill");
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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts__identifier__GET);

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
				.Respond("application/json", EndpointsResource.chat_users_me_contacts__identifier__GET);

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
	}
}
