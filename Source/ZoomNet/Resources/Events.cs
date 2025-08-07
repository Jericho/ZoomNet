using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Events : IEvents
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Events" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Events(IClient client)
		{
			_client = client;
		}

		#region ATTENDEE ACTIONS

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AttendeeAction>> GetAllAttendeeActionsAsync(string eventId, string attendeeEmailAddress, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/attendee_action")
				.WithArgument("email", attendeeEmailAddress)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<AttendeeAction>("attendees");
		}

		/// <inheritdoc/>
		public async Task<(string Email, string ErrorMessage)[]> CheckInAttendeesAsync(string eventId, IEnumerable<string> attendeeEmailAddresses, string source, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "attendees", attendeeEmailAddresses?.Select(e => new JsonObject { { "email", e }, { "action", "check-in" } }).ToArray() },
				{ "source", source }
			};

			var response = await _client
				.PatchAsync($"zoom_events/events/{eventId}/attendee_action")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson("errors");

			return response
				.EnumerateArray()
				.Select(a => (a.GetPropertyValue<string>("email"), a.GetPropertyValue<string>("error_message")))
				.ToArray();
		}

		/// <inheritdoc/>
		public async Task<(string Email, string ErrorMessage)[]> CheckInAttendeesAsync(string eventId, string sessionId, IEnumerable<string> attendeeEmailAddresses, string source, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "attendees", attendeeEmailAddresses?.Select(e => new JsonObject { { "email", e }, { "action", "check-in" } }).ToArray() },
				{ "source", source }
			};

			var response = await _client
				.PatchAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/attendee_action")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson("errors");

			return response
				.EnumerateArray()
				.Select(a => (a.GetPropertyValue<string>("email"), a.GetPropertyValue<string>("error_message")))
				.ToArray();
		}

		#endregion

		#region COEDITORS

		/// <inheritdoc/>
		public Task<EventCoEditor[]> GetAllCoEditorsAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/coeditors")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventCoEditor[]>("coeditors");
		}

		/// <inheritdoc/>
		public Task AddCoEditorsAsync(string eventId, IEnumerable<(string EmailAddress, EventCoEditorPermissionGroup[] Permissions)> coeditors, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "operation", "add" },
				{
					"coeditors",
					coeditors?.Select(c => new JsonObject
					{
						{ "email", c.EmailAddress },
						{ "permission_groups", c.Permissions?.Select(p => p.ToEnumString()).ToArray() }
					}).ToArray()
				}
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/coeditors")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateCoEditorsAsync(string eventId, IEnumerable<(string EmailAddress, IEnumerable<EventCoEditorPermissionGroup> Permissions)> coeditors, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "operation", "update" },
				{
					"coeditors",
					coeditors?.Select(c => new JsonObject
					{
						{ "email", c.EmailAddress },
						{ "permission_groups", c.Permissions?.Select(p => p.ToEnumString()).ToArray() }
					}).ToArray()
				}
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/coeditors")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteCoEditorsAsync(string eventId, IEnumerable<string> emailAddresses, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "operation", "delete" },
				{
					"coeditors",
					emailAddresses?.Select(emailAddress => new JsonObject
					{
						{ "email", emailAddress },
					}).ToArray()
				}
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/coeditors")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region EVENT ACCES

		/// <inheritdoc/>
		public Task<EventAccessLink> CreateEventAccessLinkAsync(string eventId, string name, EventAccessLinkType type = EventAccessLinkType.Registration, EventAuthenticationMethod authenticationMethod = EventAuthenticationMethod.ZoomAccount, bool isDefault = false, IEnumerable<string> allowDomainList = null, IEnumerable<string> emailRestrictList = null, bool emailAuthentication = true, bool securityCodeVerification = true, string ticketTypeId = null, RecurringEventRegistrationType recurringRegistrationType = RecurringEventRegistrationType.AllSessions, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"zoom_events/events/{eventId}/access_links")
				.WithJsonBody(new JsonObject
				{
					{ "name", name },
					{ "type", type.ToEnumString() },
					{ "authentication_method", authenticationMethod.ToEnumString() },
					{ "is_default", isDefault },
					{ "allow_domain_list", allowDomainList?.ToArray() },
					{ "email_restrict_list", emailRestrictList?.ToArray() },
					{
						"security_at_join", new JsonObject
						{
							{ "email_authentication", emailAuthentication },
							{ "security_code_verification", securityCodeVerification }
						}
					},
					{ "ticket_type_id", ticketTypeId },
					{ "recurring_registration_option", recurringRegistrationType.ToEnumString() }
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<EventAccessLink>();
		}

		/// <inheritdoc/>
		public Task DeleteEventAccessLinkAsync(string eventId, string accessLinkId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/access_links/{accessLinkId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventAccessLink> GetEventAccessLinkAsync(string eventId, string accessLinkId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/access_links/{accessLinkId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventAccessLink>();
		}

		/// <inheritdoc/>
		public Task<EventAccessLink[]> GetAllEventAccessLinksAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/access_links")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventAccessLink[]>("access_links");
		}

		/// <inheritdoc/>
		public Task UpdateEventAccessLinkAsync(string eventId, string accessLinkId, string name = null, bool? isDefault = null, EventAuthenticationMethod? authenticationMethod = null, IEnumerable<string> allowDomainList = null, IEnumerable<string> emailRestrictList = null, bool? emailAuthentication = null, bool? securityCodeVerification = null, string ticketTypeId = null, RecurringEventRegistrationType? recurringRegistrationType = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "is_default", isDefault },
				{ "authentication_method", authenticationMethod?.ToEnumString() },
				{ "allow_domain_list", allowDomainList?.ToArray() },
				{ "email_restrict_list", emailRestrictList?.ToArray() },
				{ "ticket_type_id", ticketTypeId },
				{ "recurring_registration_option", recurringRegistrationType?.ToEnumString() }
			};

			if (emailAuthentication.HasValue || securityCodeVerification.HasValue)
			{
				data["security_at_join"] = new JsonObject
				{
					{ "email_authentication", emailAuthentication },
					{ "security_code_verification", securityCodeVerification }
				};
			}

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/access_links/{accessLinkId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region EVENTS

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Event>> GetAllAsync(UserRoleType role = UserRoleType.Host, EventListStatus status = EventListStatus.Upcoming, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("zoom_events/events")
				.WithArgument("role_type", role.ToEnumString())
				.WithArgument("event_status_type", status.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Event>("events");
		}

		/// <inheritdoc/>
		public Task<Event> GetAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Event>();
		}

		/// <inheritdoc/>
		public Task<SimpleEvent> CreateSimpleEventAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Simple.ToEnumString() },
				{ "access_level", isRestricted ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED" },
				{
					"calendar", new[]
					{
						new JsonObject
						{
							// It's important to convert these two dates to UTC otherwise Zoom will reject them
							// with the following unhelpful message: "Calender must contains start_time and end_time".
							{ "start_time", start.ToZoomFormat(TimeZones.UTC) },
							{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
						}
					}
				},
				{ "meeting_type", meetingType.ToEnumString() },
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },
				{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PostAsync("zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<SimpleEvent>();
		}

		/// <inheritdoc/>
		public Task<Conference> CreateConferenceAsync(string name, string description, IEnumerable<(DateTime Start, DateTime End)> calendar, TimeZones timeZone, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Conference.ToEnumString() },
				{ "access_level", isRestricted ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED" },
				{
					"calendar", calendar?.Select(c => new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", c.Start.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", c.End.ToZoomFormat(TimeZones.UTC) },
					}).ToArray()
				},
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },
				{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PostAsync("zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Conference>();
		}

		/// <inheritdoc/>
		public Task<RecurringEvent> CreateRecurringEventAsync(string name, string description, DateTime start, DateTime end, EventRecurrenceInfo recurrence, TimeZones timeZone, string hubId, bool isRestricted = false, EventAttendanceType attendanceType = EventAttendanceType.Virtual, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Reccuring.ToEnumString() },
				{ "access_level", isRestricted ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED" },
				{
					"calendar", new[]
					{
						new JsonObject
						{
							// It's important to convert these two dates to UTC otherwise Zoom will reject them
							// with the following unhelpful message: "Calender must contains start_time and end_time".
							{ "start_time", start.ToZoomFormat(TimeZones.UTC) },
							{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
						}
					}
				},
				{ "recurrence", recurrence },
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				//{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },	// Commented out for the time being.
				//{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },		// For more details, see: https://devforum.zoom.us/t/event-outside-lobby-time-range/134883
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PostAsync("zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringEvent>();
		}

		/// <inheritdoc/>
		public Task UpdateSimpleEventAsync(string eventId, string name = null, string description = null, DateTime? start = null, DateTime? end = null, TimeZones? timeZone = null, EventMeetingType? meetingType = null, string hubId = null, bool? isRestricted = null, EventAttendanceType? attendanceType = null, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			IEnumerable<(DateTime? Start, DateTime? End)> calendar = null;
			if (start.HasValue || end.HasValue)
			{
				calendar = new[] { (Start: start, End: end) };
			}

			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Simple.ToEnumString() },
				{ "access_level", isRestricted.HasValue ? (isRestricted.Value ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED") : null },
				{
					"calendar", calendar?.Select(c => new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", c.Start?.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", c.End?.ToZoomFormat(TimeZones.UTC) },
					}).ToArray()
				},
				{ "meeting_type", meetingType?.ToEnumString() },
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType?.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },
				{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateConferenceAsync(string eventId, string name = null, string description = null, IEnumerable<(DateTime? Start, DateTime? End)> calendar = null, TimeZones? timeZone = null, string hubId = null, bool? isRestricted = null, EventAttendanceType? attendanceType = null, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Conference.ToEnumString() },
				{ "access_level", isRestricted.HasValue ? (isRestricted.Value ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED") : null },
				{
					"calendar", calendar?.Select(c => new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", c.Start?.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", c.End?.ToZoomFormat(TimeZones.UTC) },
					}).ToArray()
				},
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType?.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },
				{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateRecurringEventAsync(string eventId, string name = null, string description = null, DateTime? start = null, DateTime? end = null, EventRecurrenceInfo recurrence = null, TimeZones? timeZone = null, string hubId = null, bool? isRestricted = null, EventAttendanceType? attendanceType = null, IEnumerable<EventCategory> categories = null, IEnumerable<string> tags = null, string contactName = null, DateTime? lobbyStart = null, DateTime? lobbyEnd = null, IEnumerable<Country> blockedCountries = null, string tagLine = null, CancellationToken cancellationToken = default)
		{
			IEnumerable<(DateTime? Start, DateTime? End)> calendar = null;
			if (start.HasValue || end.HasValue)
			{
				calendar = new[] { (Start: start, End: end) };
			}

			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Reccuring.ToEnumString() },
				{ "access_level", isRestricted.HasValue ? (isRestricted.Value ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED") : null },
				{
					"calendar", calendar?.Select(c => new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", c.Start?.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", c.End?.ToZoomFormat(TimeZones.UTC) },
					}).ToArray()
				},
				{ "recurrence", recurrence },
				{ "hub_id", hubId },
				{ "attendance_type", attendanceType?.ToEnumString() },
				{ "categories", categories?.Select(c => c.ToEnumString()).ToArray() },
				{ "tags", tags?.ToArray() },
				{ "contact_name", contactName },
				//{ "lobby_start_time", lobbyStart?.ToZoomFormat(TimeZones.UTC) },	// Commented out for the time being.
				//{ "lobby_end_time", lobbyEnd?.ToZoomFormat(TimeZones.UTC) },		// For more details, see: https://devforum.zoom.us/t/event-outside-lobby-time-range/134883
				{ "blocked_countries", blockedCountries?.Select(bc => bc.ToEnumString()).ToArray() },
				{ "tagline", tagLine },
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task PublishEventAsync(string eventId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				 // Documentation says the operation name is "Publish", but the API rejects it with the following message: "Invalid Event operation".
				 // API accepts "publish", all lower case.
				{ "operation", "publish" },
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/event_actions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task CancelEventAsync(string eventId, string cancellationMessage, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				 // Documentation says the operation name is "Cancel", but the API rejects it with the following message: "Invalid Event operation".
				 // API accepts "cancel", all lower case.
				{ "operation", "cancel" },
				{ "cancel_message", cancellationMessage }
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/event_actions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteEventAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<string> DuplicateEventAsync(string eventId, string name, DateTime start, TimeZones timeZone, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"zoom_events/events/{eventId}/event_actions")
				.WithJsonBody(new JsonObject
				{
					{ "operation", "duplicate" },
					{
						"duplicate_event", new JsonObject
						{
							{ "name", name },
							{ "start_time", start.ToZoomFormat(timeZone) },
							{ "timezone", timeZone }
						}
					}
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("event_id");
		}

		#endregion

		#region EXHIBITORS

		/// <inheritdoc/>
		public Task<EventExhibitor> CreateExhibitorAsync(string eventId, string name, string contactFullName, string contactEmail, bool isSponsor, string sponsorTierId, string description, IEnumerable<string> sessionIds, string website, string privacyPolicyUrl, string linkedInUrl, string twitterUrl, string youtubeUrl, string instagramUrl, string facebookUrl, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "contact_name", contactFullName },
				{ "contact_email", contactEmail },
				{ "is_sponsor", isSponsor },
				{ "tier_id", sponsorTierId },
				{ "description", description },
				{ "associated_sessions", sessionIds?.ToArray() },
				{ "website", website },
				{ "privacy_policy", privacyPolicyUrl },
				{ "linkedin_url", linkedInUrl },
				{ "twitter_url", twitterUrl },
				{ "youtube_url", youtubeUrl },
				{ "instagram_url", instagramUrl },
				{ "facebook_url", facebookUrl }
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/exhibitors")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<EventExhibitor>();
		}

		/// <inheritdoc/>
		public Task DeleteExhibitorAsync(string eventId, string exhibitorId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/exhibitors/{exhibitorId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventExhibitor> GetExhibitorAsync(string eventId, string exhibitorId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/exhibitors/{exhibitorId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventExhibitor>();
		}

		/// <inheritdoc/>
		public Task<EventExhibitor[]> GetAllExhibitorsAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/exhibitors")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventExhibitor[]>("exhibitors");
		}

		/// <inheritdoc/>
		public Task<SponsorTier[]> GetAllSponsorTiersAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sponsor_tiers")
				.WithCancellationToken(cancellationToken)
				.AsObject<SponsorTier[]>("sponsor_tiers");
		}

		/// <inheritdoc/>
		public Task UpdateExhibitorAsync(string eventId, string exhibitorId, string name = null, string contactFullName = null, string contactEmail = null, bool? isSponsor = null, string sponsorTierId = null, string description = null, IEnumerable<string> sessionIds = null, string website = null, string privacyPolicyUrl = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, string instagramUrl = null, string facebookUrl = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "contact_name", contactFullName },
				{ "contact_email", contactEmail },
				{ "is_sponsor", isSponsor },
				{ "tier_id", sponsorTierId },
				{ "description", description },
				{ "associated_sessions", sessionIds?.ToArray() },
				{ "website", website },
				{ "privacy_policy", privacyPolicyUrl },
				{ "linkedin_url", linkedInUrl },
				{ "twitter_url", twitterUrl },
				{ "youtube_url", youtubeUrl },
				{ "instagram_url", instagramUrl },
				{ "facebook_url", facebookUrl }
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/exhibitors/{exhibitorId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
		#endregion

		#region HUBS

		/// <inheritdoc/>
		public Task<string> CreateHubHostAsync(string hubId, string emailAddress, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "email", emailAddress },
			};

			return _client
				.PostAsync($"zoom_events/hubs/{hubId}/hosts")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("host_user_id");
		}

		/// <inheritdoc/>
		public Task<Hub[]> GetAllHubsAsync(UserRoleType userRole = UserRoleType.Host, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("zoom_events/hubs")
				.WithArgument("role_type", userRole.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<Hub[]>("hubs");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<HubHost>> GetAllHubHostsAsync(string hubId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"zoom_events/hubs/{hubId}/hosts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<HubHost>("hosts");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<HubVideo>> GetAllHubVideosAsync(string hubId, string folderId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"zoom_events/hubs/{hubId}/videos")
				.WithArgument("folder_id", folderId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<HubVideo>("videos");
		}

		/// <inheritdoc/>
		public Task RemoveHostFromHubAsync(string hubId, string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/hubs/{hubId}/hosts/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region REGISTRANTS

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<EventRegistrant>> GetAllRegistrantsAsync(string eventId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"zoom_events/events/{eventId}/registrants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<EventRegistrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<EventSessionAttendee>> GetAllSessionAttendeesAsync(string eventId, string sessionId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/attendees")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<EventSessionAttendee>("attendees");
		}

		#endregion

		#region REPORTS

		#endregion

		#region SESSIONS

		/// <inheritdoc/>
		public Task<EventSession> CreateSessionAsync(
			string eventId,
			string name,
			DateTime start,
			DateTime end,
			TimeZones timeZone,
			string description = null,
			EventSessionType type = EventSessionType.Meeting,
			IEnumerable<(string Id, bool CanEditSession, bool IsDisplayedInSessionDetails, bool CanActAsAlternativeHost)> speakers = null,
			bool isFeatured = false,
			bool isVisibleInLandingPage = true,
			bool isFeaturedInLobby = false,
			bool isVisibleInLobby = true,
			bool isSimulive = false,
			string recordingFileId = null,
			bool isChatInLobbyEnabled = false,
			bool isLedBySponsor = false,
			IEnumerable<string> trackLabels = null,
			IEnumerable<string> audienceLabels = null,
			IEnumerable<string> productLabels = null,
			IEnumerable<string> levels = null,
			IEnumerable<string> alternativeHosts = null,
			IEnumerable<string> panelists = null,
			EventAttendanceType attendanceType = EventAttendanceType.Virtual,
			string physicalLocation = null,
			bool allowReservations = false,
			int maxCapacity = 0,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "start_time", start.ToZoomFormat(TimeZones.UTC) }, // For some reason the Zoom API requires the start and end dates to be in UTC format and returns an error message if you format the dates in any other time zone
				{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
				{ "timezone", timeZone.ToEnumString() },
				{ "type", (int)type },
				{
					"session_speakers",
					speakers?
						.Select(s => new JsonObject
						{
							{ "speaker_id", s.Id },
							{ "access_to_edit_session", s.CanEditSession },
							{ "show_in_session_detail", s.IsDisplayedInSessionDetails },
							{ "has_alternative_host_permission", s.CanActAsAlternativeHost }
						})
						.ToArray()
				},
				{ "featured", isFeatured },
				{ "visible_in_landing_page", isVisibleInLandingPage },
				{ "featured_in_lobby", isFeaturedInLobby },
				{ "visible_in_lobby", isVisibleInLobby },
				{ "is_simulive", isSimulive },
				{ "record_file_id", recordingFileId },
				{ "chat_channel", isChatInLobbyEnabled },
				{ "led_by_sponsor", isLedBySponsor },
				{ "track_labels", trackLabels?.ToArray() },
				{ "audience_labels", audienceLabels?.ToArray() },
				{ "product_labels", productLabels?.ToArray() },
				{ "levels", levels?.ToArray() },
				{ "alternative_hosts", alternativeHosts?.ToArray() },
				{ "panelists", panelists?.ToArray() },
				{ "attendance_type", attendanceType.ToEnumString() },
				{ "physical_location", physicalLocation },
				{
					"session_reservation", new JsonObject
					{
						{ "max_capacity", maxCapacity },
						{ "allow_reservations", allowReservations }
					}
				}
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/sessions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSession>();
		}

		/// <inheritdoc/>
		public Task UpsertSessionInterpretersAsync(string eventId, string sessionId, IEnumerable<(string EmailAddress, InterpretationLanguageForEventSession SourceLanguage, InterpretationLanguageForEventSession TargetLanguage)> languageInterpreters, IEnumerable<(string EmailAddress, InterpretationSignLanguage TargetLanguage)> signLanguageInterpreters, CancellationToken cancellationToken = default)
		{
			var languageInterpretersAsJson = languageInterpreters?.Select(i =>
				new JsonObject
				{
					{ "email", i.EmailAddress },
					{ "type", (int)InterpreterType.Language },
					{ "source_language_id", i.SourceLanguage.ToEnumString() },
					{ "target_language_id", i.TargetLanguage.ToEnumString() },
				});

			var signLanguageInterpretersAsJson = signLanguageInterpreters?.Select(i =>
				new JsonObject
				{
					{ "email", i.EmailAddress },
					{ "type", (int)InterpreterType.Sign },
					{ "target_language_id", i.TargetLanguage.ToEnumString() },
				});

			var data = new JsonObject
			{
				{ "interpreters", languageInterpretersAsJson.Union(signLanguageInterpretersAsJson).ToArray() }
			};

			return _client
				.PutAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/interpreters")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpsertSessionPollsAsync(string eventId, string sessionId, IEnumerable<PollForEventSession> polls, CancellationToken cancellationToken = default)
		{
			foreach (var poll in polls)
			{
				// There are a number of scenarios where questions are deemed to be "advanced". When a poll contains advanced questions, it's "poll_type" cannot be "Basic".
				// See: https://devforum.zoom.us/t/unable-to-create-poll-for-an-event-session-the-poll-contains-advanced-poll-questions/135561

				// So far, the scenarios I have been able to onfirm to cause a question to be considered "advanced" are:
				// - The question is required (IsRequired == true)
				// - The question is a dropdown (ShowAsDropdown == true)
				// - The type of question is anything other than single choice (Type != PollQuestionType.SingleChoice)
				var pollContainsAdvancedQuestions = poll.Questions?.Any(q =>
					q.IsRequired ||
					q.ShowAsDropdown.GetValueOrDefault(false) ||
					q.Type != PollQuestionType.SingleChoice) ?? false;
				var isBasicPoll = poll.Type == PollType.Basic;
				if (pollContainsAdvancedQuestions && isBasicPoll) poll.Type = PollType.Advanced;

				// If the max and/or min number of characters are not configured, the API will return the following error message:
				// Short/Long answers can not exceed character length.

				// Make sure the min and max character limits are within the allowed range for short answers
				foreach (var item in poll.Questions.Where(q => q.Type == PollQuestionType.Short))
				{
					item.MinimumNumberOfCharacters = Math.Max(item.MinimumNumberOfCharacters ?? 1, 1);
					item.MaximumNumberOfCharacters = Math.Min(item.MaximumNumberOfCharacters ?? 500, 500);
				}

				// Make sure the min and max character limits are within the allowed range for long answers
				foreach (var item in poll.Questions.Where(q => q.Type == PollQuestionType.Long))
				{
					item.MinimumNumberOfCharacters = Math.Max(item.MinimumNumberOfCharacters ?? 1, 1);
					item.MaximumNumberOfCharacters = Math.Min(item.MaximumNumberOfCharacters ?? 2000, 2000);
				}
			}

			var data = new JsonObject
			{
				{
					"polls",
					polls?.Select(p => new JsonObject
					{
						{ "title", p.Title },
						{ "poll_type", (int)p.Type },
						{ "status", p.Status.ToEnumString() },
						{ "anonymous", p.AllowAnonymous },
						{
							"questions",
							p.Questions?.Select(q => new JsonObject
							{
								{ "answer_max_character", q.MaximumNumberOfCharacters },
								{ "answer_min_character", q.MinimumNumberOfCharacters },
								{ "answer_required", p.Type == PollType.Basic ? null : q.IsRequired },
								{ "answers", q.Answers?.ToArray() },
								{ "case_sensitive", q.IsCaseSensitive },
								{ "name", q.Question },
								{ "prompts", q.Prompts?.ToArray() },
								{ "prompt_right_answers", q.PromptCorrectAnswers?.ToArray() },
								{ "rating_max_label", q.RatingHighScoreLabel },
								{ "rating_max_value", q.RatingMaximumValue },
								{ "rating_min_label", q.RatingLowScoreLabel },
								{ "rating_min_value", q.RatingMinimumValue },
								{ "right_answers", q.CorrectAnswers?.ToArray() },
								{ "show_as_dropdown", q.ShowAsDropdown },
								{ "type", q.Type.ToEnumString() },
								{ "title", q.Title }
							}).ToArray()
						}
					}).ToArray()
				}
			};

			return _client
				.PutAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/polls")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteSessionAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
					.DeleteAsync($"zoom_events/events/{eventId}/sessions/{sessionId}")
					.WithCancellationToken(cancellationToken)
					.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateSessionAsync(
			string eventId,
			string sessionId,
			string name = null,
			DateTime? start = null,
			DateTime? end = null,
			TimeZones? timeZone = null,
			string description = null,
			EventSessionType? type = null,
			IEnumerable<(string Id, bool CanEditSession, bool IsDisplayedInSessionDetails, bool CanActAsAlternativeHost)> speakers = null,
			bool? isFeatured = null,
			bool? isVisibleInLandingPage = null,
			bool? isFeaturedInLobby = null,
			bool? isVisibleInLobby = null,
			bool? isSimulive = null,
			string recordingFileId = null,
			bool? isChatInLobbyEnabled = null,
			bool? isLedBySponsor = null,
			IEnumerable<string> trackLabels = null,
			IEnumerable<string> audienceLabels = null,
			IEnumerable<string> productLabels = null,
			IEnumerable<string> levels = null,
			IEnumerable<string> alternativeHosts = null,
			IEnumerable<string> panelists = null,
			EventAttendanceType? attendanceType = null,
			string physicalLocation = null,
			bool? allowReservations = null,
			int? maxCapacity = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "start_time", start?.ToZoomFormat(TimeZones.UTC) }, // For some reason the Zoom API requires the start and end dates to be in UTC format and returns an error message if you format the dates in any other time zone
				{ "end_time", end?.ToZoomFormat(TimeZones.UTC) },
				{ "timezone", timeZone?.ToEnumString() },
				{ "type", (int?)type },
				{
					"session_speakers",
					speakers?
						.Select(s => new JsonObject
						{
							{ "speaker_id", s.Id },
							{ "access_to_edit_session", s.CanEditSession },
							{ "show_in_session_detail", s.IsDisplayedInSessionDetails },
							{ "has_alternative_host_permission", s.CanActAsAlternativeHost }
						})
						.ToArray()
				},
				{ "featured", isFeatured },
				{ "visible_in_landing_page", isVisibleInLandingPage },
				{ "featured_in_lobby", isFeaturedInLobby },
				{ "visible_in_lobby", isVisibleInLobby },
				{ "is_simulive", isSimulive },
				{ "record_file_id", recordingFileId },
				{ "chat_channel", isChatInLobbyEnabled },
				{ "led_by_sponsor", isLedBySponsor },
				{ "track_labels", trackLabels?.ToArray() },
				{ "audience_labels", audienceLabels?.ToArray() },
				{ "product_labels", productLabels?.ToArray() },
				{ "levels", levels?.ToArray() },
				{ "alternative_hosts", alternativeHosts?.ToArray() },
				{ "panelists", panelists?.ToArray() },
				{ "attendance_type", attendanceType?.ToEnumString() },
				{ "physical_location", physicalLocation },
			};

			if (maxCapacity.HasValue || allowReservations.HasValue)
			{
				data["session_reservation"] = new JsonObject
				{
					{ "max_capacity", maxCapacity },
					{ "allow_reservations", allowReservations }
				};
			}

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/sessions/{sessionId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventSession> GetSessionAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSession>();
		}

		/// <inheritdoc/>
		public Task<EventSession[]> GetAllSessionsAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSession[]>("sessions");
		}

		/// <inheritdoc/>
		public Task AddSessionReservationAsync(string eventId, string sessionId, string emailAddress, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/reservations")
				.WithJsonBody(new JsonObject { { "email", emailAddress } })
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteSessionReservationAsync(string eventId, string sessionId, string emailAddress, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/reservations")
				.WithJsonBody(new JsonObject { { "email", emailAddress } })
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Interpreter[]> GetAllSessionInterpretersAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
					.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/interpreters")
					.WithCancellationToken(cancellationToken)
					.AsObject<Interpreter[]>("interpreters");
		}

		/// <inheritdoc/>
		public Task<PollForEventSession[]> GetAllSessionPollsAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollForEventSession[]>("polls");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<EventSessionReservation>> GetAllSessionReservationsAsync(string eventId, string sessionId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/reservations")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<EventSessionReservation>("reservations");
		}

		/// <inheritdoc/>
		public Task UpdateSessionLivestreamConfigurationAsync(string eventId, string sessionId, bool incomingEnabled, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "incoming_enabled", incomingEnabled },
			};

			if (incomingEnabled)
			{
				data["incoming_config"] = new JsonObject
				{
					// Generated key, specific to a session.
					// If a generated key already exist for this sessionId, then it is reused.
					// This is the only type supported as of now and this is a mandatory field when the incoming_enabled field is set to true.
					{ "stream_key_type", "GENERATED" },
				};
			}

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/livestream")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventSessionLivestreamConfiguration> GetSessionLivestreamConfgurationAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/livestream")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSessionLivestreamConfiguration>("incoming_config", false);
		}

		/// <inheritdoc/>
		public Task<string> GetSessionJoinTokenAsync(string eventId, string sessionId, CancellationToken cancellationToken = default)
		{
			return _client
			   .GetAsync($"zoom_events/events/{eventId}/sessions/{sessionId}/join_token")
			   .WithCancellationToken(cancellationToken)
			   .AsObject<string>("join_token");
		}

		#endregion

		#region SPEAKERS

		/// <inheritdoc/>
		public Task<EventSpeaker> CreateSpeakerAsync(string eventId, string name, string emailAddress = null, string jobTitle = null, string biography = null, string companyName = null, string companyWebsite = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, bool featuredInEventDetailPage = false, bool visibleInEventDetailPage = true, bool featuredInLobby = false, bool visibleInLobby = true, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "email", emailAddress },
				{ "job_title", jobTitle },
				{ "biography", biography },
				{ "company_name", companyName },
				{ "company_website", companyWebsite },
				{ "linkedin_url", linkedInUrl },
				{ "twitter_url", twitterUrl },
				{ "youtube_url", youtubeUrl },
				{ "featured_in_event_detail_page", featuredInEventDetailPage },
				{ "visible_in_event_detail_page", visibleInEventDetailPage },
				{ "featured_in_lobby", featuredInLobby },
				{ "visible_in_lobby", visibleInLobby },
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/speakers")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSpeaker>();
		}

		/// <inheritdoc/>
		public Task DeleteSpeakerAsync(string eventId, string speakerId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/speakers/{speakerId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventSpeaker> GetSpeakerAsync(string eventId, string speakerId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/speakers/{speakerId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSpeaker>();
		}

		/// <inheritdoc/>
		public Task<EventSpeaker[]> GetAllSpeakersAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/speakers")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventSpeaker[]>("speakers");
		}

		/// <inheritdoc/>
		public Task UpdateSpeakerAsync(string eventId, string speakerId, string name = null, string emailAddress = null, string jobTitle = null, string biography = null, string companyName = null, string companyWebsite = null, string linkedInUrl = null, string twitterUrl = null, string youtubeUrl = null, bool? featuredInEventDetailPage = null, bool? visibleInEventDetailPage = null, bool? featuredInLobby = null, bool? visibleInLobby = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "email", emailAddress },
				{ "job_title", jobTitle },
				{ "biography", biography },
				{ "company_name", companyName },
				{ "company_website", companyWebsite },
				{ "linkedin_url", linkedInUrl },
				{ "twitter_url", twitterUrl },
				{ "youtube_url", youtubeUrl },
				{ "featured_in_event_detail_page", featuredInEventDetailPage },
				{ "visible_in_event_detail_page", visibleInEventDetailPage },
				{ "featured_in_lobby", featuredInLobby },
				{ "visible_in_lobby", visibleInLobby },
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/speakers/{speakerId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region TICKET TYPES

		/// <inheritdoc/>
		public Task<string> CreateTicketTypeAsync(string eventId, string name, DateTime start, DateTime end, string currencyCode, double? price = null, int? quantity = null, string description = null, int? quantitySold = null, IEnumerable<string> sessionIds = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "start_time", start.ToZoomFormat(TimeZones.UTC) },
				{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
				{ "currency", currencyCode },
				{ "free", !price.HasValue },
				{ "price", price?.ToString() }, // The Zoom API requires a string even though the value is numerical
				{ "quantity", quantity },
				{ "description", description },
				{ "sold_quantity", quantitySold },
				{ "sessions", sessionIds?.ToArray() },
				{ "attendance_type", "in-person" } // Zoom only allows tickets for in-person events
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/ticket_types")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("ticket_type_id");
		}

		/// <inheritdoc/>
		public Task DeleteTicketTypeAsync(string eventId, string ticketTypeId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/ticket_types/{ticketTypeId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<(EventRegistrationQuestion[] StandardQuestions, EventRegistrationCustomQuestion[] CustomQuestions)> GetRegistrationQuestionsForEventAsync(string eventId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"zoom_events/events/{eventId}/questions")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var standardQuestions = Array.Empty<EventRegistrationQuestion>();
			if (response.TryGetProperty("questions", out JsonElement standardQuestionsJsonElement))
			{
				standardQuestions = standardQuestionsJsonElement.EnumerateArray()
					.Select(item => item.ToObject<EventRegistrationQuestion>())
					.ToArray();
			}

			var customQuestions = Array.Empty<EventRegistrationCustomQuestion>();
			if (response.TryGetProperty("custom_questions", out JsonElement customQuestionsJsonElement))
			{
				customQuestions = customQuestionsJsonElement.EnumerateArray()
					.Select(item => item.ToObject<EventRegistrationCustomQuestion>())
					.ToArray();
			}

			return (standardQuestions, customQuestions);
		}

		/// <inheritdoc/>
		public async Task<(EventRegistrationQuestion[] StandardQuestions, EventRegistrationCustomQuestion[] CustomQuestions)> GetRegistrationQuestionsForTicketTypeAsync(string eventId, string ticketTypeId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"zoom_events/events/{eventId}/ticket_types/{ticketTypeId}/questions")
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var standardQuestions = Array.Empty<EventRegistrationQuestion>();
			if (response.TryGetProperty("questions", out JsonElement standardQuestionsJsonElement))
			{
				standardQuestions = standardQuestionsJsonElement.EnumerateArray()
					.Select(item => item.ToObject<EventRegistrationQuestion>())
					.ToArray();
			}

			var customQuestions = Array.Empty<EventRegistrationCustomQuestion>();
			if (response.TryGetProperty("custom_questions", out JsonElement customQuestionsJsonElement))
			{
				customQuestions = customQuestionsJsonElement.EnumerateArray()
					.Select(item => item.ToObject<EventRegistrationCustomQuestion>())
					.ToArray();
			}

			return (standardQuestions, customQuestions);
		}

		/// <inheritdoc/>
		public Task<EventTicketType[]> GetAllTicketTypesAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/ticket_types")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventTicketType[]>("ticket_types");
		}

		/// <inheritdoc/>
		public Task UpdateRegistrationQuestionsForEventAsync(
			string eventId,
			IEnumerable<EventRegistrationQuestion> standardQuestions = null,
			IEnumerable<EventRegistrationCustomQuestion> customQuestions = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "questions", standardQuestions?.ToArray() },
				{ "custom_questions", customQuestions?.ToArray() }
			};

			return _client
				.PutAsync($"zoom_events/events/{eventId}/questions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateRegistrationQuestionsForTicketTypeAsync(
			string eventId,
			string ticketTypeId,
			IEnumerable<EventRegistrationQuestion> standardQuestions = null,
			IEnumerable<EventRegistrationCustomQuestion> customQuestions = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "questions", standardQuestions?.ToArray() },
				{ "custom_questions", customQuestions?.ToArray() }
			};

			return _client
				.PutAsync($"zoom_events/events/{eventId}/ticket_types/{ticketTypeId}/questions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateTicketTypeAsync(string eventId, string ticketTypeId, string name = null, DateTime? start = null, DateTime? end = null, string currencyCode = null, double? price = null, int? quantity = null, string description = null, int quantitySold = 0, IEnumerable<string> sessionIds = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "start_time", start?.ToZoomFormat(TimeZones.UTC) },
				{ "end_time", end?.ToZoomFormat(TimeZones.UTC) },
				{ "currency", currencyCode },
				{ "price", price?.ToString() }, // The Zoom API requires a string even though the value is numerical
				{ "quantity", quantity },
				{ "description", description },
				{ "sold_quantity", quantitySold },
				{ "sessions", sessionIds?.ToArray() },
			};
			if (price.HasValue) data.Add("free", false); // Change the 'free' property if (and only if) the price has been specified.

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/ticket_types/{ticketTypeId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region TICKETS

		/// <inheritdoc/>
		public async Task<(EventTicket[] Tickets, EventTicketError[] Errors)> CreateTicketsAsync(string eventId, IEnumerable<EventTicket> tickets, string source = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "registration_source", source },
				{
					"tickets", tickets?
						.Select(t => new JsonObject
						{
							{ "email", t.Email },
							{ "ticket_type_id", t.TypeId },
							{ "external_ticket_id", t.ExternalTicketId },
							{ "send_notification", t.SendNotifications },
							{ "fast_join", t.FastJoin },
							{ "registration_needed", t.RegistrationNeeded },
							{ "session_ids", t.SessionIds?.ToArray() },
							{ "first_name", t.FirstName },
							{ "last_name", t.LastName },
							{ "address", t.Address },
							{ "city", t.City },
							{ "state", t.State },
							{ "country", t.Country },
							{ "zip", t.Zip },
							{ "phone", t.Phone },
							{ "industry", t.Industry },
							{ "job_title", t.JobTitle },
							{ "organization", t.Organization },
							{ "comments", t.Comments },
							{
								"custom_questions", t.CustomQuestions?
									.Select(q => new JsonObject { { "title", q.Key }, { "answer", q.Value } })
									.ToArray()
							}
						}).ToArray()
				}
			};

			var response = await _client
				.PostAsync($"zoom_events/events/{eventId}/tickets")
				.WithArgument("validation_level", "standard")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var createdTickets = response.GetProperty("tickets", false)?.ToObject<EventTicket[]>() ?? Array.Empty<EventTicket>();
			var errors = response.GetProperty("errors", false)?.ToObject<EventTicketError[]>() ?? Array.Empty<EventTicketError>();

			return (createdTickets, errors);
		}

		/// <inheritdoc/>
		public Task DeleteTicketAsync(string eventId, string ticketId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/events/{eventId}/tickets/{ticketId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<EventTicket> GetTicketAsync(string eventId, string ticketId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/tickets/{ticketId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventTicket>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<EventTicket>> GetAllTicketsAsync(string eventId, string ticketTypeId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/tickets")
				.WithArgument("ticket_type_id", ticketTypeId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<EventTicket>("tickets");
		}

		/// <inheritdoc/>
		public Task UpdateTicketAsync(string eventId, string ticketId, string firstName = null, string lastName = null, string address = null, string city = null, string state = null, string zip = null, string country = null, string phone = null, string industry = null, string jobTitle = null, string organization = null, string comments = null, string externalTicketId = null, IEnumerable<KeyValuePair<string, string>> customQuestions = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "address", address },
				{ "city", city },
				{ "state", state },
				{ "zip", zip },
				{ "country", country },
				{ "phone", phone },
				{ "industry", industry },
				{ "job_title", jobTitle },
				{ "organization", organization },
				{ "comments", comments },
				{ "external_ticket_id", externalTicketId },
				{
					"custom_questions", customQuestions?
						.Select(q => new JsonObject { { "title", q.Key }, { "answer", q.Value } })
						.ToArray()
				}
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/tickets/{ticketId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region VIDEO_ON_DEMAND

		/// <inheritdoc/>
		public Task PublishVideoOnDemandChannelAsync(string hubId, string channelId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "operation", "publish" }
			};

			return _client
				.PostAsync($"zoom_events/hubs/{hubId}/vod_channels/{channelId}/actions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<VideoOnDemandChannel> CreateVideoOnDemandChannelAsync(string hubId, string name, string description, VideoOnDemandChannelType type, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "type", type.ToEnumString() }
			};

			return _client
				.PostAsync($"zoom_events/hubs/{hubId}/vod_channels")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<VideoOnDemandChannel>();
		}

		/// <inheritdoc/>
		public Task UpdateVideoOnDemandChannelAsync(string hubId, string channelId, string name = null, string description = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
			};

			return _client
				.PatchAsync($"zoom_events/hubs/{hubId}/vod_channels/{channelId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		public Task<VideoOnDemandChannel> GetVideoOnDemandChannelAsync(string hubId, string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/hubs/{hubId}/vod_channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<VideoOnDemandChannel>();
		}

		/// <inheritdoc/>
		public Task DeleteVideoOnDemandChannelAsync(string hubId, string channelId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"zoom_events/hubs/{hubId}/vod_channels/{channelId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<VideoOnDemandChannel>> GetAllVidoOnDemandChannelsAsync(string hubId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/hubs/{hubId}/vod_channels")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<VideoOnDemandChannel>("vod_channels");
		}

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
