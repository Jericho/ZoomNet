using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Webinars : IWebinars
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Webinars" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Webinars(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<WebinarSummary>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"users/{userId}/webinars")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<WebinarSummary>("webinars");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<WebinarSummary>> GetAllAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"users/{userId}/webinars")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<WebinarSummary>("webinars");
		}

		/// <inheritdoc/>
		public Task<ScheduledWebinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "type", 5 },
				{ "topic", topic },
				{ "agenda", agenda },
				{ "password", password },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() },
				{ "template_id", templateId }
			};

			return _client
				.PostAsync($"users/{userId}/webinars")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ScheduledWebinar>();
		}

		/// <inheritdoc/>
		public Task<RecurringWebinar> CreateRecurringWebinarAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "type", start.HasValue ? 9 : 6 }, // 9 = Recurring with fixed time. 6 = Recurring with no fixed time
				{ "topic", topic },
				{ "agenda", agenda },
				{ "password", password },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "recurrence", recurrence },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() },
				{ "template_id", templateId }
			};

			return _client
				.PostAsync($"users/{userId}/webinars")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringWebinar>();
		}

		/// <inheritdoc/>
		public Task UpdateWebinarOccurrenceAsync(long webinarId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, WebinarSettings settings = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "agenda", agenda },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "timezone", timeZone },
				{ "settings", settings }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateScheduledWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "topic", topic },
				{ "agenda", agenda },
				{ "password", password },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateRecurringWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, RecurrenceInfo recurrence = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "topic", topic },
				{ "agenda", agenda },
				{ "password", password },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "recurrence", recurrence },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("Webinar subscription plan is missing. Enable webinar for this user once the subscription is added.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Webinar> GetAsync(long webinarId, string occurrenceId = null, bool includePreviousOccurrences = false, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("show_previous_occurrences", includePreviousOccurrences.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsObject<Webinar>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(long webinarId, string occurrenceId = null, bool sendNotification = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("cancel_webinar_reminder", sendNotification.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task EndAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "end" }
			};

			return _client
				.PutAsync($"webinars/{webinarId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Panelist[]> GetPanelistsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/panelists")
				.WithCancellationToken(cancellationToken)
				.AsObject<Panelist[]>("panelists");
		}

		/// <inheritdoc/>
		public Task AddPanelistAsync(long webinarId, string email, string fullName, string virtualBackgroundId = null, CancellationToken cancellationToken = default)
		{
			return AddPanelistsAsync(webinarId, new[] { (email, fullName, virtualBackgroundId) }, cancellationToken);
		}

		/// <inheritdoc/>
		public Task AddPanelistsAsync(long webinarId, IEnumerable<(string Email, string FullName, string VirtualBackgroundId)> panelists, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{
					"panelists", panelists?.Select(p => new JsonObject
					{
						{ "email", p.Email },
						{ "name", p.FullName },
						{ "virtual_background_id", p.VirtualBackgroundId },
					}).ToArray()
				}
			};

			return _client
				.PostAsync($"webinars/{webinarId}/panelists")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemovePanelistAsync(long webinarId, string panelistId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/panelists/{panelistId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemoveAllPanelistsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/panelists")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string trackingSourceId = null, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"webinars/{webinarId}/registrants")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("tracking_source_id", trackingSourceId)
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string trackingSourceId = null, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"webinars/{webinarId}/registrants")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("tracking_source_id", trackingSourceId)
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<Registrant> GetRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/registrants/{registrantId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsObject<Registrant>();
		}

		/// <inheritdoc/>
		public Task<RegistrantInfo> AddRegistrantAsync(long webinarId, string email, string firstName, string lastName, string address = null, string city = null, Country? country = null, string postalCode = null, string stateOrProvince = null, string phoneNumber = null, string industry = null, string organization = null, string jobTitle = null, PurchasingTimeFrame? timeFrame = null, RoleInPurchaseProcess? role = null, NumberOfEmployees? employees = null, string comments = null, IEnumerable<RegistrationAnswer> questionAnswers = null, Language? language = null, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "email", email },
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "address", address },
				{ "city", city },
				{ "country", country },
				{ "zip", postalCode },
				{ "state", stateOrProvince },
				{ "phone", phoneNumber },
				{ "industry", industry },
				{ "org", organization },
				{ "job_title", jobTitle },
				{ "purchasing_time_frame", timeFrame },
				{ "role_in_purchase_process", role },
				{ "no_of_employees", employees },
				{ "custom_questions", questionAnswers?.ToArray() },
				{ "language", language },
				{ "comments", comments }
			};

			return _client
				.PostAsync($"webinars/{webinarId}/registrants")
				.WithArgument("occurence_ids", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RegistrantInfo>();
		}

		/// <inheritdoc/>
		public Task<BatchRegistrantInfo[]> PerformBatchRegistrationAsync(long webinarId, IEnumerable<BatchRegistrant> registrants, bool autoApprove = false, CancellationToken cancellationToken = default)
		{
			if (registrants == null || !registrants.Any()) throw new ArgumentNullException(nameof(registrants), "You must provide at least one registrant");
			if (registrants.Count() > 30) throw new ArgumentOutOfRangeException(nameof(registrants), "You can register up to 30 registrants at once");

			var data = new JsonObject
			{
				{ "auto_approve", autoApprove },
				{ "registrants", registrants.ToArray() },
			};

			return _client
				.PostAsync($"webinars/{webinarId}/batch_registrants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<BatchRegistrantInfo[]>("registrants");
		}

		/// <inheritdoc/>
		public Task DeleteRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/registrants/{registrantId}")
				.WithArgument("occurence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task ApproveRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task ApproveRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "approve", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task RejectRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return RejectRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task RejectRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "deny", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task CancelRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return CancelRegistrantsAsync(webinarId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task CancelRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(webinarId, registrantsInfo, "cancel", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<PollForMeetingOrWebinar[]> GetPollsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollForMeetingOrWebinar[]>("polls");
		}

		/// <inheritdoc/>
		public Task<PollForMeetingOrWebinar> CreatePoll(long webinarId, string title, IEnumerable<PollQuestionForMeetingOrWebinar> questions, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "title", title },
				{ "questions", questions?.ToArray() }
			};

			return _client
				.PostAsync($"webinars/{webinarId}/polls")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<PollForMeetingOrWebinar>();
		}

		/// <inheritdoc/>
		public Task<PollForMeetingOrWebinar> GetPollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollForMeetingOrWebinar>();
		}

		/// <inheritdoc/>
		public Task UpdatePollAsync(long webinarId, long pollId, string title, IEnumerable<PollQuestionForMeetingOrWebinar> questions, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "title", title },
				{ "questions", questions?.ToArray() }
			};

			return _client
				.PutAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeletePollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"webinars/{webinarId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<RegistrationQuestionsForWebinar> GetRegistrationQuestionsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"webinars/{webinarId}/registrants/questions")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var allFields = response.GetProperty("questions").EnumerateArray()
				.Select(item => (Field: item.GetPropertyValue<string>("field_name").ToEnum<RegistrationField>(), IsRequired: item.GetPropertyValue<bool>("required")));

			var requiredFields = allFields.Where(f => f.IsRequired).Select(f => f.Field).ToArray();
			var optionalFields = allFields.Where(f => !f.IsRequired).Select(f => f.Field).ToArray();

			var registrationQuestions = new RegistrationQuestionsForWebinar
			{
				RequiredFields = requiredFields,
				OptionalFields = optionalFields,
				Questions = response.GetProperty("custom_questions", false)?.ToObject<RegistrationCustomQuestionForWebinar[]>() ?? Array.Empty<RegistrationCustomQuestionForWebinar>()
			};
			return registrationQuestions;
		}

		/// <inheritdoc/>
		public Task UpdateRegistrationQuestionsAsync(long webinarId, IEnumerable<RegistrationField> requiredFields, IEnumerable<RegistrationField> optionalFields, IEnumerable<RegistrationCustomQuestionForWebinar> customQuestions, CancellationToken cancellationToken = default)
		{
			var required = (requiredFields ?? Enumerable.Empty<RegistrationField>())
				.GroupBy(f => f).Select(grp => grp.First()); // Remove duplicates

			var optional = (optionalFields ?? Enumerable.Empty<RegistrationField>())
				.Except(required) // Remove 'optional' fields that are on the 'required' enumeration
				.GroupBy(f => f).Select(grp => grp.First()); // Remove duplicates

			var standardFields = required.Select(f => new JsonObject { { "field_name", f.ToEnumString() }, { "required", true } })
				.Union(optional.Select(f => new JsonObject { { "field_name", f.ToEnumString() }, { "required", false } }))
				.ToArray();

			var data = new JsonObject
			{
				{ "questions", standardFields },
				{ "custom_questions", customQuestions?.ToArray() }
			};

			return _client
				.PatchAsync($"webinars/{webinarId}/registrants/questions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
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
			var data = new JsonObject
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
			var data = new JsonObject
			{
				{ "action", "start" },
				{
					"settings", new JsonObject
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
			var data = new JsonObject
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
			if (names == null || !names.Any()) throw new ArgumentNullException(nameof(names), "You must provide at least one name");

			var data = new JsonObject
			{
				{ "ttl", timeToLive },
				{ "attendees", names?.Select(n => new JsonObject { { "name", n } }).ToArray() }
			};

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
			var data = new JsonObject
			{
				{ "third_party_survey", thirdPartySurveyLink },
				{ "show_in_the_browser", showInBrowser },
				{
					"custom_survey",
					new JsonObject
					{
						{ "anonymous", allowAnonymous },
						{ "questions", questions?.ToArray() }
					}
				}
			};

			return _client
				.PatchAsync($"webinars/{webinarId}/survey")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		private Task UpdateRegistrantsStatusAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string status, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", status },
				{ "registrants", registrantsInfo?.Select(ri => new JsonObject { { "id", ri.RegistrantId }, { "email", ri.RegistrantEmail } }).ToArray() },
			};

			return _client
				.PutAsync($"webinars/{webinarId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
