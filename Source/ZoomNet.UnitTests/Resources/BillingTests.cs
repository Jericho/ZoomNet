using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class BillingTests
	{
		private const string BILLING_INFO_JSON = @"{
			""first_name"": ""John"",
			""last_name"": ""Doe"",
			""email"": ""billing@example.com"",
			""phone_number"": ""+1-555-0123"",
			""address"": ""123 Main Street"",
			""apt"": ""Suite 100"",
			""city"": ""New York"",
			""state"": ""NY"",
			""zip"": ""10001"",
			""country"": ""US""
		}";

		private const string INVOICES_JSON = @"{
			""currency"": ""USD"",
			""invoices"": [
				{
					""id"": ""INV-001"",
					""invoice_number"": ""2024-001"",
					""invoice_date"": ""2024-01-15"",
					""target_date"": ""2024-01-15"",
					""due_date"": ""2024-02-15"",
					""total_amount"": 150.00,
					""tax_amount"": 12.50,
					""balance"": 0.00,
					""status"": ""paid""
				},
				{
					""id"": ""INV-002"",
					""invoice_number"": ""2024-002"",
					""invoice_date"": ""2024-02-15"",
					""target_date"": ""2024-02-15"",
					""due_date"": ""2024-03-15"",
					""total_amount"": 175.00,
					""tax_amount"": 14.58,
					""balance"": 175.00,
					""status"": ""pending""
				}
			]
		}";

		private const string INVOICE_DETAILS_JSON = @"{
			""id"": ""INV-001"",
			""invoice_number"": ""2024-001"",
			""invoice_date"": ""2024-01-15"",
			""target_date"": ""2024-01-15"",
			""due_date"": ""2024-02-15"",
			""total_amount"": 150.00,
			""tax_amount"": 12.50,
			""balance"": 0.00,
			""status"": ""paid"",
			""currency"": ""USD"",
			""invoice_items"": [
				{
					""charge_name"": ""Pro Plan Subscription"",
					""charge_number"": ""CHG-001"",
					""charge_type"": ""subscription"",
					""quantity"": 10,
					""start_date"": ""2024-01-01"",
					""end_date"": ""2024-01-31"",
					""total_amount"": 150.00,
					""tax_amount"": 12.50,
					""partner_sku"": ""SKU-PRO"",
					""purchase_order_number"": ""PO-2024-001""
				}
			]
		}";

		private const string PLAN_USAGE_JSON = @"{
			""plan_base"": {
			""hosts"": 88,
			""type"": ""yearly"",
			""usage"": 28,
			""active_hosts"": 44,
			""pending"": 1
			},
			""plan_large_meeting"": [
			{
				""hosts"": 88,
				""type"": ""large500_monthly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_recording"": {
			""free_storage"": ""2 GB"",
			""free_storage_usage"": ""2 GB"",
			""plan_storage"": ""10 GB"",
			""plan_storage_exceed"": ""0"",
			""max_exceed_date"": ""2023-05-04"",
			""plan_storage_usage"": ""1 GB"",
			""type"": ""cmr_monthly_commitment_40""
			},
			""plan_united"": {
			""hosts"": 88,
			""name"": ""Zoom Meetings Pro and Zoom Phone Pro Monthly"",
			""type"": ""pro_zpp_monthly"",
			""usage"": 28,
			""pending"": 1
			},
			""plan_webinar"": [
			{
				""hosts"": 88,
				""type"": ""webinar500_monthly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_events"": [
			{
				""hosts"": 88,
				""type"": ""zoomevents500_monthly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_one"": [
			{
				""hosts"": 88,
				""type"": ""zobp_usca_monthly_unlimited"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_rooms"": {
			""hosts"": 88,
			""type"": ""zroom_monthly"",
			""usage"": 28
			},
			""plan_room_connector"": [
			{
				""hosts"": 88,
				""type"": ""roomconnector_monthly"",
				""usage"": 28
			}
			],
			""plan_whiteboard"": {
			""type"": ""zwb_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_whiteboard_plus"": {
			""type"": ""zwb_plus_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zoom_iq"": {
			""type"": ""ziq_sales_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_ztransl"": {
			""type"": ""ztransl_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zwr"": [
			{
				""hosts"": 88,
				""type"": ""zwr_yearly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zva"": [
			{
				""hosts"": 88,
				""type"": ""zva_monthly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_znode_base"": {
			""type"": ""znode_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zphybrid"": {
			""type"": ""zphybrid_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zsched"": {
			""type"": ""zsched_yearly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zmhybrid"": {
			""type"": ""zm_hybrid_quarterly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_mchybrid"": {
			""type"": ""zmc_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_rchybrid"": {
			""type"": ""zrc_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_cmrhybrid"": {
			""type"": ""zm_node_hybrid_rec_quarterly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_tchybrid"": {
			""type"": ""zn_tc_hybrid_monthly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_workforce_management"": {
			""type"": ""zm_wf_mgt_quarterly"",
			""hosts"": 88,
			""usage"": 28,
			""pending"": 1
			},
			""plan_zoom_one_premier"": [
			{
				""hosts"": 88,
				""type"": ""zoe_prem_jp_yearly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_one_edu_school_campus_plus"": [
			{
				""hosts"": 88,
				""type"": ""zo_edu_sch_cmp_plus_jp_yearly"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_one_edu_premier"": [
			{
				""hosts"": 88,
				""type"": ""plan_zoom_one_edu_premier"",
				""usage"": 28,
				""pending"": 1
			}
			],
			""plan_zoom_one_edu_student"": {
			""hosts"": 88,
			""type"": ""zo_edu_ent_hied_stu_yearly"",
			""usage"": 28,
			""pending"": 1
			},
			""plan_visitor_management"": {
			""hosts"": 88,
			""type"": ""zm_visitor_management_monthly"",
			""usage"": 28,
			""pending"": 1
			},
			""plan_partner_premier_support"": {
			""hosts"": 88,
			""type"": ""partner_premier_support_monthly"",
			""usage"": 28,
			""pending"": 1
			},
			""plan_zoom_clips_plus"": {
			""hosts"": 88,
			""type"": ""zm_clips_plus_monthly"",
			""usage"": 28,
			""pending"": 1
			}
		}";

		private readonly ITestOutputHelper _outputHelper;

		public BillingTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		[Fact]
		public async Task GetInfoAsync()
		{
			// Arrange
			var accountId = "test_account_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond("application/json", BILLING_INFO_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInfoAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.FirstName.ShouldBe("John");
			result.LastName.ShouldBe("Doe");
			result.Email.ShouldBe("billing@example.com");
			result.PhoneNumber.ShouldBe("+1-555-0123");
			result.Address.ShouldBe("123 Main Street");
			result.Apartment.ShouldBe("Suite 100");
			result.City.ShouldBe("New York");
			result.State.ShouldBe("NY");
			result.PostalCode.ShouldBe("10001");
			result.Country.ShouldBe("US");
		}

		[Fact]
		public async Task UpdateInfoAsync_WithAllParameters()
		{
			// Arrange
			var accountId = "test_account_123";
			var address = "456 Oak Avenue";
			var suite = "Apt 5B";
			var city = "Los Angeles";
			var country = "US";
			var email = "newemail@example.com";
			var firstName = "Jane";
			var lastName = "Smith";
			var phoneNumber = "+1-555-9876";
			var state = "CA";
			var zip = "90001";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			await billing.UpdateInfoAsync(accountId, address, suite, city, country, email, firstName, lastName, phoneNumber, state, zip, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateInfoAsync_WithMinimalParameters()
		{
			// Arrange
			var accountId = "test_account_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			await billing.UpdateInfoAsync(accountId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateInfoAsync_WithPartialParameters()
		{
			// Arrange
			var accountId = "test_account_123";
			var email = "updated@example.com";
			var phoneNumber = "+1-555-1111";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			await billing.UpdateInfoAsync(accountId, email: email, phoneNumber: phoneNumber, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetInvoicesAsync()
		{
			// Arrange
			var accountId = "test_account_123";
			var from = new DateTime(2024, 1, 1);
			var to = new DateTime(2024, 3, 31);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices"))
				.WithQueryString("from", "2024-01-01")
				.WithQueryString("to", "2024-03-31")
				.Respond("application/json", INVOICES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoicesAsync(accountId, from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);

			// Verify first invoice
			result[0].Id.ShouldBe("INV-001");
			result[0].InvoiceNumber.ShouldBe("2024-001");
			result[0].TotalAmount.ShouldBe(150.00m);
			result[0].TaxAmount.ShouldBe(12.50m);
			result[0].Balance.ShouldBe(0.00m);
			result[0].Status.ShouldBe("paid");
			result[0].Currency.ShouldBe("USD"); // Currency should be populated from the parent object

			// Verify second invoice
			result[1].Id.ShouldBe("INV-002");
			result[1].InvoiceNumber.ShouldBe("2024-002");
			result[1].TotalAmount.ShouldBe(175.00m);
			result[1].TaxAmount.ShouldBe(14.58m);
			result[1].Balance.ShouldBe(175.00m);
			result[1].Status.ShouldBe("pending");
			result[1].Currency.ShouldBe("USD");
		}

		[Fact]
		public async Task GetInvoicesAsync_EmptyResult()
		{
			// Arrange
			var accountId = "test_account_123";
			var from = new DateTime(2024, 1, 1);
			var to = new DateTime(2024, 3, 31);
			var emptyInvoicesJson = @"{
				""currency"": ""USD"",
				""invoices"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices"))
				.WithQueryString("from", "2024-01-01")
				.WithQueryString("to", "2024-03-31")
				.Respond("application/json", emptyInvoicesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoicesAsync(accountId, from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetInvoiceDetailsAsync()
		{
			// Arrange
			var accountId = "test_account_123";
			var invoiceId = "INV-001";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices", invoiceId))
				.Respond("application/json", INVOICE_DETAILS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoiceDetailsAsync(accountId, invoiceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("INV-001");
			result.InvoiceNumber.ShouldBe("2024-001");
			result.TotalAmount.ShouldBe(150.00m);
			result.TaxAmount.ShouldBe(12.50m);
			result.Balance.ShouldBe(0.00m);
			result.Status.ShouldBe("paid");
			result.Currency.ShouldBe("USD");
		}

		[Fact]
		public async Task GetPlanUsageAsync()
		{
			// Arrange
			var accountId = "test_account_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "plans/usage"))
				.Respond("application/json", PLAN_USAGE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetPlanUsageAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DownloadInvoiceAsync()
		{
			// Arrange
			var invoiceId = "INV-001";
			var pdfContent = Encoding.UTF8.GetBytes("%PDF-1.4 fake PDF content");

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("billing/invoices", invoiceId, "download"))
				.Respond("application/pdf", new MemoryStream(pdfContent));

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.DownloadInvoiceAsync(invoiceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();

			// Read the stream to verify content
			using (var memoryStream = new MemoryStream())
			{
				await result.CopyToAsync(memoryStream, 81920, cancellationToken: TestContext.Current.CancellationToken);
				var downloadedContent = memoryStream.ToArray();
				downloadedContent.ShouldBe(pdfContent);
			}
		}

		[Fact]
		public async Task DownloadInvoiceAsync_EmptyStream()
		{
			// Arrange
			var invoiceId = "INV-002";
			var emptyStream = new MemoryStream();

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("billing/invoices", invoiceId, "download"))
				.Respond("application/pdf", emptyStream);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.DownloadInvoiceAsync(invoiceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();

			// Verify the stream is empty
			using (var memoryStream = new MemoryStream())
			{
				await result.CopyToAsync(memoryStream, 81920, TestContext.Current.CancellationToken);
				memoryStream.Length.ShouldBe(0);
			}
		}

		[Fact]
		public async Task GetInvoicesAsync_DateRangeFormatting()
		{
			// Arrange
			var accountId = "test_account_123";
			var from = new DateTime(2024, 6, 15, 10, 30, 45); // With time component
			var to = new DateTime(2024, 9, 20, 18, 45, 30);   // With time component

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices"))
				.WithQueryString("from", "2024-06-15") // Should only include date
				.WithQueryString("to", "2024-09-20")   // Should only include date
				.Respond("application/json", INVOICES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoicesAsync(accountId, from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetInfoAsync_WithDifferentAccountId()
		{
			// Arrange
			var accountId = "different_account_456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond("application/json", BILLING_INFO_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInfoAsync(accountId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetInvoiceDetailsAsync_WithDifferentInvoiceId()
		{
			// Arrange
			var accountId = "test_account_123";
			var invoiceId = "INV-999";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices", invoiceId))
				.Respond("application/json", INVOICE_DETAILS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoiceDetailsAsync(accountId, invoiceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DownloadInvoiceAsync_LargeFile()
		{
			// Arrange
			var invoiceId = "INV-LARGE";
			var largeContent = new byte[1024 * 1024]; // 1 MB
			for (int i = 0; i < largeContent.Length; i++)
			{
				largeContent[i] = (byte)(i % 256);
			}

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("billing/invoices", invoiceId, "download"))
				.Respond("application/pdf", new MemoryStream(largeContent));

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.DownloadInvoiceAsync(invoiceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();

			using (var memoryStream = new MemoryStream())
			{
				await result.CopyToAsync(memoryStream, 81920, TestContext.Current.CancellationToken);
				memoryStream.Length.ShouldBe(largeContent.Length);
			}
		}

		[Fact]
		public async Task GetInvoicesAsync_WithSingleInvoice()
		{
			// Arrange
			var accountId = "test_account_123";
			var from = new DateTime(2024, 1, 1);
			var to = new DateTime(2024, 1, 31);
			var singleInvoiceJson = @"{
				""currency"": ""EUR"",
				""invoices"": [
					{
						""id"": ""INV-SINGLE"",
						""invoice_number"": ""2024-SINGLE"",
						""invoice_date"": ""2024-01-15"",
						""target_date"": ""2024-01-15"",
						""due_date"": ""2024-02-15"",
						""total_amount"": 99.99,
						""tax_amount"": 19.99,
						""balance"": 0.00,
						""status"": ""paid""
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("accounts", accountId, "billing/invoices"))
				.WithQueryString("from", "2024-01-01")
				.WithQueryString("to", "2024-01-31")
				.Respond("application/json", singleInvoiceJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			var result = await billing.GetInvoicesAsync(accountId, from, to, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("INV-SINGLE");
			result[0].Currency.ShouldBe("EUR");
		}

		[Fact]
		public async Task UpdateInfoAsync_OnlyAddress()
		{
			// Arrange
			var accountId = "test_account_123";
			var address = "789 Pine Road";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			await billing.UpdateInfoAsync(accountId, address: address, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateInfoAsync_OnlyNameFields()
		{
			// Arrange
			var accountId = "test_account_123";
			var firstName = "Alice";
			var lastName = "Johnson";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("accounts", accountId, "billing"))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var billing = new Billing(client);

			// Act
			await billing.UpdateInfoAsync(accountId, firstName: firstName, lastName: lastName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}
	}
}
