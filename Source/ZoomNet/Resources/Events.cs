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
		public Task CheckInAttendeesAsync(string eventId, IEnumerable<string> attendeeEmailAddresses, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "attendees", attendeeEmailAddresses?.Select(e => new JsonObject { { "email", e }, { "action", "check-in" } }).ToArray() }
			};

			return _client
				.PatchAsync($"zoom_events/events/{eventId}/attendee_action")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region COEDITORS

		#endregion

		#region EVENT ACCES

		#endregion

		#region EVENTS

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("zoom_events/events")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Event>("events");
		}

		/// <inheritdoc/>
		public Task<SimpleEvent> CreateSimpleEventAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Simple.ToEnumString() },
				{ "access_level", isRestricted ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED" },
				{
					"calendar", new JsonArray(new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", start.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
					})
				},
				{ "meeting_type", meetingType.ToEnumString() },
				{ "hub_id", hubId }
			};

			return _client
				.PostAsync($"zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<SimpleEvent>();
		}

		/// <inheritdoc/>
		public Task<Conference> CreateConferenceAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "timezone", timeZone },
				{ "event_type", EventType.Conference.ToEnumString() },
				{ "access_level", isRestricted ? "PRIVATE_RESTRICTED" : "PRIVATE_UNRESTRICTED" },
				{
					"calendar", new JsonArray(new JsonObject
					{
						// It's important to convert these two dates to UTC otherwise Zoom will reject them
						// with the following unhelpful message: "Calender must contains start_time and end_time".
						{ "start_time", start.ToZoomFormat(TimeZones.UTC) },
						{ "end_time", end.ToZoomFormat(TimeZones.UTC) },
					})
				},
				{ "hub_id", hubId }
			};

			return _client
				.PostAsync($"zoom_events/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Conference>();
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
		public Task<SponsorTier[]> GetAllSponsorTiersAsync(string eventId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"zoom_events/events/{eventId}/sponsor_tiers")
				.WithCancellationToken(cancellationToken)
				.AsObject<SponsorTier[]>("sponsor_tiers");
		}

		#endregion

		#region HUBS

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

		#endregion

		#region TICKETS

		#endregion

		#region VIDEO_ON_DEMAND

		#endregion

		#region VIDEO_ON_DEMAND REGISTRATION

		#endregion
	}
}
