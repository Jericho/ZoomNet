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
	/// <summary>
	/// Allows you to manage meetings.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IMeetings" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/meetings">Zoom documentation</a> for more information.
	/// </remarks>
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

		/// <summary>
		/// Creates an instant meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new <see cref="Meeting"/>.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
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

		/// <summary>
		/// Creates a scheduled meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new <see cref="Meeting"/>.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
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

		/// <summary>
		/// Creates a recurring meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time. If omitted, a 'Recurring meeting with no fixed time' will be created.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new <see cref="Meeting"/>.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
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

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		public Task<Meeting> GetAsync(long meetingId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsObject<Meeting>();
		}

		/// <summary>
		/// Update the details of a meeting occurence.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Update the details of a scheduled meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Update the details of a recurring meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time. If omitted, a 'Recurring meeting with no fixed time' will be created.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Delete a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="notifyHost">If true, a notification email is sent to the host and alternative host.</param>
		/// <param name="notifyRegistrants">If true, a notification email is sent to the registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAsync(long meetingId, string occurrenceId = null, bool notifyHost = true, bool notifyRegistrants = false, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("schedule_for_reminder", notifyHost.ToString().ToLower())
				.WithArgument("cancel_meeting_reminder", notifyRegistrants.ToString().ToLower())
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// End a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Recover a deleted meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
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

		/// <summary>
		/// List registrants of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
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

		/// <summary>
		/// List registrants of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
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

		/// <summary>
		/// Add a registrant to a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
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
		/// <param name="autoApprove">Indicates if the registrant should be automatically approved.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="RegistrantInfo" />.
		/// </returns>
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

		/// <summary>
		/// Register up to 30 registrants at once for a meeting that requires registration.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrants">An array of registrants.</param>
		/// <param name="autoApprove">Indicates if the registrant should be automatically approved.</param>
		/// <param name="registrantsConfirmationEmail">Indicates if send confirmation Email to Registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="BatchRegistrantInfo" />.
		/// </returns>
		public Task<BatchRegistrantInfo[]> PerformBatchRegistrationAsync(long meetingId, IEnumerable<BatchRegistrant> registrants, bool autoApprove = false, bool registrantsConfirmationEmail = false, CancellationToken cancellationToken = default)
		{
			if (registrants == null || registrants.Any() == false || registrants.Count() > 30)
			{
				throw new ArgumentOutOfRangeException("The registants count must be between 1 and 30.");
			}

			var data = new JsonObject
			{
				{ "registrants", registrants },
				{ "auto_approve", autoApprove },
				{ "registrants_confirmation_email", registrantsConfirmationEmail },
			};

			var response = _client
				.PostAsync($"meetings/{meetingId}/batch_registrants")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<BatchRegistrantInfo[]>("registrants");

			return response;
		}

		/// <summary>
		/// Delete a meeting registrant.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant id.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteRegistrantAsync(long meetingId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/registrants/{registrantId}")
				.WithArgument("occurence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve a meeting registrant.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Registrant"/>.
		/// </returns>
		public Task<Registrant> GetRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/registrants/{registrantId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Registrant>();
		}

		/// <summary>
		/// Approve a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return ApproveRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Approve multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be approved.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "approve", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Reject a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return RejectRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Reject multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be rejected.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task RejectRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "deny", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Cancel a previously approved registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return CancelRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Cancel multiple previously approved registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be cancelled.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default)
		{
			return UpdateRegistrantsStatusAsync(meetingId, registrantsInfo, "cancel", occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Retrieve all polls for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Poll" />.
		/// </returns>
		public Task<Poll[]> GetPollsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll[]>("polls");
		}

		/// <summary>
		/// Create a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Poll"/>.
		/// </returns>
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

		/// <summary>
		/// Retrieve a poll.
		/// </summary>
		/// <param name="meetingId">The meeting id.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Poll" />.
		/// </returns>
		public Task<Poll> GetPollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"meetings/{meetingId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Poll>();
		}

		/// <summary>
		/// Update a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Delete a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeletePollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}/polls/{pollId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the questions that are to be answered by users while registering for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollQuestion"/>.
		/// </returns>
		public async Task<RegistrationQuestionsForMeeting> GetRegistrationQuestionsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"meetings/{meetingId}/registrants/questions")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument()
				.ConfigureAwait(false);

			var allFields = response.RootElement.GetProperty("questions").EnumerateArray()
				.Select(item => (Field: item.GetPropertyValue<string>("field_name").ToEnum<RegistrationField>(), IsRequired: item.GetPropertyValue<bool>("required")));

			var requiredFields = allFields.Where(f => f.IsRequired).Select(f => f.Field).ToArray();
			var optionalFields = allFields.Where(f => !f.IsRequired).Select(f => f.Field).ToArray();

			var registrationQuestions = new RegistrationQuestionsForMeeting
			{
				RequiredFields = requiredFields,
				OptionalFields = optionalFields,
				Questions = response.RootElement.GetProperty("custom_questions", false)?.ToObject<RegistrationCustomQuestionForMeeting[]>() ?? Array.Empty<RegistrationCustomQuestionForMeeting>()
			};
			return registrationQuestions;
		}

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="requiredFields">List of fields that must be answer when registering for the meeting.</param>
		/// <param name="optionalFields">List of fields that can be answer when registering for the meeting.</param>
		/// <param name="customQuestions">Additional questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
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

		/// <summary>
		/// Get the meeting invite note that was sent for a specific meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The invite note.
		/// </returns>
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
		public Task<string> CreateTemplateFromExistingMeeting(string userId, long meetingId, string templateName, bool saveRecurrence = false, bool overwrite = false, CancellationToken cancellationToken = default)
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
