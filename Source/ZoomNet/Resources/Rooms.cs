using Pathoschild.Http.Client;
using System;
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

		#region ZOOM ROOMS

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
				.AsPaginatedResponseWithToken<Room>("rooms");
		}

		/// <inheritdoc/>
		public Task<Room> CreateAsync(string name, RoomType type, string locationId = null, string calendarId = null, string[] tagIds = null, string userId = null, bool? isProDevice = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "type", type.ToEnumString() },
				{ "location_id", locationId },
				{ "calendar_resource_id", calendarId },
				{ "tag_ids", tagIds?.ToArray() },
				{ "user_id", userId },
				{ "pro_device", isProDevice }
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
		public Task MoveAsync(string roomId, string locationId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "location_id", locationId }
			};

			return _client
				.PutAsync($"rooms/{roomId}/location")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<(RoomBasicProfile Basic, string DeviceProfileId, RoomSetupProfile Setup)> GetProfileAsync(string roomId, bool regenerateActivationCode = false, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/{roomId}")
				.WithArgument("regenerate_activation_code", regenerateActivationCode)
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var basicProfile = response.GetProperty("basic", true)?.ToObject<RoomBasicProfile>();
			var deviceProfileId = response.GetPropertyValue<string>("device/device_profile_id", null);
			var setupProfile = response.GetProperty("setup", true)?.ToObject<RoomSetupProfile>();

			return (basicProfile, deviceProfileId, setupProfile);
		}

		/// <inheritdoc/>
		public Task DisplayEmergencyContentToAccountsAsync(string content, IEnumerable<string> accountIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_displayed" },
				{
					"params", new JsonObject
					{
						{ "content", content },
						{ "target_ids", accountIds?.ToArray() },
						{ "target_type", "account" }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DisplayEmergencyContentToLocationsAsync(string content, IEnumerable<string> locationIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_displayed" },
				{
					"params", new JsonObject
					{
						{ "content", content },
						{ "target_ids", locationIds?.ToArray() },
						{ "target_type", "location" }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DisplayEmergencyContentToRoomsAsync(string content, IEnumerable<string> roomIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_displayed" },
				{
					"params", new JsonObject
					{
						{ "content", content },
						{ "target_ids", roomIds?.ToArray() },
						{ "target_type", "room" }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemoveEmergencyContentFromAccountsAsync(IEnumerable<string> accountIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_removed" },
				{
					"params", new JsonObject
					{
						{ "target_ids", accountIds?.ToArray() },
						{ "target_type", "account" },
						{ "force_remove", true }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemoveEmergencyContentFromLocationsAsync(IEnumerable<string> locationIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_removed" },
				{
					"params", new JsonObject
					{
						{ "target_ids", locationIds?.ToArray() },
						{ "target_type", "location" },
						{ "force_remove", true }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemoveEmergencyContentFromRoomsAsync(IEnumerable<string> roomIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "method", "zoomroom.emergency_alert_removed" },
				{
					"params", new JsonObject
					{
						{ "target_ids", roomIds?.ToArray() },
						{ "target_type", "room" },
						{ "force_remove", true }
					}
				}
			};

			return _client
				.PatchAsync($"rooms/events")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<(RoomAlertSettings AlertSettings, RoomNotificationSettings NotificationSettings)> GetAlertSettingsAsync(string roomId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/{roomId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Alert.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var alertSettings = response.GetProperty("client_alert", true)?.ToObject<RoomAlertSettings>();
			var notificationSettings = response.GetProperty("notification", true)?.ToObject<RoomNotificationSettings>();

			return (alertSettings, notificationSettings);
		}

		/// <inheritdoc/>
		public async Task<(RoomSecuritySettings SecuritySettings, RoomSettings RoomSettings)> GetSettingsAsync(string roomId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/{roomId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Meeting.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var securitySettings = response.GetProperty("meeting_security", true)?.ToObject<RoomSecuritySettings>();
			var roomSettings = response.GetProperty("zoom_rooms", true)?.ToObject<RoomSettings>();

			return (securitySettings, roomSettings);
		}

		/// <inheritdoc/>
		public Task<RoomSignageSettings> GetSignageSettingsAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Signage.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomSignageSettings>("digital_signage");
		}

		/// <inheritdoc/>
		public Task<RoomSchedulingDisplaySettings> GetSchedulingDisplaySettingsAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.SchedulingDisplay.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomSchedulingDisplaySettings>("scheduling_display");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithTokenAndDateRange<RoomSensorData>> GetSensorDataAsync(string roomId, string deviceId = null, RoomSensorType? sensorType = null, DateTime? from = null, DateTime? to = null, int recordsPerPage = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms/{roomId}/sensor_data")
				.WithArgument("device_id", deviceId)
				.WithArgument("sensor_type", sensorType?.ToEnumString())
				.WithArgument("from", from?.ToZoomFormat())
				.WithArgument("to", to?.ToZoomFormat())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<RoomSensorData>("sensor_data");
		}

		/// <inheritdoc/>
		public Task<string> GetVirtualControllerUrlAsync(string roomId, bool preAuthenticatedLink = false, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/virtual_controller")
				.WithArgument("pre_authenticated_link", preAuthenticatedLink)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("url");
		}

		/// <inheritdoc/>
		public Task<RoomDeviceProfile[]> GetDeviceProfilesAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/device_profiles")
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomDeviceProfile[]>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<SignageContentItem>> GetSignageContentsAsync(SignageResourceType resourceType, string folderId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms/digital_signage")
				.WithArgument("type", resourceType.ToEnumString())
				.WithArgument("folder_id", folderId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<SignageContentItem>("contents");
		}

		#endregion

		#region ZOOM LOCATIONS

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
		public async Task<(RoomAlertSettings AlertSettings, RoomNotificationSettings NotificationSettings)> GetLocationAlertSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Alert.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var alertSettings = response.GetProperty("client_alert", true)?.ToObject<RoomAlertSettings>();
			var notificationSettings = response.GetProperty("notification", true)?.ToObject<RoomNotificationSettings>();

			return (alertSettings, notificationSettings);
		}

		/// <inheritdoc/>
		public async Task<(RoomSecuritySettings SecuritySettings, RoomSettings RoomSettings)> GetLocationSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Meeting.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var securitySettings = response.GetProperty("meeting_security", true)?.ToObject<RoomSecuritySettings>();
			var roomSettings = response.GetProperty("zoom_rooms", true)?.ToObject<RoomSettings>();

			return (securitySettings, roomSettings);
		}

		/// <inheritdoc/>
		public Task<RoomSignageSettings> GetLocationSignageSettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.Signage.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomSignageSettings>("digital_signage");
		}

		/// <inheritdoc/>
		public Task<RoomSchedulingDisplaySettings> GetLocationSchedulingDisplaySettingsAsync(string locationId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/locations/{locationId}/settings")
				.WithArgument("setting_type", RoomLocationSettingsType.SchedulingDisplay.ToEnumString())
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomSchedulingDisplaySettings>("scheduling_display");
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
			var setupProfile = response.GetProperty("setup", true)?.ToObject<RoomLocationSetupProfile>();

			return (basicProfile, setupProfile);
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

		#endregion

		#region ZOOM ROOMS TAGS

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

		#endregion

		#region ZOOM ROOM DEVICES

		/// <inheritdoc/>
		public Task<RoomDevice[]> GetAllDevicesAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/devices")
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomDevice[]>("devices");
		}

		/// <inheritdoc/>
		public Task GetDevicesInformationAsync(string roomId, CancellationToken cancellationToken = default)
		{
			/*
				NOTE TO SELF: I haven't been able to test this functionality. The response to this endpoint is always empty.
			*/

			return _client
				.GetAsync($"rooms/{roomId}/device_profiles/devices")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task CreateDeviceProfileAsync(string roomId, bool? enableAudioProcessing = null, bool? autoAdjustMicrophoneLevel = null, string cameraId = null, bool? enableEchoCancellation = null, string microphoneId = null, string name = null, RoomDeviceNoiseSuppressionType? noiseSuppressionType = null, string speakerId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "audio_processing", enableAudioProcessing },
				{ "auto_adjust_mic_level", autoAdjustMicrophoneLevel },
				{ "camera_id", cameraId },
				{ "echo_cancellation", enableEchoCancellation },
				{ "microphone_id", microphoneId },
				{ "name", name },
				{ "noise_suppression", noiseSuppressionType?.ToEnumString() },
				{ "speaker_id", speakerId }
			};

			/*
				NOTE TO SELF: I haven't been able to test this functionality because I get the following error message:
				"Unable to create device profile because there is no microphone/speaker/camera available in the following Zoom Room: aDLGFI6hRvaXkISCUXzUOA."
			*/

			return _client
				.PostAsync($"rooms/{roomId}/device_profiles")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteDeviceProfileAsync(string roomId, string deviceProfileId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/{roomId}/device_profiles/{deviceProfileId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<RoomDeviceProfile> GetDeviceProfileAsync(string roomId, string deviceProfileId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"rooms/{roomId}/device_profiles/{deviceProfileId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomDeviceProfile>();
		}

		/// <inheritdoc/>
		public Task UpgradeAppVersionAsync(string roomId, string deviceId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "upgrade" }
			};

			return _client
				.PutAsync($"rooms/{roomId}/devices/{deviceId}/app_version")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DowngradeAppVersionAsync(string roomId, string deviceId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "downgrade" }
			};

			return _client
				.PutAsync($"rooms/{roomId}/devices/{deviceId}/app_version")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task CancelAppVersionChangeAsync(string roomId, string deviceId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "action", "cancel" }
			};

			return _client
				.PutAsync($"rooms/{roomId}/devices/{deviceId}/app_version")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteDeviceAsync(string roomId, string deviceId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"rooms/{roomId}/devices/{deviceId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion
	}
}
