using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class WebinarsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public WebinarsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync_WithPaginationToken_ReturnsWebinars()
		{
			// Arrange
			var userId = "user123";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "webinars"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.users__userId__webinars_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetAllAsync(userId, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe(1234567890);
			result.Records[0].Topic.ShouldBe("Marketing Strategies Webinar");
			result.Records[1].Id.ShouldBe(9876543210);
		}

		#endregion

		#region Create Webinar Tests

		[Fact]
		public async Task CreateScheduledWebinarAsync_WithValidParameters_ReturnsWebinar()
		{
			// Arrange
			var userId = "user123";
			var topic = "Product Launch Webinar";
			var agenda = "Launching our new product line";
			var start = new DateTime(2023, 8, 1, 15, 0, 0);
			var duration = 45;
			var timeZone = TimeZones.America_Los_Angeles;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "webinars"))
				.Respond("application/json", EndpointsResource.users__userId__webinars_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateScheduledWebinarAsync(userId, topic, agenda, start, duration, timeZone, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(2222222222);
			result.Topic.ShouldBe("Product Launch Webinar");
			result.Duration.ShouldBe(45);
		}

		[Fact]
		public async Task CreateRecurringWebinarAsync_WithRecurrence_ReturnsWebinar()
		{
			// Arrange
			var userId = "user123";
			var topic = "Weekly Training Series";
			var agenda = "Weekly training sessions";
			var start = new DateTime(2023, 7, 1, 9, 0, 0);
			var duration = 30;
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				WeeklyDays = new[] { DayOfWeek.Tuesday }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "webinars"))
				.Respond("application/json", EndpointsResource.users__userId__webinars_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateRecurringWebinarAsync(userId, topic, agenda, start, duration, recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(3333333333);
			result.Topic.ShouldBe("Weekly Training Series");
		}

		#endregion

		#region GetAsync Test

		[Fact]
		public async Task GetAsync_WithWebinarId_ReturnsWebinar()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("show_previous_occurrences", "false")
				.Respond("application/json", EndpointsResource.webinars__webinarId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe(2222222222);
			result.Topic.ShouldBe("Product Launch Webinar");
		}

		#endregion

		#region Update Webinar Tests

		[Fact]
		public async Task UpdateScheduledWebinarAsync_WithValidParameters_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var topic = "Updated Webinar Topic";
			var agenda = "Updated agenda";
			var duration = 60;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateScheduledWebinarAsync(webinarId, topic, agenda, duration: duration, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRecurringWebinarAsync_WithRecurrence_Succeeds()
		{
			// Arrange
			var webinarId = 3333333333L;
			var topic = "Updated Recurring Webinar";
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateRecurringWebinarAsync(webinarId, topic, recurrence: recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateWebinarOccurrenceAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 3333333333L;
			var occurrenceId = "occurrence123";
			var agenda = "Updated occurrence agenda";
			var start = new DateTime(2023, 7, 15, 10, 0, 0);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("occurrence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateWebinarOccurrenceAsync(webinarId, occurrenceId, agenda, start, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Delete and Status Tests

		[Fact]
		public async Task DeleteAsync_WithWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("cancel_webinar_reminder", "false")
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteAsync(webinarId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task EndAsync_WithWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.EndAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Panelist Tests

		[Fact]
		public async Task GetPanelistsAsync_WithWebinarId_ReturnsPanelists()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__panelists_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetPanelistsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("panelist1");
			result[0].Email.ShouldBe("john@example.com");
			result[1].Id.ShouldBe("panelist2");
			result[1].Email.ShouldBe("jane@example.com");
		}

		[Fact]
		public async Task AddPanelistAsync_WithEmailAndName_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var email = "newpanelist@example.com";
			var fullName = "New Panelist";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists"))
				.Respond(System.Net.HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.AddPanelistAsync(webinarId, email, fullName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemovePanelistAsync_WithPanelistId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var panelistId = "panelist1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists", panelistId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.RemovePanelistAsync(webinarId, panelistId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveAllPanelistsAsync_WithWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.RemoveAllPanelistsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Registrant Tests

		[Fact]
		public async Task GetRegistrantsAsync_WithStatus_ReturnsRegistrants()
		{
			// Arrange
			var webinarId = 1234567890L;
			var status = RegistrantStatus.Approved;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants"))
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrantsAsync(webinarId, status, recordsPerPage: 30, pagingToken: null, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("reg001");
			result.Records[0].Email.ShouldBe("attendee1@example.com");
		}

		[Fact]
		public async Task GetRegistrantAsync_WithRegistrantId_ReturnsRegistrant()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantId = "reg123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", registrantId))
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants__registrantId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrantAsync(webinarId, registrantId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("reg123");
			result.Email.ShouldBe("registrant@example.com");
		}

		[Fact]
		public async Task AddRegistrantAsync_WithRequiredFields_ReturnsRegistrantInfo()
		{
			// Arrange
			var webinarId = 1234567890L;
			var email = "newattendee@example.com";
			var firstName = "New";
			var lastName = "Attendee";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.AddRegistrantAsync(webinarId, email, firstName, lastName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("new_reg_123");
			result.JoinUrl.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public async Task PerformBatchRegistrationAsync_WithMultipleRegistrants_ReturnsRegistrantInfos()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrants = new[]
			{
				new BatchRegistrant { Email = "batch1@example.com", FirstName = "Batch", LastName = "One" },
				new BatchRegistrant { Email = "batch2@example.com", FirstName = "Batch", LastName = "Two" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "batch_registrants"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__batch_registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.PerformBatchRegistrationAsync(webinarId, registrants, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
		}

		#endregion

		#region Exception and Validation Tests

		[Fact]
		public async Task PerformBatchRegistrationAsync_WithEmptyRegistrants_ThrowsException()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrants = Array.Empty<BatchRegistrant>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => webinars.PerformBatchRegistrationAsync(webinarId, registrants, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task PerformBatchRegistrationAsync_WithTooManyRegistrants_ThrowsException()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrants = Enumerable.Range(1, 31)
				.Select(i => new BatchRegistrant { Email = $"user{i}@example.com", FirstName = "User", LastName = i.ToString() })
				.ToArray();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => webinars.PerformBatchRegistrationAsync(webinarId, registrants, cancellationToken: TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task PerformBatchRegistrationAsync_WithAutoApprove_ReturnsRegistrantInfos()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrants = new[]
			{
				new BatchRegistrant { Email = "auto1@example.com", FirstName = "Auto", LastName = "One" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "batch_registrants"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__batch_registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.PerformBatchRegistrationAsync(webinarId, registrants, autoApprove: true, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateInviteLinksAsync_WithEmptyNames_ThrowsException()
		{
			// Arrange
			var webinarId = 1234567890L;
			var names = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => webinars.CreateInviteLinksAsync(webinarId, names, cancellationToken: TestContext.Current.CancellationToken));
		}

		#endregion

		#region Additional Parameter Variation Tests

		[Fact]
		public async Task CreateScheduledWebinarAsync_WithAllParameters_ReturnsWebinar()
		{
			// Arrange
			var userId = "user123";
			var topic = "Comprehensive Webinar";
			var agenda = "Detailed agenda";
			var start = new DateTime(2023, 8, 1, 15, 0, 0);
			var duration = 45;
			var timeZone = TimeZones.America_Los_Angeles;
			var password = "secure123";
			var settings = new WebinarSettings { StartVideoWhenHostJoins = true, StartVideoWhenPanelistsJoin = true };
			var trackingFields = new Dictionary<string, string> { { "source", "marketing" } };
			var templateId = "template123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "webinars"))
				.Respond("application/json", EndpointsResource.users__userId__webinars_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateScheduledWebinarAsync(userId, topic, agenda, start, duration, timeZone, password, settings, trackingFields, templateId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateRecurringWebinarAsync_WithoutStartTime_ReturnsWebinar()
		{
			// Arrange
			var userId = "user123";
			var topic = "No Fixed Time Webinar";
			var agenda = "Flexible schedule";
			DateTime? start = null;
			var duration = 30;
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Daily,
				RepeatInterval = 1
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "webinars"))
				.Respond("application/json", EndpointsResource.users__userId__webinars_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateRecurringWebinarAsync(userId, topic, agenda, start, duration, recurrence, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateRecurringWebinarAsync_WithAllParameters_ReturnsWebinar()
		{
			// Arrange
			var userId = "user123";
			var topic = "Complete Recurring Webinar";
			var agenda = "Full details";
			var start = new DateTime(2023, 7, 1, 9, 0, 0);
			var duration = 30;
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Weekly,
				RepeatInterval = 1,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday }
			};
			var password = "recurring123";
			var settings = new WebinarSettings { StartVideoWhenHostJoins = false };
			var trackingFields = new Dictionary<string, string> { { "department", "sales" } };
			var templateId = "recurTemplate";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("users", userId, "webinars"))
				.Respond("application/json", EndpointsResource.users__userId__webinars_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateRecurringWebinarAsync(userId, topic, agenda, start, duration, recurrence, TimeZones.UTC, password, settings, trackingFields, templateId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateScheduledWebinarAsync_WithTrackingFields_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var trackingFields = new Dictionary<string, string>
			{
				{ "campaign", "Q3-2023" },
				{ "region", "EMEA" }
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateScheduledWebinarAsync(webinarId, trackingFields: trackingFields, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRecurringWebinarAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var webinarId = 3333333333L;
			var topic = "Fully Updated Webinar";
			var agenda = "Complete update";
			var start = new DateTime(2023, 8, 1, 10, 0, 0);
			var duration = 45;
			var recurrence = new RecurrenceInfo
			{
				Type = RecurrenceType.Monthly,
				RepeatInterval = 1,
				MonthlyDay = 15
			};
			var password = "updated456";
			var settings = new WebinarSettings { StartVideoWhenHostJoins = true };
			var trackingFields = new Dictionary<string, string> { { "updated", "true" } };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateRecurringWebinarAsync(webinarId, topic, agenda, start, duration, TimeZones.America_New_York, recurrence, password, settings, trackingFields, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateWebinarOccurrenceAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var webinarId = 3333333333L;
			var occurrenceId = "occurrence123";
			var agenda = "Updated occurrence";
			var start = new DateTime(2023, 7, 20, 15, 0, 0);
			var duration = 60;
			var timeZone = TimeZones.Europe_London;
			var settings = new WebinarSettings { StartVideoWhenPanelistsJoin = false };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("occurrence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateWebinarOccurrenceAsync(webinarId, occurrenceId, agenda, start, duration, timeZone, settings, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddRegistrantAsync_WithAllParameters_ReturnsRegistrantInfo()
		{
			// Arrange
			var webinarId = 1234567890L;
			var email = "complete@example.com";
			var firstName = "Complete";
			var lastName = "User";
			var address = "123 Main St";
			var city = "New York";
			var country = Country.United_States_of_America;
			var postalCode = "10001";
			var stateOrProvince = "NY";
			var phoneNumber = "+1-555-0100";
			var industry = "Technology";
			var organization = "Tech Corp";
			var jobTitle = "Developer";
			var timeFrame = PurchasingTimeFrame.Within_a_month;
			var role = RoleInPurchaseProcess.Decision_Maker;
			var employees = NumberOfEmployees.Between_0001_and_0020;
			var comments = "Looking forward to this webinar";
			var questionAnswers = new[]
			{
				new RegistrationAnswer { Title = "Why attending?", Answer = "Professional development" }
			};
			var language = Language.English_US;
			var occurrenceId = "occurrence456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.AddRegistrantAsync(webinarId, email, firstName, lastName, address, city, country, postalCode, stateOrProvince, phoneNumber, industry, organization, jobTitle, timeFrame, role, employees, comments, questionAnswers, language, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("new_reg_123");
			result.JoinUrl.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public async Task DeleteRegistrantAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantId = "reg001";
			var occurrenceId = "occurrence789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", registrantId))
				.WithQueryString("occurence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteRegistrantAsync(webinarId, registrantId, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task ApproveRegistrantAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantId = "reg001";
			var registrantEmail = "attendee1@example.com";
			var occurrenceId = "occurrence456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.WithQueryString("occurence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.ApproveRegistrantAsync(webinarId, registrantId, registrantEmail, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantId = "reg002";
			var registrantEmail = "attendee2@example.com";
			var occurrenceId = "occurrence789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.WithQueryString("occurence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.RejectRegistrantAsync(webinarId, registrantId, registrantEmail, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelRegistrantAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantId = "reg003";
			var registrantEmail = "attendee3@example.com";
			var occurrenceId = "occurrence101";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.WithQueryString("occurence_id", occurrenceId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.CancelRegistrantAsync(webinarId, registrantId, registrantEmail, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetRegistrantsAsync_WithOccurrenceId_ReturnsRegistrants()
		{
			// Arrange
			var webinarId = 1234567890L;
			var status = RegistrantStatus.Approved;
			var occurrenceId = "occurrence202";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants"))
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("occurrence_id", occurrenceId)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrantsAsync(webinarId, status, null, occurrenceId, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateSurveyAsync_WithThirdPartySurveyLink_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var thirdPartySurveyLink = "https://survey.example.com/webinar-feedback";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateSurveyAsync(webinarId, null, true, true, thirdPartySurveyLink, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateSurveyAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var questions = new[]
			{
				new SurveyQuestion
				{
					Name = "Satisfaction level?",
					Type = SurveyQuestionType.Rating_Scale,
					IsRequired = true,
					RatingMinimumValue = 1,
					RatingMaximumValue = 10
				},
				new SurveyQuestion
				{
					Name = "Additional comments?",
					Type = SurveyQuestionType.Long,
					IsRequired = false,
					MinimumNumberOfCharacters = 10,
					MaximumNumberOfCharacters = 500
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateSurveyAsync(webinarId, questions, false, false, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateInviteLinksAsync_WithCustomTTL_ReturnsInviteLinks()
		{
			// Arrange
			var webinarId = 1234567890L;
			var names = new[] { "VIP Guest 1", "VIP Guest 2", "VIP Guest 3" };
			var timeToLive = 14400L; // 4 hours

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "invite_links"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__invite_links_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreateInviteLinksAsync(webinarId, names, timeToLive, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task StartLiveStreamAsync_WithoutDisplayName_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var displaySpeakerName = false;
			var speakerName = "Anonymous";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.StartLiveStreamAsync(webinarId, displaySpeakerName, speakerName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AddPanelistAsync_WithVirtualBackground_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var email = "panelist@example.com";
			var fullName = "Expert Panelist";
			var virtualBackgroundId = "vbg_custom_123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists"))
				.Respond(System.Net.HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.AddPanelistAsync(webinarId, email, fullName, virtualBackgroundId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsAsync_WithOnlyRequiredFields_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var requiredFields = new[] { RegistrationField.LastName, RegistrationField.Organization, RegistrationField.JobTitle };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "questions"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateRegistrationQuestionsAsync(webinarId, requiredFields, null, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsAsync_WithOnlyOptionalFields_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var optionalFields = new[] { RegistrationField.Phone, RegistrationField.Industry };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "questions"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateRegistrationQuestionsAsync(webinarId, null, optionalFields, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRegistrationQuestionsAsync_WithOnlyCustomQuestions_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForWebinar
				{
					Title = "How did you hear about us?",
					Type = RegistrationCustomQuestionTypeForWebinar.SingleRadio,
					IsRequired = true,
					Answers = new[] { "Social Media", "Email", "Website", "Friend" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "questions"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateRegistrationQuestionsAsync(webinarId, null, null, customQuestions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAllAsync_WithCustomRecordsPerPage_ReturnsWebinars()
		{
			// Arrange
			var userId = "user123";
			var recordsPerPage = 50;
			var pagingToken = "customToken789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "webinars"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.users__userId__webinars_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetAllAsync(userId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Registrant Status Update Tests

		[Fact]
		public async Task ApproveRegistrantsAsync_WithMultipleRegistrants_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantsInfo = new[]
			{
				("reg001", "attendee1@example.com"),
				("reg002", "attendee2@example.com"),
				("reg003", "attendee3@example.com")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.ApproveRegistrantsAsync(webinarId, registrantsInfo, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RejectRegistrantsAsync_WithMultipleRegistrants_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantsInfo = new[]
			{
				("reg004", "rejectee1@example.com"),
				("reg005", "rejectee2@example.com")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.RejectRegistrantsAsync(webinarId, registrantsInfo, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelRegistrantsAsync_WithMultipleRegistrants_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var registrantsInfo = new[]
			{
				("reg006", "cancelee1@example.com"),
				("reg007", "cancelee2@example.com"),
				("reg008", "cancelee3@example.com")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.CancelRegistrantsAsync(webinarId, registrantsInfo, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Additional Get Tests with Variations

		[Fact]
		public async Task GetAsync_WithOccurrenceId_ReturnsWebinar()
		{
			// Arrange
			var webinarId = 3333333333L;
			var occurrenceId = "occurrence123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("occurrence_id", occurrenceId)
				.WithQueryString("show_previous_occurrences", "false")
				.Respond("application/json", EndpointsResource.webinars__webinarId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetAsync(webinarId, occurrenceId, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAsync_WithIncludePreviousOccurrences_ReturnsWebinar()
		{
			// Arrange
			var webinarId = 3333333333L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("show_previous_occurrences", "true")
				.Respond("application/json", EndpointsResource.webinars__webinarId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetAsync(webinarId, null, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteAsync_WithOccurrenceId_Succeeds()
		{
			// Arrange
			var webinarId = 3333333333L;
			var occurrenceId = "occurrence456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("occurrence_id", occurrenceId)
				.WithQueryString("cancel_webinar_reminder", "false")
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteAsync(webinarId, occurrenceId, false, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_WithSendNotification_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString()))
				.WithQueryString("cancel_webinar_reminder", "true")
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteAsync(webinarId, null, true, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetRegistrantsAsync_WithTrackingSourceId_ReturnsRegistrants()
		{
			// Arrange
			var webinarId = 1234567890L;
			var status = RegistrantStatus.Approved;
			var trackingSourceId = "source1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants"))
				.WithQueryString("status", status.ToEnumString())
				.WithQueryString("tracking_source_id", trackingSourceId)
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrantsAsync(webinarId, status, trackingSourceId, null, 30, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetRegistrantAsync_WithOccurrenceId_ReturnsRegistrant()
		{
			// Arrange
			var webinarId = 3333333333L;
			var registrantId = "reg123";
			var occurrenceId = "occurrence789";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", registrantId))
				.WithQueryString("occurrence_id", occurrenceId)
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants__registrantId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrantAsync(webinarId, registrantId, occurrenceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task AddPanelistsAsync_WithMultiplePanelists_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var panelists = new[]
			{
				("panelist1@example.com", "Panelist One", "vbg_001"),
				("panelist2@example.com", "Panelist Two", (string)null),
				("panelist3@example.com", "Panelist Three", "vbg_003")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "panelists"))
				.Respond(System.Net.HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.AddPanelistsAsync(webinarId, panelists, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Poll Tests

		[Fact]
		public async Task GetPollsAsync_WithWebinarId_ReturnsPolls()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__polls_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetPollsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetPollAsync_WithPollId_ReturnsPoll()
		{
			// Arrange
			var webinarId = 1234567890L;
			var pollId = 12345L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond("application/json", EndpointsResource.webinars__webinarId__polls__pollId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetPollAsync(webinarId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Title.ShouldBe("Webinar Feedback");
		}

		[Fact]
		public async Task CreatePollAsync_WithBasicQuestions_CreatesPoll()
		{
			// Arrange
			var webinarId = 1234567890L;
			var title = "Webinar Feedback";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "How satisfied are you?",
					Type = PollQuestionType.SingleChoice,
					IsRequired = true,
					Answers = new[] { "Excellent", "Good", "Fair", "Poor" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__polls_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreatePollAsync(webinarId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Title.ShouldBe("Webinar Feedback");
		}

		[Fact]
		public async Task CreatePollAsync_WithMultipleQuestions_CreatesPoll()
		{
			// Arrange
			var webinarId = 1234567890L;
			var title = "Comprehensive Survey";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Rate the content",
					Type = PollQuestionType.SingleChoice,
					IsRequired = true,
					Answers = new[] { "Excellent", "Good", "Fair", "Poor" }
				},
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Would you recommend this?",
					Type = PollQuestionType.MultipleChoice,
					IsRequired = false,
					Answers = new[] { "Yes", "No", "Maybe" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__polls_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreatePollAsync(webinarId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreatePollAsync_WithNullQuestions_CreatesPoll()
		{
			// Arrange
			var webinarId = 1234567890L;
			var title = "Empty Poll";
			IEnumerable<PollQuestionForMeetingOrWebinar> questions = null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__polls_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.CreatePollAsync(webinarId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdatePollAsync_WithUpdatedTitle_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var pollId = 12345L;
			var title = "Updated Feedback Survey";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "How satisfied are you?",
					Type = PollQuestionType.SingleChoice,
					IsRequired = true,
					Answers = new[] { "Excellent", "Good", "Fair", "Poor" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdatePollAsync(webinarId, pollId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdatePollAsync_WithNewQuestions_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var pollId = 12345L;
			var title = "Updated Poll";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Rate your experience",
					Type = PollQuestionType.RatingScale,
					IsRequired = true,
					RatingMinimumValue = 1,
					RatingMaximumValue = 5
				},
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Additional feedback",
					Type = PollQuestionType.Short,
					IsRequired = false,
					MinimumNumberOfCharacters = 10,
					MaximumNumberOfCharacters = 500
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdatePollAsync(webinarId, pollId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdatePollAsync_WithMultipleChoiceQuestion_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var pollId = 12345L;
			var title = "Multi-Select Survey";
			var questions = new[]
			{
				new PollQuestionForMeetingOrWebinar
				{
					Question = "Which topics interest you?",
					Type = PollQuestionType.MultipleChoice,
					IsRequired = false,
					Answers = new[] { "Technology", "Business", "Marketing", "Design" }
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdatePollAsync(webinarId, pollId, title, questions, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeletePollAsync_WithPollId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var pollId = 12345L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeletePollAsync(webinarId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeletePollAsync_WithDifferentWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 9876543210L;
			var pollId = 54321L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "polls", pollId.ToString()))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeletePollAsync(webinarId, pollId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetTrackingSourcesAsync_WithWebinarId_ReturnsTrackingSources()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "tracking_sources"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__tracking_sources_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetTrackingSourcesAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetLiveStreamSettingsAsync_WithWebinarId_ReturnsSettings()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__livestream_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetLiveStreamSettingsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Url.ShouldBe("https://stream.example.com/live");
		}

		[Fact]
		public async Task GetSurveyAsync_WithWebinarId_ReturnsSurvey()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "survey"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__survey_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetSurveyAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetTemplatesAsync_WithUserId_ReturnsTemplates()
		{
			// Arrange
			var userId = "user123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "webinar_templates"))
				.Respond("application/json", EndpointsResource.users__userId__webinar_templates_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetTemplatesAsync(userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("template001");
			result[0].Name.ShouldBe("Standard Webinar Template");
			result[1].Id.ShouldBe("template002");
			result[1].Name.ShouldBe("Training Webinar Template");
		}

		[Fact]
		public async Task UpdateLiveStreamAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;
			var streamUrl = "https://stream.example.com/live";
			var streamKey = "streamkey123";
			var pageUrl = "https://example.com/webinar";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateLiveStreamAsync(webinarId, streamUrl, streamKey, pageUrl, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateLiveStreamAsync_WithDifferentUrls_Succeeds()
		{
			// Arrange
			var webinarId = 9876543210L;
			var streamUrl = "rtmp://stream.custom.com/live";
			var streamKey = "custom-key-456";
			var pageUrl = "https://custom.com/watch";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.UpdateLiveStreamAsync(webinarId, streamUrl, streamKey, pageUrl, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StopLiveStreamAsync_WithWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.StopLiveStreamAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task StopLiveStreamAsync_WithDifferentWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("webinars", webinarId.ToString(), "livestream", "status"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.StopLiveStreamAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSurveyAsync_WithWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteSurveyAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteSurveyAsync_WithDifferentWebinarId_Succeeds()
		{
			// Arrange
			var webinarId = 9876543210L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "survey"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			await webinars.DeleteSurveyAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetRegistrationQuestionsAsync_WithWebinarId_ReturnsQuestions()
		{
			// Arrange
			var webinarId = 1234567890L;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("webinars", webinarId.ToString(), "registrants", "questions"))
				.Respond("application/json", EndpointsResource.webinars__webinarId__registrants_questions_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var webinars = new Webinars(client);

			// Act
			var result = await webinars.GetRegistrationQuestionsAsync(webinarId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
