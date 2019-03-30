using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using ZoomNet.Models;
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
	/// Allows you to manage meetings.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IMeetings" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/meetings/meetings">Zoom documentation</a> for more information.
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

		/// <summary>
		/// Retrieve all meetings of the specified type for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="type">The type of meetings. Allowed values: Scheduled, Live, Upcoming.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Meeting" />.
		/// </returns>
		public Task<PaginatedResponse<Meeting>> GetAllAsync(string userId, MeetingListType type = MeetingListType.Scheduled, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"users/{userId}/meetings")
				.WithArgument("type", JToken.Parse(JsonConvert.SerializeObject(type)).ToString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Meeting>("meetings", new MeetingConverter());
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
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		public Task<InstantMeeting> CreateInstantMeetingAsync(
			string userId,
			string topic,
			string agenda,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject()
			{
				{ "type", 1 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

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
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		public Task<ScheduledMeeting> CreateScheduledMeetingAsync(
			string userId,
			string topic,
			string agenda,
			DateTime start,
			int duration,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject()
			{
				{ "type", 2 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("start_time", start.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("timezone", "UTC");
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

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
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		public Task<RecurringMeeting> CreateRecurringMeetingAsync(
			string userId,
			string topic,
			string agenda,
			DateTime? start,
			int duration,
			RecurrenceInfo recurrence,
			string password = null,
			MeetingSettings settings = null,
			IDictionary<string, string> trackingFields = null,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject()
			{
				// 3 = Recurring with no fixed time
				// 8 = Recurring with fixed time
				{ "type", start.HasValue ? 8 : 3 }
			};
			data.AddPropertyIfValue("topic", topic);
			data.AddPropertyIfValue("password", password);
			data.AddPropertyIfValue("agenda", agenda);
			data.AddPropertyIfValue("start_time", start?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
			data.AddPropertyIfValue("duration", duration);
			data.AddPropertyIfValue("recurrence", recurrence);
			data.AddPropertyIfValue("timezone", "UTC");
			data.AddPropertyIfValue("settings", settings);
			data.AddPropertyIfValue("tracking_fields", trackingFields?.Select(tf => new JObject() { { "field", tf.Key }, { "value", tf.Value } }));

			return _client
				.PostAsync($"users/{userId}/meetings")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringMeeting>();
		}

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		public Task<Meeting> GetAsync(string userId, long meetingId, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.GetAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
				.WithCancellationToken(cancellationToken)
				.AsObject<Meeting>(null, new MeetingConverter());
		}

		/// <summary>
		/// Delete a meeting.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAsync(string userId, long meetingId, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.DeleteAsync($"meetings/{meetingId}")
				.WithArgument("occurrence_id", occurrenceId)
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
		public Task EndAsync(long meetingId, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.PutAsync($"meetings/{meetingId}/status")
				.WithArgument("action", "end")
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
		public Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long meetingId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"meetings/{meetingId}/registrants")
				.WithArgument("status", JToken.Parse(JsonConvert.SerializeObject(status)).ToString())
				.WithArgument("occurrence_id", occurrenceId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("page", page)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponse<Registrant>("registrants");
		}

		/// <summary>
		/// List registrants of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="email">A valid email address.</param>
		/// <param name="firstName">User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Registrant" />.
		/// </returns>
		public Task<Registrant> AddRegistrantsAsync(long meetingId, string email, string firstName, string lastName, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.AddPropertyIfValue("email", email);
			data.AddPropertyIfValue("first_name", firstName);
			data.AddPropertyIfValue("last_name", lastName);

			return _client
				.PostAsync($"meetings/{meetingId}/registrants")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
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
		public Task ApproveRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
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
		public Task ApproveRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.AddPropertyIfValue("action", "approve");
			data.AddPropertyIfValue("registrants", registrantsInfo.Select(ri => new { id = ri.RegistrantId, email = ri.RegistrantEmail }).ToArray());

			return _client
				.PostAsync($"meetings/{meetingId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
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
		public Task RejectRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return ApproveRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
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
		public Task RejectRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.AddPropertyIfValue("action", "deny");
			data.AddPropertyIfValue("registrants", registrantsInfo.Select(ri => new { id = ri.RegistrantId, email = ri.RegistrantEmail }).ToArray());

			return _client
				.PostAsync($"meetings/{meetingId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Cancel a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return ApproveRegistrantsAsync(meetingId, new[] { (registrantId, registrantEmail) }, occurrenceId, cancellationToken);
		}

		/// <summary>
		/// Cancel multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be cancelled.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task CancelRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.AddPropertyIfValue("action", "approve");
			data.AddPropertyIfValue("registrants", registrantsInfo.Select(ri => new { id = ri.RegistrantId, email = ri.RegistrantEmail }).ToArray());

			return _client
				.PostAsync($"meetings/{meetingId}/registrants/status")
				.WithArgument("occurence_id", occurrenceId)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
