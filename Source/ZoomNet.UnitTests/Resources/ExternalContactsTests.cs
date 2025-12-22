using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class ExternalContactsTests
	{
		private const string EXTERNAL_CONTACTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""external_contacts"": [
				{
					""external_contact_id"": ""extContact1"",
					""name"": ""John Doe"",
					""description"": ""Primary contact"",
					""email"": ""john.doe@example.com"",
					""extension_number"": ""1001"",
					""id"": ""custom001"",
					""phone_numbers"": [""+1234567890"", ""+0987654321""],
					""routing_path"": ""default"",
					""auto_call_recorded"": true
				},
				{
					""external_contact_id"": ""extContact2"",
					""name"": ""Jane Smith"",
					""description"": ""Secondary contact"",
					""email"": ""jane.smith@example.com"",
					""extension_number"": ""1002"",
					""id"": ""custom002"",
					""phone_numbers"": [""+1122334455""],
					""routing_path"": ""secondary"",
					""auto_call_recorded"": false
				}
			]
		}";

		private const string SINGLE_EXTERNAL_CONTACT_JSON = @"{
			""external_contact_id"": ""extContact1"",
			""name"": ""John Doe"",
			""description"": ""Primary contact"",
			""email"": ""john.doe@example.com"",
			""extension_number"": ""1001"",
			""id"": ""custom001"",
			""phone_numbers"": [""+1234567890"", ""+0987654321""],
			""routing_path"": ""default"",
			""auto_call_recorded"": true
		}";

		private const string CREATED_EXTERNAL_CONTACT_JSON = @"{
			""external_contact_id"": ""newExtContact"",
			""name"": ""New Contact""
		}";

		private readonly ITestOutputHelper _outputHelper;

		public ExternalContactsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync_WithDefaultParameters()
		{
			// Arrange
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EXTERNAL_CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("token123");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].ExternalContactId.ShouldBe("extContact1");
			result.Records[0].Name.ShouldBe("John Doe");
			result.Records[0].Description.ShouldBe("Primary contact");
			result.Records[0].Email.ShouldBe("john.doe@example.com");
			result.Records[0].ExtensionNumber.ShouldBe("1001");
			result.Records[0].PhoneNumbers.ShouldNotBeNull();
			result.Records[0].PhoneNumbers.Count.ShouldBe(2);
			result.Records[0].AutoCallRecorded.ShouldBeTrue();
		}

		[Fact]
		public async Task GetAllAsync_WithCustomRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 10;
			var nextPageToken = "token456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", nextPageToken)
				.Respond("application/json", EXTERNAL_CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(recordsPerPage, nextPageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllAsync_WithMinimumRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EXTERNAL_CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

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
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => externalContacts.GetAllAsync(recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetAllAsync_RecordsPerPageTooHigh_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => externalContacts.GetAllAsync(recordsPerPage: 301, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetAllAsync_EmptyResults()
		{
			// Arrange
			var emptyContactsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""external_contacts"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyContactsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
			result.NextPageToken.ShouldBeEmpty();
		}

		[Fact]
		public async Task GetAllAsync_WithPagingToken()
		{
			// Arrange
			var nextPageToken = "next_page_token_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", "30")
				.WithQueryString("next_page_token", nextPageToken)
				.Respond("application/json", EXTERNAL_CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(nextPageToken: nextPageToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region GetDetailsAsync Tests

		[Fact]
		public async Task GetDetailsAsync_WithValidId()
		{
			// Arrange
			var externalContactId = "extContact1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts", externalContactId))
				.Respond("application/json", SINGLE_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetDetailsAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ExternalContactId.ShouldBe("extContact1");
			result.Name.ShouldBe("John Doe");
			result.Description.ShouldBe("Primary contact");
			result.Email.ShouldBe("john.doe@example.com");
			result.ExtensionNumber.ShouldBe("1001");
			result.Id.ShouldBe("custom001");
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.Count.ShouldBe(2);
			result.RoutingPath.ShouldBe("default");
			result.AutoCallRecorded.ShouldBeTrue();
		}

		[Fact]
		public async Task GetDetailsAsync_NullId_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentNullException>(() => externalContacts.GetDetailsAsync(null, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetDetailsAsync_EmptyId_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => externalContacts.GetDetailsAsync(string.Empty, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task GetDetailsAsync_WithDifferentContactId()
		{
			// Arrange
			var externalContactId = "differentContact123";
			var customContactJson = @"{
				""external_contact_id"": ""differentContact123"",
				""name"": ""Different Contact"",
				""email"": ""different@example.com""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts", externalContactId))
				.Respond("application/json", customContactJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetDetailsAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ExternalContactId.ShouldBe("differentContact123");
			result.Name.ShouldBe("Different Contact");
		}

		#endregion

		#region AddAsync Tests

		[Fact]
		public async Task AddAsync_WithValidContact()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				Name = "New Contact",
				Description = "Test description",
				Email = "new.contact@example.com",
				ExtensionNumber = "1003",
				PhoneNumbers = ["+1234567890"],
				RoutingPath = "default",
				AutoCallRecorded = true
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("phone", "external_contacts"))
				.Respond("application/json", CREATED_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ExternalContactId.ShouldBe("newExtContact");
			result.Name.ShouldBe("New Contact");
		}

		[Fact]
		public async Task AddAsync_WithMinimalContact()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				Name = "Minimal Contact"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("phone", "external_contacts"))
				.Respond("application/json", CREATED_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task AddAsync_WithCompleteContact()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				Name = "Complete Contact",
				Description = "Fully detailed contact",
				Email = "complete@example.com",
				ExtensionNumber = "1004",
				Id = "custom004",
				PhoneNumbers = new List<string> { "+1234567890", "+0987654321", "+1122334455" },
				RoutingPath = "custom_path",
				AutoCallRecorded = false
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("phone", "external_contacts"))
				.Respond("application/json", CREATED_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task AddAsync_WithEmptyPhoneNumbersList()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				Name = "Contact without phones",
				PhoneNumbers = new List<string>()
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("phone", "external_contacts"))
				.Respond("application/json", CREATED_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region DeleteAsync Tests

		[Fact]
		public async Task DeleteAsync_WithValidId()
		{
			// Arrange
			var externalContactId = "extContact1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("phone", "external_contacts", externalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.DeleteAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_NullId_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentNullException>(() => externalContacts.DeleteAsync(null, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task DeleteAsync_EmptyId_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => externalContacts.DeleteAsync(string.Empty, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task DeleteAsync_WithDifferentContactId()
		{
			// Arrange
			var externalContactId = "toBeDeleted456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("phone", "external_contacts", externalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.DeleteAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region UpdateAsync Tests

		[Fact]
		public async Task UpdateAsync_WithValidContact()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = "extContact1",
				Name = "Updated Name",
				Description = "Updated description",
				Email = "updated@example.com",
				ExtensionNumber = "1005",
				PhoneNumbers = new List<string> { "+9999999999" },
				RoutingPath = "updated_path",
				AutoCallRecorded = false
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "external_contacts", externalContact.ExternalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_NullContact_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentNullException>(() => externalContacts.UpdateAsync(null, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task UpdateAsync_NullExternalContactId_ThrowsException()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = null,
				Name = "Contact without ID"
			};

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentNullException>(() => externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task UpdateAsync_EmptyExternalContactId_ThrowsException()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = string.Empty,
				Name = "Contact with empty ID"
			};

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task UpdateAsync_WithPartialUpdate()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = "extContact2",
				Name = "Partially Updated Contact"
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "external_contacts", externalContact.ExternalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithCompleteContact()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = "extContact3",
				Name = "Fully Updated Contact",
				Description = "Complete update with all fields",
				Email = "fully.updated@example.com",
				ExtensionNumber = "1006",
				Id = "custom006",
				PhoneNumbers = new List<string> { "+1111111111", "+2222222222" },
				RoutingPath = "complete_path",
				AutoCallRecorded = true
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "external_contacts", externalContact.ExternalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_ChangingAutoCallRecorded()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = "extContact4",
				Name = "Contact with changed recording setting",
				AutoCallRecorded = true
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "external_contacts", externalContact.ExternalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetAllAsync_WithMaxRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 100;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EXTERNAL_CONTACTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetDetailsAsync_ContactWithNoPhoneNumbers()
		{
			// Arrange
			var externalContactId = "noPhones";
			var noPhoneJson = @"{
				""external_contact_id"": ""noPhones"",
				""name"": ""Contact Without Phones"",
				""phone_numbers"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts", externalContactId))
				.Respond("application/json", noPhoneJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetDetailsAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.Count.ShouldBe(0);
		}

		[Fact]
		public async Task GetAllAsync_SingleContact()
		{
			// Arrange
			var singleContactJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""external_contacts"": [
					{
						""external_contact_id"": ""single"",
						""name"": ""Single Contact""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "external_contacts"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", singleContactJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].ExternalContactId.ShouldBe("single");
			result.Records[0].Name.ShouldBe("Single Contact");
		}

		[Fact]
		public async Task AddAsync_ContactWithMultiplePhoneNumbers()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				Name = "Multi Phone Contact",
				PhoneNumbers = new List<string>
				{
					"+1111111111",
					"+2222222222",
					"+3333333333",
					"+4444444444"
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("phone", "external_contacts"))
				.Respond("application/json", CREATED_EXTERNAL_CONTACT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateAsync_ClearingPhoneNumbers()
		{
			// Arrange
			var externalContact = new ExternalContactDetails
			{
				ExternalContactId = "extContact5",
				Name = "Contact with no phones after update",
				PhoneNumbers = new List<string>()
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("phone", "external_contacts", externalContact.ExternalContactId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			await externalContacts.UpdateAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
