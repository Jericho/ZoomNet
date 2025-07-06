using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

		#endregion

		#region EVENT ACCES

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
				.PostAsync($"zoom_events/events")
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
				.PostAsync($"zoom_events/events")
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
				.PostAsync($"zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RecurringEvent>();
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
			//IEnumerable<EventSessionSpeaker> speakers = null,
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
				//{ "session_speakers", speakers?.ToArray() }, // Not implemented yet
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

		#endregion

		#region SPEAKERS

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
		public Task<EventTicketType[]> GetAllTicketTypesAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/ticket_types")
				.WithCancellationToken(cancellationToken)
				.AsObject<EventTicketType[]>("ticket_types");
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
				{ "free", !price.HasValue },
				{ "price", price?.ToString() }, // The Zoom API requires a string even though the value is numerical
				{ "quantity", quantity },
				{ "description", description },
				{ "sold_quantity", quantitySold },
				{ "sessions", sessionIds?.ToArray() },
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/ticket_types/{ticketTypeId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region TICKETS

		/// <inheritdoc/>
		public Task<EventTicket[]> CreateTicketsAsync(string eventId, IEnumerable<EventTicket> tickets, string source = null, CancellationToken cancellationToken = default)
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
							{ "custom_questions", t.CustomQuestions?.Select(q => new JsonObject { { "title", q.Key }, { "answer", q.Value } }).ToArray() }
						}).ToArray()
				}
			};

			return _client
				.PostAsync($"zoom_events/events/{eventId}/tickets")
				.WithArgument("validation_level", "standard")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<EventTicket[]>("tickets");
		}

		#endregion

		#region VIDEO_ON_DEMAND

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
