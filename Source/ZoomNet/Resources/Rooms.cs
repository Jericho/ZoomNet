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
		public Task<Room> CreateAsync(string name, RoomType type, string locationtId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "type", type.ToEnumString() },
				{ "location_id", locationtId }
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

		/// <inheritdoc/>
		public async Task<(RoomLocationBasicProfile Basic, RoomLocationSetupProfile Setup)> GetLocationProfileAsync(string locationId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/locations/{locationId}")
				.WithArgument("setting_type", RoomLocationSettingsType.Meeting.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var basicProfile = response.GetProperty("basic", true)?.ToObject<RoomLocationBasicProfile>();
			var setupProfiule = response.GetProperty("setup", true)?.ToObject<RoomLocationSetupProfile>();

			return (basicProfile, setupProfiule);
		}

		/// <inheritdoc/>
		public Task UpdateLocationProfileAsync(string locationId, string address = null, string description = null, string name = null, bool? codeIsRequiredToExit = null, string passcode = null, string supportEmail = null, string supportPhone = null, TimeZones? timezone = null, bool? applyBackgroundImageToAllDisplays = null, IEnumerable<RoomLocationBackgroundImageInfo> backgroundImageInfos = null, CancellationToken cancellationToken = default)
		{
			var basicProfile = new JsonObject
			{
				{ "address", address },
				{ "description", description },
				{ "name", name },
				{ "required_code_to_ext", codeIsRequiredToExit },
				{ "room_passcode", passcode },
				{ "support_email", supportEmail },
				{ "support_phone", supportPhone },
				{ "timezone", timezone?.ToEnumString() }
			};

			var setupProfile = new JsonObject
			{
				{ "apply_background_image_to_all_displays", applyBackgroundImageToAllDisplays },
				{ "background_image_info", backgroundImageInfos?.ToArray() },
			};

			var data = new JsonObject
			{
				{ "basic", basicProfile },
				{ "setup", setupProfile }
			};

			return _client
				.PatchAsync($"rooms/locations/{locationId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<string> CreateTagAsync(string name, string description, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description }
			};

			return _client
				.PostAsync("rooms/tags")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}

		/// <inheritdoc/>
		public Task UpdateTagAsync(string tagId, string name = null, string description = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description }
			};

			return _client
				.PatchAsync($"rooms/tags/{tagId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task AssignTagsToRoom(string roomId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "tag_ids", tagIds.ToArray() }
			};

			return _client
				.PatchAsync($"rooms/{roomId}/tags")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task AssignTagsToRoomsInLocation(string locationId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "tag_ids", tagIds.ToArray() }
			};

			return _client
				.PatchAsync($"rooms/locations/{locationId}/tags")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UnAssignTagFromRoom(string roomId, string tagId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/{roomId}/tags")
				.WithArgument("tag_ids", tagId) // The name of the parameter is "tag_ids" (plural) but the value is a single tagId. Documentation sauys: "Currently, only one Tag ID per request is allowed."
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<RoomTag>> GetAllTagsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("rooms/tags")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<RoomTag>("tags");
		}

		/// <inheritdoc/>
		public Task DeleteTagAsync(string tagId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/tags/{tagId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
