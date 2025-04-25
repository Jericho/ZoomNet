using Pathoschild.Http.Client;
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
	public class Rooms : IRooms
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Rooms" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Rooms(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Room>> GetAllAsync(string parentLocationId = null, RoomLocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms")
				.WithArgument("parent_location_id", parentLocationId)
				.WithArgument("type", type?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Room>("locations");
		}

		/// <inheritdoc/>
		public Task<Room> CreateAsync(string name, RoomType type, string parenLocationtId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "type", type.ToEnumString() },
				{ "parent_location_id", parenLocationtId }
			};

			return _client
				.PostAsync("rooms")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Room>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/{roomId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<RoomLocation>> GetAllLocationsAsync(string parentLocationId = null, RoomLocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms/locations")
				.WithArgument("parent_location_id", parentLocationId)
				.WithArgument("type", type?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<RoomLocation>("locations");
		}

		/// <inheritdoc/>
		public Task<RoomLocationType[]> GetLocationStructureAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync("rooms/locations/structure")
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomLocationType[]>("structures");
		}

		/// <inheritdoc/>
		public Task UpdateLocationStructureAsync(IEnumerable<RoomLocationType> structure, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "structures", structure.ToArray() }
			};

			return _client
				.PutAsync("rooms/locations/structure")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<RoomLocation> CreateLocationAsync(string name, string parentId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "parent_location_id", parentId }
			};

			return _client
				.PostAsync("rooms/locations")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomLocation>();
		}

		/// <inheritdoc/>
		public Task DeleteLocationAsync(string locationId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/locations/{locationId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task MoveLocationASync(string locationId, string parentId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "parent_location_id", parentId }
			};

			return _client
				.PutAsync($"rooms/locations/{locationId}/location")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<(RoomLocationAlertSettings AlertSettings, RoomLocationNotificationSettings NotificationSettings)> GetLocationAlertSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Alert.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var alertSettings = response.GetProperty("client_alert", true)?.ToObject<RoomLocationAlertSettings>();
			var notificationSettings = response.GetProperty("notification", true)?.ToObject<RoomLocationNotificationSettings>();

			return (alertSettings, notificationSettings);
		}

		/// <inheritdoc/>
		public async Task<(RoomLocationSecuritySettings SecuritySettings, RoomLocationSettings RoomSettings)> GetLocationSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Meeting.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var securitySettings = response.GetProperty("meeting_security", true)?.ToObject<RoomLocationSecuritySettings>();
			var roomSettings = response.GetProperty("zoom_rooms", true)?.ToObject<RoomLocationSettings>();

			return (securitySettings, roomSettings);
		}

		/// <inheritdoc/>
		public Task<RoomLocationSignageSettings> GetLocationSignageSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Signage.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomLocationSignageSettings>("digital_signage");
		}

		/// <inheritdoc/>
		public Task<RoomLocationSchedulingDisplaySettings> GetLocationSchedulingDisplaySettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.SchedulingDisplay.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomLocationSchedulingDisplaySettings>("scheduling_display");
		}
	}
}
