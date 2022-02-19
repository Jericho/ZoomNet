using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage webinars.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IWebinars" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/webinars/">Zoom documentation</a> for more information.
	/// </remarks>
	public class Webinars : IWebinars
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Webinars" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Webinars(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Webinar" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Webinar>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/webinars")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Webinar>("webinars", new WebinarConverter());
		}

		/// <summary>
		/// Retrieve all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Webinar" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<Webinar>> GetAllAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/webinars")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Webinar>("webinars", new WebinarConverter());
		}

		/// <summary>
		/// Creates a scheduled webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer. If passed in, Zoom advise using the userId in the <paramref name="userId"/> field, rather than email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		public Task<ScheduledWebinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "type", 5 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start.ToZoomFormat(timeZone));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfEnumValue("timezone", timeZone);
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));
			data.AddPropertyIfValue("template_id", templateId);

			return _client
				.PostAsync($"users/{userId}/webinars")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ScheduledWebinar>();
		}

		/// <summary>
		/// Creates a recurring webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer. If passed in, Zoom advise using the userId in the <paramref name="userId"/> field, rather than email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		public Task<RecurringWebinar> CreateRecurringWebinarAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				// 6 = Recurring with no fixed time
				// 9 = Recurring with fixed time
				{ "type", start.HasValue ? 9 : 6 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start.ToZoomFormat(timeZone));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("recurrence", recurrence);
			data.AddPropertyIfEnumValue("timezone", timeZone);
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));
			data.AddPropertyIfValue("template_id", templateId);

			return _client
				.PostAsync($"users/{userId}/webinars")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringWebinar>();
		}

		/// <summary>
		/// Update the details of a webinar occurence.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateWebinarOccurrenceAsync(long webinarId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, WebinarSettings settings = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("start_time", start.ToZoomFormat(timeZone));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfEnumValue("timezone", timeZone);
			data.AddPropertyIfValue("settings", settings);

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Updates an existing scheduled webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateScheduledWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start.ToZoomFormat(timeZone));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfEnumValue("timezone", timeZone);
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Updates an existing recurring webinar for a user.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateRecurringWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, RecurrenceInfo recurrence = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start.ToZoomFormat(timeZone));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("recurrence", recurrence);
			data.AddPropertyIfEnumValue("timezone", timeZone);
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the details of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Webinar" />.
		/// </returns>
		public Task<Webinar> GetAsync(long webinarId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsObject<Webinar>(jsonConverter: new WebinarConverter());
		}

		/// <summary>
		/// Delete a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="sendNotification">If true, a notification email is sent to the panelists and registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAsync(long webinarId, string occurrenceId = null, bool sendNotification = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("cancel_webinar_reminder", sendNotification.ToString().ToLower())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// End a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task EndAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "action", "end" }
			};

			return _client
				.PutAsync($"webinars/{webinarId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// List panelists of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Panelist"/>.
		/// </returns>
		public Task<Panelist[]> GetPanelistsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/panelists")
				.WithCancellationToken(cancellationToken)
				.AsObject<Panelist[]>("panelists");
		}

		/// <summary>
		/// Add a single panelist to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="email">Panelist's email address.</param>
		/// <param name="fullName">Panelist's full name.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task AddPanelistAsync(long webinarId, string email, string fullName, CancellationToken cancellationToken = default)
		{
			return AddPanelistsAsync(webinarId, new (string Email, string FullName)[] { (email, fullName) }, cancellationToken);
		}

		/// <summary>
		/// Add multiple panelists to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="panelists">The panelists to add to the webinar.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task AddPanelistsAsync(long webinarId, IEnumerable<(string Email, string FullName)> panelists, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("panelists", panelists.Select(p => new { email = p.Email, name = p.FullName }).ToArray());

			return _client
				.PostAsync($"webinars/{webinarId}/panelists")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Remove a single panelist from a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="panelistId">Panelist's email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RemovePanelistAsync(long webinarId, string panelistId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/panelists/{panelistId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Remove all panelists from a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RemoveAllPanelistsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/panelists")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// List the users that have registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"webinars/{webinarId}/registrants")
				.WithArgument("status", JToken.Parse(JsonConvert.SerializeObject(status)).ToString())
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
		}

		/// <summary>
		/// List the users that have registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"webinars/{webinarId}/registrants")
				.WithArgument("status", JToken.Parse(JsonConvert.SerializeObject(status)).ToString())
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

		/// <summary>
		/// Get details on a specific user who registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Registrant" />.
		/// </returns>
		public Task<Registrant> GetRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/registrants/{registrantId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsObject<Registrant>();
		}

		/// <summary>
		/// Add a registrant to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="email">A valid email address.</param>
		/// <param name="firstName">Registrant's first name.</param>
		/// <param name="lastName">Registrant's last name.</param>
		/// <param name="address">Registrant's address.</param>
		/// <param name="city">Registrant's city.</param>
		/// <param name="country">Registrant's country.</param>
		/// <param name="postalCode">Registrant's zip or postal code.</param>
		/// <param name="stateOrProvince">Registrant's state or province.</param>
		/// <param name="phoneNumber">Registrant's phone number.</param>
		/// <param name="industry">Registrant's industry.</param>
		/// <param name="organization">Registrant's organization.</param>
		/// <param name="jobTitle">Registrant's job title.</param>
		/// <param name="timeFrame">This field can be used to gauge interest of attendees towards buying your product or service.</param>
		/// <param name="role">Registrant's role in purchase decision.</param>
		/// <param name="employees">Number of employees.</param>
		/// <param name="comments">A field that allows registrant to provide any questions or comments that they might have.</param>
		/// <param name="questionAnswers">Answers to the custom registration questions.</param>
		/// <param name="language">Registrant's language preference for confirmation emails.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="RegistrantInfo" />.
		/// </returns>
		public Task<RegistrantInfo> AddRegistrantAsync(long webinarId, string email, string firstName, string lastName, string address = null, string city = null, Country? country = null, string postalCode = null, string stateOrProvince = null, string phoneNumber = null, string industry = null, string organization = null, string jobTitle = null, PurchasingTimeFrame? timeFrame = null, RoleInPurchaseProcess? role = null, NumberOfEmployees? employees = null, string comments = null, IEnumerable<RegistrationAnswer> questionAnswers = null, Language? language = null, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("email", email);
			data.AddPropertyIfValue("first_name", firstName);
			data.AddPropertyIfValue("last_name", lastName);
			data.AddPropertyIfValue("address", address);
			data.AddPropertyIfValue("city", city);
			data.AddPropertyIfValue("country", country);
			data.AddPropertyIfValue("zip", postalCode);
			data.AddPropertyIfValue("state", stateOrProvince);
			data.AddPropertyIfValue("phone", phoneNumber);
			data.AddPropertyIfValue("industry", industry);
			data.AddPropertyIfValue("org", organization);
			data.AddPropertyIfValue("job_title", jobTitle);
			data.AddPropertyIfValue("purchasing_time_frame", timeFrame);
			data.AddPropertyIfValue("role_in_purchase_process", role);
			data.AddPropertyIfValue("no_of_employees", employees);
			data.AddPropertyIfValue("custom_questions", questionAnswers);
			data.AddPropertyIfValue("language", language);
			data.AddPropertyIfValue("comments", comments);

			return _client
				.PostAsync($"webinars/{webinarId}/registrants")
				.WithArgument("occurence_ids", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RegistrantInfo>();
		}

		/// <summary>
		/// Delete a webinar registrant.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant id.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/registrants/{registrantId}")
				.WithArgument("occurence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Approve a registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Approve multiple registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be approved.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "approve", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Reject a registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return RejectRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Reject multiple registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be rejected.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "deny", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Cancel a previously approved registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return CancelRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Cancel multiple previously approved registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be cancelled.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "cancel", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Retrieve all polls for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Poll" />.
		/// </returns>
		public Task<Poll[]> GetPollsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll[]>("polls");
		}

		/// <summary>
		/// Create a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task<Poll> CreatePoll(long webinarId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "title", title }
			};
			data.AddPropertyIfValue("questions", questions);

			return _client
				.PostAsync($"webinars/{webinarId}/polls")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll>();
		}

		/// <summary>
		/// Retrieve a poll.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Poll" />.
		/// </returns>
		public Task<Poll> GetPollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll>();
		}

		/// <summary>
		/// Update a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdatePollAsync(long webinarId, long pollId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("title", title);
			data.AddPropertyIfValue("questions", questions);

			return _client
				.PutAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeletePollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollQuestion"/>.
		/// </returns>
		public async Task<RegistrationQuestionsForWebinar> GetRegistrationQuestionsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"webinars/{webinarId}/registrants/questions")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonObject()
				.ConfigureAwait(false);

			var allFields = ((JArray)response.Property("questions")?.Value ?? Enumerable.Empty<JToken>())
				.Select(item => (Field: item.GetPropertyValue<string>("field_name").ToEnum<RegistrationField>(), IsRequired: item.GetPropertyValue<bool>("required")));
			var requiredFields = allFields.Where(f => f.IsRequired).Select(f => f.Field).ToArray();
			var optionalFields = allFields.Where(f => !f.IsRequired).Select(f => f.Field).ToArray();

			var registrationQuestions = new RegistrationQuestionsForWebinar
			{
				RequiredFields = requiredFields,
				OptionalFields = optionalFields,
				Questions = response.Property("custom_questions")?.Value.ToObject<RegistrationCustomQuestionForWebinar[]>() ?? Array.Empty<RegistrationCustomQuestionForWebinar>()
			};
			return registrationQuestions;
		}

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="requiredFields">List of fields that must be answer when registering for the webinar.</param>
		/// <param name="optionalFields">List of fields that can be answer when registering for the webinar.</param>
		/// <param name="customQuestions">Additional questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateRegistrationQuestionsAsync(long webinarId, IEnumerable<RegistrationField> requiredFields, IEnumerable<RegistrationField> optionalFields, IEnumerable<RegistrationCustomQuestionForWebinar> customQuestions, CancellationToken cancellationToken = default)
		{
			var required = (requiredFields ?? Enumerable.Empty<RegistrationField>())
				.GroupBy(f => f).Select(grp => grp.First()); // Remove duplicates

			var optional = (optionalFields ?? Enumerable.Empty<RegistrationField>())
				.Except(required) // Remove 'optional' fields that are on the 'required' enumeration
				.GroupBy(f => f).Select(grp => grp.First()); // Remove duplicates

			var standardFields = required.Select(f => new JObject() { { "field_name", f.ToEnumString() }, { "required", true } })
				.Union(optional.Select(f => new JObject() { { "field_name", f.ToEnumString() }, { "required", false } }))
				.ToArray();

			var data = new JObject();
			data.AddPropertyIfValue("questions", standardFields);
			data.AddPropertyIfValue("custom_questions", customQuestions);

			return _client
				.PatchAsync($"webinars/{webinarId}/registrants/questions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve all the tracking sources of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="TrackingSource" />.
		/// </returns>
		public Task<TrackingSource[]> GetTrackingSourcesAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/tracking_sources")
				.WithCancellationToken(cancellationToken)
				.AsObject<TrackingSource[]>("tracking_sources");
		}

		/// <inheritdoc/>
		public Task<WebinarTemplate[]> GetTemplatesAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/webinar_templates")
				.WithCancellationToken(cancellationToken)
				.AsObject<WebinarTemplate[]>("templates");
		}

		/// <inheritdoc/>
		public Task UpdateLiveStreamAsync(long webinarId, string streamUrl, string streamKey, string pageUrl, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "stream_url", streamUrl },
				{ "stream_key", streamKey },
				{ "page_url", pageUrl }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}/livestream")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StartLiveStreamAsync(long webinarId, bool displaySpeakerName, string speakerName, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "action", "start" },
				{
					"settings", new JObject()
					{
						{ "active_speaker_name", displaySpeakerName },
						{ "display_name", speakerName }
					}
				}
			};

			return _client
				.PatchAsync($"webinars/{webinarId}/livestream/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StopLiveStreamAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "action", "stop" }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}/livestream/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<LiveStreamingSettings> GetLiveStreamSettingsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/livestream")
				.WithCancellationToken(cancellationToken)
				.AsObject<LiveStreamingSettings>();
		}

		/// <inheritdoc/>
		public Task<InviteLink[]> CreateInviteLinksAsync(long webinarId, IEnumerable<string> names, long timeToLive = 7200, CancellationToken cancellationToken = default)
		{
			if (names == null || !names.Any()) throw new ArgumentNullException("You must provide at least one name", nameof(names));

			var data = new JObject()
			{
				{ "ttl", timeToLive }
			};
			data.AddPropertyIfValue("attendees", names?.Select(n => new JObject { { "name", n } }).ToArray());

			return _client
				.PostAsync($"webinars/{webinarId}/invite_links")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<InviteLink[]>("attendees");
		}

		/// <inheritdoc/>
		public Task DeleteSurveyAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/survey")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Survey> GetSurveyAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/survey")
				.WithCancellationToken(cancellationToken)
				.AsObject<Survey>();
		}

		/// <inheritdoc/>
		public Task UpdateSurveyAsync(long webinarId, IEnumerable<SurveyQuestion> questions = null, bool allowAnonymous = true, bool showInBrowser = true, string thirdPartySurveyLink = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("third_party_survey", thirdPartySurveyLink);
			data.AddPropertyIfValue("show_in_the_browser", showInBrowser);
			data.AddPropertyIfValue("custom_survey/anonymous", allowAnonymous);
			data.AddPropertyIfValue("custom_survey/questions", questions?.ToArray());

			return _client
				.PatchAsync($"webinars/{webinarId}/survey")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		private Task UpdateRegistrantsStatusAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string status, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("action", status);
			data.AddPropertyIfValue("registrants", registrantsInfo.Select(ri => new { id = ri.RegistrantId, email = ri.RegistrantEmail }).ToArray());

			return _client
				.PutAsync($"webinars/{webinarId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
