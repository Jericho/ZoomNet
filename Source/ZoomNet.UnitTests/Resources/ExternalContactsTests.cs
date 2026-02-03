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
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class ExternalContactsTests
	{
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
				.Respond("application/json", EndpointsResource.phone_external_contacts_GET);

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
			result.NextPageToken.ShouldBe("BJLYC6PABbAHdjwSkGVQeeR6B1juwHqj3G2");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].ExternalContactId.ShouldBe("OJGi5xOFQPmrJbKg68-iWg");
			result.Records[0].Name.ShouldBe("Johnson");
			result.Records[0].Description.ShouldBe("External contact Johnson");
			result.Records[0].Email.ShouldBe("example@example.com");
			result.Records[0].ExtensionNumber.ShouldBe("101014");
			result.Records[0].PhoneNumbers.ShouldNotBeNull();
			result.Records[0].PhoneNumbers.Count.ShouldBe(1);
			result.Records[0].AutoCallRecorded.ShouldBeTrue();
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
				.Respond("application/json", EndpointsResource.phone_external_contacts__externalContactId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.GetDetailsAsync(externalContactId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ExternalContactId.ShouldBe("OJGi5xOFQPmrJbKg68-iWg");
			result.Name.ShouldBe("Johnson");
			result.Description.ShouldBe("External contact Johnson");
			result.Email.ShouldBe("example@example.com");
			result.ExtensionNumber.ShouldBe("101014");
			result.Id.ShouldBe("external_contact_01");
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.Count.ShouldBe(1);
			result.RoutingPath.ShouldBeNull();
			result.AutoCallRecorded.ShouldBeTrue();
			result.ProfilePictureUrl.ShouldBe("https://file.zoom.us/public/file/NgY6XhB0Q1KUOIpbnjQrTA/MS41Lm3jVUCBCmBzQxZEXUvHcJ_YRtZ-CVuQ68tE7kKC60t_/twP0LQrnQfS7bQ6utANohw.png");
			result.TreatAsInternalExtension.ShouldBeTrue();
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
				.Respond("application/json", EndpointsResource.phone_external_contacts_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var externalContacts = new ExternalContacts(client);

			// Act
			var result = await externalContacts.AddAsync(externalContact, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ExternalContactId.ShouldBe("nqerMCD0Tu6RPGoCpVbPtA");
			result.Name.ShouldBe("Johnson");
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
				.Respond("application/json", EndpointsResource.phone_external_contacts_POST);

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
				.Respond("application/json", EndpointsResource.phone_external_contacts_POST);

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
				.Respond("application/json", EndpointsResource.phone_external_contacts_POST);

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
				.Respond("application/json", EndpointsResource.phone_external_contacts_POST);

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
