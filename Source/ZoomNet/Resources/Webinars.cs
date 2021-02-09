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
				.WithArgument("page", page)
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
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer. If passed in, Zoom advise using the userId in the <paramref name="userId"/> field, rather than email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		public Task<ScheduledWebinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{ "type", 5 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("timezone", "UTC");
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
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		public Task<RecurringWebinar> CreateRecurringWebinarAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
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
			data.AddPropertyIfValue("start_time", start?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("recurrence", recurrence);
			data.AddPropertyIfValue("timezone", "UTC");
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

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
		/// <param name="settings">Webinar settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateWebinarOccurrenceAsync(long webinarId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, WebinarSettings settings = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("start_time", start?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			if (start.HasValue) data.Add("timezone", "UTC");
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
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public async Task UpdateScheduledWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			if (start.HasValue) data.Add("timezone", "UTC");
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

			var result = await _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage()
				.ConfigureAwait(false);

			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				// Zoom returns an HTTP 200 message when there is no webinar subscription and instead returns a 204 after a successful update
				throw new NotSupportedException("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added");
			}
		}

		/// <summary>
		/// Updates an existing recurring webinar for a user.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public async Task UpdateRecurringWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, RecurrenceInfo recurrence = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("start_time", start?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("recurrence", recurrence);
			if (start.HasValue) data.Add("timezone", "UTC");
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

			var result = await _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage()
				.ConfigureAwait(false);

			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				// Zoom returns an HTTP 200 message when there is no webinar subscription and instead returns a 204 after a successful update
				throw new NotSupportedException("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added");
			}
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
		/// The unique identifier of the new panelist.
		/// </returns>
		public async Task<Panelist> AddPanelistAsync(long webinarId, string email, string fullName, CancellationToken cancellationToken = default)
		{
			var panelists = await AddPanelistsAsync(webinarId, new (string Email, string FullName)[] { (email, fullName) }, cancellationToken).ConfigureAwait(false);
			return panelists.FirstOrDefault();
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
		public Task<Panelist[]> AddPanelistsAsync(long webinarId, IEnumerable<(string Email, string FullName)> panelists, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("panelists", panelists.Select(p => new { email = p.Email, name = p.FullName }).ToArray());

			return _client
				.PostAsync($"webinars/{webinarId}/panelists")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Panelist[]>();
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
				.WithArgument("page", page)
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
		/// <param name="firstName">User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Registrant" />.
		/// </returns>
		public Task<Registrant> AddRegistrantAsync(long webinarId, string email, string firstName, string lastName, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("email", email);
			data.AddPropertyIfValue("first_name", firstName);
			data.AddPropertyIfValue("last_name", lastName);

			return _client
				.PostAsync($"webinars/{webinarId}/registrants")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Registrant>();
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
		public Task<PollQuestion[]> GetRegistrationQuestions(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/registrants/questions")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollQuestion[]>("custom_questions");
		}

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="customQuestions">The questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateRegistrationQuestions(long webinarId, IEnumerable<PollQuestion> customQuestions, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("custom_questions", customQuestions);

			return _client
				.PutAsync($"webinars/{webinarId}/registrants/questions")
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

		/// <summary>
		/// Retrieve all the templates created for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Template" />.
		/// </returns>
		public Task<Template[]> GetWebinarTemplatesAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/webinar_templates")
				.WithCancellationToken(cancellationToken)
				.AsObject<Template[]>("templates");
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
