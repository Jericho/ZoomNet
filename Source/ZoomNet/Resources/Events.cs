using Pathoschild.Http.Client;
using System;
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

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"zoom_events/events")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Event>("events");
		}

		/// <inheritdoc/>
		public Task<SimpleEvent> CreateSimpleAsync(string name, string description, DateTime start, DateTime end, TimeZones timeZone, EventMeetingType meetingType, string hubId, bool isRestricted = false, CancellationToken cancellationToken = default)
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
	}
}
