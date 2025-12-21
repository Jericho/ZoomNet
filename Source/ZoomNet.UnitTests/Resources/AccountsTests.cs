using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class AccountsTests
	{
		private const string SINGLE_ACCOUNT_JSON = @"{
			""id"": 123456789,
			""account_name"": ""Test Account"",
			""owner_email"": ""owner@example.com"",
			""account_type"": ""Pro"",
			""seats"": 10,
			""subscription_start_time"": ""2021-01-01T00:00:00Z"",
			""subscription_end_time"": ""2021-12-31T23:59:59Z"",
			""created_at"": ""2020-01-01T00:00:00Z""
		}";

		private const string MULTIPLE_ACCOUNTS_JSON = @"{
			""page_count"": 1,
			""page_number"": 1,
			""page_size"": 30,
			""total_records"": 2,
			""accounts"": [
				{
					""id"": 111111111,
					""account_name"": ""Test Account 1"",
					""owner_email"": ""owner1@example.com"",
					""account_type"": ""Pro"",
					""seats"": 10,
					""created_at"": ""2020-01-01T00:00:00Z""
				},
				{
					""id"": 222222222,
					""account_name"": ""Test Account 2"",
					""owner_email"": ""owner2@example.com"",
					""account_type"": ""Business"",
					""seats"": 20,
					""created_at"": ""2020-06-01T00:00:00Z""
				}
			]
		}";

		private const string MULTIPLE_ACCOUNTS_WITH_TOKEN_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""next_token_123"",
			""accounts"": [
				{
					""id"": 111111111,
					""account_name"": ""Test Account 1"",
					""owner_email"": ""owner1@example.com"",
					""account_type"": ""Pro"",
					""seats"": 10,
					""created_at"": ""2020-01-01T00:00:00Z""
				},
				{
					""id"": 222222222,
					""account_name"": ""Test Account 2"",
					""owner_email"": ""owner2@example.com"",
					""account_type"": ""Business"",
					""seats"": 20,
					""created_at"": ""2020-06-01T00:00:00Z""
				}
			]
		}";

		private const string AUTHENTICATION_SETTINGS_JSON = @"{
			""meeting_authentication"": true,
			""authentication_options"": [
				{
					""id"": ""auth_option_1"",
					""name"": ""Auth Option 1"",
					""type"": ""internally"",
					""default_option"": true,
					""domains"": ""example.com""
				},
				{
					""id"": ""auth_option_2"",
					""name"": ""Auth Option 2"",
					""type"": ""enforce_login"",
					""default_option"": false,
					""domains"": ""test.com""
				}
			]
		}";

		private const string MANAGED_DOMAINS_JSON = @"{
			""total_records"": 2,
			""domains"": [
				{
					""domain"": ""example.com"",
					""status"": ""activated""
				},
				{
					""domain"": ""test.com"",
					""status"": ""pending""
				}
			]
		}";

		private const string TRUSTED_DOMAINS_JSON = @"{
			""trusted_domains"": [
				""trusted1.com"",
				""trusted2.com"",
				""trusted3.com""
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public AccountsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public async Task GetAllAsync_WithPageNumber()
		{
			// Arrange
			var recordsPerPage = 30;
			var page = 1;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("page_number", page.ToString())
				.Respond("application/json", MULTIPLE_ACCOUNTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
#pragma warning disable CS0618 // Type or member is obsolete
			var result = await accounts.GetAllAsync(recordsPerPage, page, TestContext.Current.CancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(recordsPerPage);
			result.PageNumber.ShouldBe(page);
			result.TotalRecords.ShouldBe(2);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe(111111111);
			result.Records[1].Id.ShouldBe(222222222);
		}

		[Fact]
		public async Task GetAllAsync_WithPagingToken()
		{
			// Arrange
			var recordsPerPage = 30;
			var pagingToken = "next_token_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", MULTIPLE_ACCOUNTS_WITH_TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetAllAsync(recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(recordsPerPage);
			result.NextPageToken.ShouldBe("next_token_123");
			result.MoreRecordsAvailable.ShouldBeTrue();
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllAsync_WithPagingToken_NullToken()
		{
			// Arrange
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", MULTIPLE_ACCOUNTS_WITH_TOKEN_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetAllAsync(recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task CreateAsync()
		{
			// Arrange
			var firstName = "John";
			var lastName = "Doe";
			var email = "john.doe@example.com";
			var password = "Password123!";
			var useSharedVirtualRoomConnectors = true;
			var roomConnectorsIpAddresses = new[] { "192.168.1.1", "192.168.1.2" };
			var useSharedMeetingConnectors = true;
			var meetingConnectorsIpAddresses = new[] { "192.168.2.1", "192.168.2.2" };
			var payMode = PayMode.Master;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("accounts"))
				.Respond("application/json", SINGLE_ACCOUNT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.CreateAsync(firstName, lastName, email, password, useSharedVirtualRoomConnectors, roomConnectorsIpAddresses, useSharedMeetingConnectors, meetingConnectorsIpAddresses, payMode, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(123456789);
		}

		[Fact]
		public async Task CreateAsync_WithMinimalParameters()
		{
			// Arrange
			var firstName = "Jane";
			var lastName = "Smith";
			var email = "jane.smith@example.com";
			var password = "SecurePass123!";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("accounts"))
				.Respond("application/json", SINGLE_ACCOUNT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.CreateAsync(firstName, lastName, email, password, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId))
				.Respond("application/json", SINGLE_ACCOUNT_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(123456789);
		}

		[Fact]
		public async Task DisassociateAsync()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("accounts", accountId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			await accounts.DisassociateAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateOptionsAsync()
		{
			// Arrange
			var accountId = 123456789L;
			var useSharedVirtualRoomConnectors = true;
			var roomConnectorsIpAddresses = new[] { "192.168.3.1" };
			var useSharedMeetingConnectors = false;
			var meetingConnectorsIpAddresses = new[] { "192.168.4.1" };
			var payMode = PayMode.SubAccount;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "options"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			await accounts.UpdateOptionsAsync(accountId, useSharedVirtualRoomConnectors, roomConnectorsIpAddresses, useSharedMeetingConnectors, meetingConnectorsIpAddresses, payMode, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateOptionsAsync_WithNullParameters()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "options"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			await accounts.UpdateOptionsAsync(accountId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetMeetingAuthenticationSettingsAsync()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "settings"))
				.WithQueryString("option", "meeting_authentication")
				.Respond("application/json", AUTHENTICATION_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetMeetingAuthenticationSettingsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequireAuthentication.ShouldBeTrue();
			result.AuthenticationOptions.ShouldNotBeNull();
			result.AuthenticationOptions.Length.ShouldBe(2);
			result.AuthenticationOptions[0].Domains.ShouldBe("example.com");
			result.AuthenticationOptions[0].Id.ShouldBe("auth_option_1");
			result.AuthenticationOptions[0].IsDefault.ShouldBeTrue();
			result.AuthenticationOptions[0].Type.ShouldBe(AuthenticationType.Internal);
			result.AuthenticationOptions[0].Visible.ShouldBeFalse();
			result.AuthenticationOptions[0].Name.ShouldBe("Auth Option 1");
			result.AuthenticationOptions[1].Domains.ShouldBe("test.com");
			result.AuthenticationOptions[1].Id.ShouldBe("auth_option_2");
			result.AuthenticationOptions[1].IsDefault.ShouldBeFalse();
			result.AuthenticationOptions[1].Type.ShouldBe(AuthenticationType.EnforceLogin);
			result.AuthenticationOptions[1].Visible.ShouldBeFalse();
			result.AuthenticationOptions[1].Name.ShouldBe("Auth Option 2");
		}

		[Fact]
		public async Task GetMeetingAuthenticationSettingsAsync_WhenNoAuthenticationRequired()
		{
			// Arrange
			var accountId = 123456789L;
			var noAuthJson = @"{
				""meeting_authentication"": false,
				""authentication_options"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "settings"))
				.WithQueryString("option", "meeting_authentication")
				.Respond("application/json", noAuthJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetMeetingAuthenticationSettingsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequireAuthentication.ShouldBeFalse();
			result.AuthenticationOptions.ShouldNotBeNull();
			result.AuthenticationOptions.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetRecordingAuthenticationSettingsAsync()
		{
			// Arrange
			var accountId = 123456789L;
			var recordingAuthJson = @"{
				""recording_authentication"": true,
				""authentication_options"": [
					{
						""id"": ""rec_auth_1"",
						""name"": ""Recording Auth 1"",
						""type"": ""internally"",
						""default_option"": true,
						""domains"": ""company.com""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "settings"))
				.WithQueryString("option", "recording_authentication")
				.Respond("application/json", recordingAuthJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetRecordingAuthenticationSettingsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequireAuthentication.ShouldBeTrue();
			result.AuthenticationOptions.ShouldNotBeNull();
			result.AuthenticationOptions.Length.ShouldBe(1);
			result.AuthenticationOptions[0].Id.ShouldBe("rec_auth_1");
			result.AuthenticationOptions[0].Name.ShouldBe("Recording Auth 1");
		}

		[Fact]
		public async Task GetRecordingAuthenticationSettingsAsync_WhenNoAuthenticationRequired()
		{
			// Arrange
			var accountId = 123456789L;
			var noAuthJson = @"{
				""recording_authentication"": false,
				""authentication_options"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "settings"))
				.WithQueryString("option", "recording_authentication")
				.Respond("application/json", noAuthJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetRecordingAuthenticationSettingsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RequireAuthentication.ShouldBeFalse();
			result.AuthenticationOptions.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetManagedDomainsAsync()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "managed_domains"))
				.Respond("application/json", MANAGED_DOMAINS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetManagedDomainsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Domain.ShouldBe("example.com");
			result[0].Status.ShouldBe("activated");
			result[1].Domain.ShouldBe("test.com");
			result[1].Status.ShouldBe("pending");
		}

		[Fact]
		public async Task GetManagedDomainsAsync_EmptyList()
		{
			// Arrange
			var accountId = 123456789L;
			var emptyDomainsJson = @"{
				""total_records"": 0,
				""domains"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "managed_domains"))
				.Respond("application/json", emptyDomainsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetManagedDomainsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetTrustedDomainsAsync()
		{
			// Arrange
			var accountId = 123456789L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "trusted_domains"))
				.Respond("application/json", TRUSTED_DOMAINS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetTrustedDomainsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
			result[0].ShouldBe("trusted1.com");
			result[1].ShouldBe("trusted2.com");
			result[2].ShouldBe("trusted3.com");
		}

		[Fact]
		public async Task GetTrustedDomainsAsync_EmptyList()
		{
			// Arrange
			var accountId = 123456789L;
			var emptyTrustedDomainsJson = @"{
				""trusted_domains"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "trusted_domains"))
				.Respond("application/json", emptyTrustedDomainsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			var result = await accounts.GetTrustedDomainsAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task UpdateOwnerAsync()
		{
			// Arrange
			var accountId = 123456789L;
			var newOwnerEmail = "newowner@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "owner"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act
			await accounts.UpdateOwnerAsync(accountId, newOwnerEmail, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act & Assert
#pragma warning disable CS0618 // Type or member is obsolete
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => accounts.GetAllAsync(recordsPerPage: 500, page: 1, cancellationToken: TestContext.Current.CancellationToken));
#pragma warning restore CS0618 // Type or member is obsolete
		}

		[Fact]
		public async Task GetAllAsync_WithPagingToken_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var accounts = new Accounts(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => accounts.GetAllAsync(recordsPerPage: 500, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken));
		}
	}
}
