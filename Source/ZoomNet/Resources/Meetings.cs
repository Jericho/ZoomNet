using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Meetings : IMeetings
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Meetings" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Meetings(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<MeetingSummary>> GetAllAsync(string userId, MeetingListType type = MeetingListType.Scheduled, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/meetings")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<MeetingSummary>("meetings");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<MeetingSummary>> GetAllAsync(string userId, MeetingListType type = MeetingListType.Scheduled, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/meetings")
				.WithArgument("type", type.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<MeetingSummary>("meetings");
		}

		/// <inheritdoc/>
		public Task<InstantMeeting> CreateInstantMeetingAsync(
			string userId,
			string topic,
			string agenda,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			string templateId = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "type", 1 },
				{ "topic", topic },
				{ "password", password },
				{ "agenda", agenda },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() },
				{ "template_id", templateId }
			};

			return _client
				.PostAsync($"users/{userId}/meetings")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<InstantMeeting>();
		}

		/// <inheritdoc/>
		public Task<ScheduledMeeting> CreateScheduledMeetingAsync(
			string userId,
			string topic,
			string agenda,
			DateTime start,
			int duration,
			TimeZones? timeZone = TimeZones.UTC,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			string templateId = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "type", 2 },
				{ "topic", topic },
				{ "password", password },
				{ "agenda", agenda },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() },
				{ "template_id", templateId }
			};

			return _client
				.PostAsync($"users/{userId}/meetings")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ScheduledMeeting>();
		}

		/// <inheritdoc/>
		public Task<RecurringMeeting> CreateRecurringMeetingAsync(
			string userId,
			string topic,
			string agenda,
			DateTime? start,
			int duration,
			RecurrenceInfo recurrence,
			TimeZones? timeZone = TimeZones.UTC,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			string templateId = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "type", recurrence == null ? MeetingType.RecurringNoFixedTime : MeetingType.RecurringFixedTime },
				{ "topic", topic },
				{ "password", password },
				{ "agenda", agenda },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "recurrence", recurrence },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() },
				{ "template_id", templateId }
			};

			return _client
				.PostAsync($"users/{userId}/meetings")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringMeeting>();
		}

		/// <inheritdoc/>
		public Task<Meeting> GetAsync(long meetingId, string occurrenceId = null, bool includePreviousOccurrences = false, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("show_previous_occurrences", includePreviousOccurrences.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsObject<Meeting>();
		}

		/// <inheritdoc/>
		public Task UpdateMeetingOccurrenceAsync(long meetingId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, MeetingSettings settings = null, CancellationToken cancellationToken = default)
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
				.PatchAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateScheduledMeetingAsync(long meetingId, string userId = null, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "schedule_for", userId },
				{ "topic", topic },
				{ "password", password },
				{ "agenda", agenda },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() }
			};

			return _client
				.PatchAsync($"meetings/{meetingId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateRecurringMeetingAsync(long meetingId, string userId = null, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, RecurrenceInfo recurrence = null, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "schedule_for", userId },
				{ "topic", topic },
				{ "password", password },
				{ "agenda", agenda },
				{ "start_time", start.ToZoomFormat(timeZone) },
				{ "duration", duration },
				{ "recurrence", recurrence },
				{ "timezone", timeZone },
				{ "settings", settings },
				{ "tracking_fields", trackingFields?.Select(tf => new JsonObject { { "field", tf.Key }, { "value", tf.Value } }).ToArray() }
			};

			return _client
				.PatchAsync($"meetings/{meetingId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(long meetingId, string occurrenceId = null, bool notifyHost = true, bool notifyRegistrants = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("schedule_for_reminder", notifyHost.ToString().ToLowerInvariant())
				.WithArgument("cancel_meeting_reminder", notifyRegistrants.ToString().ToLowerInvariant())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task EndAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "end" }
			};

			return _client
				.PutAsync($"meetings/{meetingId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RecoverAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "recover" }
			};

			return _client
				.PutAsync($"meetings/{meetingId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		public Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long meetingId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/registrants")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page_number", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long meetingId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/registrants")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<RegistrantInfo> AddRegistrantAsync(long meetingId, string email, string firstName, string lastName, string address = null, string city = null, Country? country = null, string postalCode = null, string stateOrProvince = null, string phoneNumber = null, string industry = null, string organization = null, string jobTitle = null, PurchasingTimeFrame? timeFrame = null, RoleInPurchaseProcess? role = null, NumberOfEmployees? employees = null, string comments = null, IEnumerable<RegistrationAnswer> questionAnswers = null, Language? language = null, bool autoApprove = false, string occurrenceId = null, CancellationToken cancellationToken = default)
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
				{ "comments", comments },
				{ "auto_approve", autoApprove }
			};

			return _client
				.PostAsync($"meetings/{meetingId}/registrants")
				.WithArgument("occurence_ids", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RegistrantInfo>();
		}

		/// <inheritdoc/>
		public Task<BatchRegistrantInfo[]> PerformBatchRegistrationAsync(long meetingId, IEnumerable<BatchRegistrant> registrants, bool autoApprove = false, bool registrantsConfirmationEmail = false, CancellationToken cancellationToken = default)
		{
			if (registrants == null || !registrants.Any()) throw new ArgumentNullException(nameof(registrants), "You must provide at least one registrant");
			if (registrants.Count() > 30) throw new ArgumentOutOfRangeException(nameof(registrants), "You can register up to 30 registrants at once");

			var data = new JsonObject
			{
				{ "registrants", registrants.ToArray() },
				{ "auto_approve", autoApprove },
				{ "registrants_confirmation_email", registrantsConfirmationEmail },
			};

			return _client
				.PostAsync($"meetings/{meetingId}/batch_registrants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<BatchRegistrantInfo[]>("registrants");
		}

		/// <inheritdoc/>
		public Task DeleteRegistrantAsync(long meetingId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/registrants/{registrantId}")
				.WithArgument("occurence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Registrant> GetRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/registrants/{registrantId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Registrant>();
		}

		/// <inheritdoc/>
		public Task ApproveRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task ApproveRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "approve", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task RejectRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return RejectRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task RejectRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "deny", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task CancelRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return CancelRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task CancelRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "cancel", occurrenceId, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<Poll[]> GetPollsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll[]>("polls");
		}

		/// <inheritdoc/>
		public Task<Poll> CreatePollAsync(long meetingId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "title", title },
				{ "questions", questions?.ToArray() }
			};

			return _client
				.PostAsync($"meetings/{meetingId}/polls")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll>();
		}

		/// <inheritdoc/>
		public Task<Poll> GetPollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll>();
		}

		/// <inheritdoc/>
		public Task UpdatePollAsync(long meetingId, long pollId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "title", title },
				{ "questions", questions?.ToArray() }
			};

			return _client
				.PutAsync($"meetings/{meetingId}/polls/{pollId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeletePollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<RegistrationQuestionsForMeeting> GetRegistrationQuestionsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"meetings/{meetingId}/registrants/questions")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var allFields = response.GetProperty("questions").EnumerateArray()
				.Select(item => (Field: item.GetPropertyValue<string>("field_name").ToEnum<RegistrationField>(), IsRequired: item.GetPropertyValue<bool>("required")));

			var requiredFields = allFields.Where(f => f.IsRequired).Select(f => f.Field).ToArray();
			var optionalFields = allFields.Where(f => !f.IsRequired).Select(f => f.Field).ToArray();

			var registrationQuestions = new RegistrationQuestionsForMeeting
			{
				RequiredFields = requiredFields,
				OptionalFields = optionalFields,
				Questions = response.GetProperty("custom_questions", false)?.ToObject<RegistrationCustomQuestionForMeeting[]>() ?? Array.Empty<RegistrationCustomQuestionForMeeting>()
			};
			return registrationQuestions;
		}

		/// <inheritdoc/>
		public Task UpdateRegistrationQuestionsAsync(long meetingId, IEnumerable<RegistrationField> requiredFields, IEnumerable<RegistrationField> optionalFields, IEnumerable<RegistrationCustomQuestionForMeeting> customQuestions, CancellationToken cancellationToken = default)
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
				.PatchAsync($"meetings/{meetingId}/registrants/questions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<string> GetInvitationAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/invitation")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("invitation");
		}

		/// <inheritdoc/>
		public Task UpdateLiveStreamAsync(long meetingId, string streamUrl, string streamKey, string pageUrl, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "stream_url", streamUrl },
				{ "stream_key", streamKey },
				{ "page_url", pageUrl }
			};

			return _client
				.PatchAsync($"meetings/{meetingId}/livestream")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StartLiveStreamAsync(long meetingId, bool displaySpeakerName, string speakerName, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "start" },
				{ "settings/active_speaker_name", displaySpeakerName },
				{ "settings/display_name", speakerName }
			};

			return _client
				.PatchAsync($"meetings/{meetingId}/livestream/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StopLiveStreamAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "stop" }
			};

			return _client
				.PatchAsync($"meetings/{meetingId}/livestream/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<LiveStreamingSettings> GetLiveStreamSettingsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/livestream")
				.WithCancellationToken(cancellationToken)
				.AsObject<LiveStreamingSettings>();
		}

		/// <inheritdoc/>
		public Task<MeetingTemplate[]> GetTemplatesAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"users/{userId}/meeting_templates")
				.WithCancellationToken(cancellationToken)
				.AsObject<MeetingTemplate[]>("templates");
		}

		/// <inheritdoc/>
		public Task<InviteLink[]> CreateInviteLinksAsync(long meetingId, IEnumerable<string> names, long timeToLive = 7200, CancellationToken cancellationToken = default)
		{
			if (names == null || !names.Any()) throw new ArgumentNullException(nameof(names), "You must provide at least one name");

			var data = new JsonObject
			{
				{ "ttl", timeToLive },
				{ "attendees", names?.Select(n => new JsonObject { { "name", n } }).ToArray() }
			};

			return _client
				.PostAsync($"meetings/{meetingId}/invite_links")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<InviteLink[]>("attendees");
		}

		/// <inheritdoc/>
		public Task DeleteSurveyAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/survey")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Survey> GetSurveyAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/survey")
				.WithCancellationToken(cancellationToken)
				.AsObject<Survey>();
		}

		/// <inheritdoc/>
		public Task UpdateSurveyAsync(long meetingId, IEnumerable<SurveyQuestion> questions = null, bool allowAnonymous = true, bool showInBrowser = true, string thirdPartySurveyLink = null, CancellationToken cancellationToken = default)
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
				.PatchAsync($"meetings/{meetingId}/survey")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StartCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "recording.start" }
			};

			return _client
				.PatchAsync($"live_meetings/{meetingId}/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task PauseCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "recording.pause" }
			};

			return _client
				.PatchAsync($"live_meetings/{meetingId}/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task ResumeCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "recording.resume" }
			};

			return _client
				.PatchAsync($"live_meetings/{meetingId}/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task StopCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "recording.stop" }
			};

			return _client
				.PatchAsync($"live_meetings/{meetingId}/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task InviteParticipantsAsync(long meetingId, IEnumerable<string> emailAddresses, CancellationToken cancellationToken = default)
		{
			if (emailAddresses == null || !emailAddresses.Any()) throw new ArgumentNullException(nameof(emailAddresses), "You must provide at least one email address");

			var data = new JsonObject
			{
				{ "method", "participant.invite" },
				{ "params", new JsonObject { { "contacts", emailAddresses.Select(emailAddress => new JsonObject { { "email", emailAddress } }).ToArray() } } }
			};

			return _client
				.PatchAsync($"live_meetings/{meetingId}/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<string> CreateTemplateFromExistingMeetingAsync(string userId, long meetingId, string templateName, bool saveRecurrence = false, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "meeting_id", meetingId },
				{ "name", templateName },
				{ "save_recurrence", saveRecurrence },
				{ "overwrite", overwrite },
			};

			return _client
				.PostAsync($"users/{userId}/meeting_templates")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		/// <inheritdoc/>
		public Task<string> GetTokenForClosedCaptioningAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/token?type=closed_caption_token")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("token");
		}

		/// <inheritdoc/>
		public Task<string> GetTokenForLocalRecordingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/jointoken/local_recording")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("token");
		}

		/// <inheritdoc/>
		public Task<string> GetTokenForLocalArchivingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/jointoken/local_archiving")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("token");
		}

		/// <inheritdoc/>
		public Task<string> GetTokenForLiveStreamingAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/jointoken/live_streaming")
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("token");
		}

		private Task UpdateRegistrantsStatusAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string status, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", status },
				{ "registrants", registrantsInfo?.Select(ri => new { id = ri.RegistrantId, email = ri.RegistrantEmail }).ToArray() }
			};

			return _client
				.PutAsync($"meetings/{meetingId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
