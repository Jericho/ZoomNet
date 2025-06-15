using Pathoschild.Http.Client;
using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

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
		public Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

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
