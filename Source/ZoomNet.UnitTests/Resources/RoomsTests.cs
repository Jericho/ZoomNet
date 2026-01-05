using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class RoomsTests
	{
		private const string ROOMS_LIST_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""rooms"": [
				{
					""id"": ""room1"",
					""name"": ""Conference Room A"",
					""type"": ""ZoomRoom"",
					""status"": ""Available"",
					""location_id"": ""loc1""
				},
				{
					""id"": ""room2"",
					""name"": ""Conference Room B"",
					""type"": ""SchedulingDisplayOnly"",
					""status"": ""Available"",
					""location_id"": ""loc2""
				}
			]
		}";

		private const string ROOM_JSON = @"{
			""id"": ""room123"",
			""name"": ""New Conference Room"",
			""type"": ""ZoomRoom"",
			""status"": ""Available"",
			""location_id"": ""loc123""
		}";

		private const string ROOM_PROFILE_JSON = @"{
			""basic"": {
				""id"": ""room123"",
				""name"": ""Conference Room A"",
				""activation_code"": ""ABC123"",
				""timezone"": ""America/New_York""
			},
			""device"": {
				""device_profile_id"": ""profile123""
			},
			""setup"": {
				""room_passcode"": ""123456"",
				""required_code_to_ext"": false
			}
		}";

		private const string LOCATIONS_LIST_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""locations"": [
				{
					""id"": ""loc1"",
					""name"": ""Building A"",
					""type"": 1,
					""parent_location_id"": null
				},
				{
					""id"": ""loc2"",
					""name"": ""Floor 1"",
					""type"": 2,
					""parent_location_id"": ""loc1""
				}
			]
		}";

		private const string LOCATION_JSON = @"{
			""id"": ""loc123"",
			""name"": ""New Building"",
			""type"": 1,
			""parent_location_id"": null
		}";

		private const string LOCATION_PROFILE_JSON = @"{
			""basic"": {
				""id"": ""loc123"",
				""name"": ""Building A"",
				""address"": ""123 Main St"",
				""timezone"": ""America/New_York""
			},
			""setup"": {
				""room_passcode"": ""654321"",
				""required_code_to_ext"": true
			}
		}";

		private const string TAG_ID_JSON = @"{
			""id"": ""tag123""
		}";

		private const string TAGS_LIST_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""tags"": [
				{
					""id"": ""tag1"",
					""name"": ""VIP Rooms"",
					""description"": ""Rooms for executives""
				},
				{
					""id"": ""tag2"",
					""name"": ""Training Rooms"",
					""description"": ""Rooms for training sessions""
				}
			]
		}";

		private const string ROOM_DEVICES_JSON = @"{
			""devices"": [
				{
					""id"": ""device1"",
					""device_type"": ""Controller"",
					""device_name"": ""iPad Controller"",
					""app_version"": ""5.10.0""
				},
				{
					""id"": ""device2"",
					""device_type"": ""Zoom Rooms Computer"",
					""device_name"": ""Main Computer"",
					""app_version"": ""5.10.0""
				}
			]
		}";

		private const string ROOM_DEVICE_PROFILES_JSON = @"[
			{
				""id"": ""profile1"",
				""name"": ""Default Profile"",
				""camera_id"": ""cam1"",
				""microphone_id"": ""mic1"",
				""speaker_id"": ""speaker1""
			}
		]";

		private const string SIGNAGE_CONTENTS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""contents"": [
				{
					""id"": ""content1"",
					""name"": ""Welcome Screen"",
					""type"": ""image"",
					""folder_id"": ""folder1""
				}
			]
		}";

		private const string SENSOR_DATA_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""token123"",
			""from"": ""2023-01-01"",
			""to"": ""2023-01-31"",
			""sensor_data"": [
				{
					""device_id"": ""device1"",
					""sensor_type"": ""temperature"",
					""value"": 72.5,
					""timestamp"": ""2023-01-15T10:00:00Z""
				}
			]
		}";

		private const string ALERT_SETTINGS_JSON = @"{
			""client_alert"": {
				""device_system_update_time"": 14
			},
			""notification"": {
				""jbh_reminder"": true
			}
		}";

		private const string ROOM_SETTINGS_JSON = @"{
			""meeting_security"": {
				""encrypt_shared_screen_content"": true
			},
			""zoom_rooms"": {
				""auto_start_stop_scheduled_meetings"": true
			}
		}";

		private const string SIGNAGE_SETTINGS_JSON = @"{
			""digital_signage"": {
				""enable_digital_signage"": true,
				""display_mode"": ""full_screen""
			}
		}";

		private const string SCHEDULING_DISPLAY_SETTINGS_JSON = @"{
			""scheduling_display"": {
				""display_meeting_list"": true,
				""all_day_event"": true
			}
		}";

		private const string VIRTUAL_CONTROLLER_URL_JSON = @"{
			""url"": ""https://zoom.us/j/room123/controller""
		}";

		private const string LOCATION_STRUCTURE_JSON = @"{
			""structures"": [1, 2, 3, 4]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public RoomsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region Zoom Rooms Tests

		[Fact]
		public async Task GetAllAsync_ReturnsRooms()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", ROOMS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Name.ShouldBe("Conference Room A");
		}

		[Fact]
		public async Task GetAllAsync_WithParentLocationAndType_ReturnsRooms()
		{
			// Arrange
			var parentLocationId = "loc1";
			var type = RoomLocationType.Floor;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms"))
				.WithQueryString("parent_location_id", parentLocationId)
				.WithQueryString("type", type.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", ROOMS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllAsync(parentLocationId, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_WithRequiredParameters_ReturnsRoom()
		{
			// Arrange
			var name = "New Conference Room";
			var type = RoomType.Room;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms"))
				.Respond("application/json", ROOM_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateAsync(name, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Name.ShouldBe("New Conference Room");
		}

		[Fact]
		public async Task CreateAsync_WithAllParameters_ReturnsRoom()
		{
			// Arrange
			var name = "Full Featured Room";
			var type = RoomType.Room;
			var locationId = "loc123";
			var calendarId = "calendar@example.com";
			var tagIds = new[] { "tag1", "tag2" };
			var userId = "user123";
			var isProDevice = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms"))
				.Respond("application/json", ROOM_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateAsync(name, type, locationId, calendarId, tagIds, userId, isProDevice, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteAsync_WithRoomId_Succeeds()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", roomId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DeleteAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task MoveAsync_WithRoomIdAndLocation_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var locationId = "loc456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", roomId, "location"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.MoveAsync(roomId, locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetProfileAsync_WithRoomId_ReturnsProfile()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId))
				.WithQueryString("regenerate_activation_code", false.ToString())
				.Respond("application/json", ROOM_PROFILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetProfileAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Basic.ShouldNotBeNull();
			result.Basic.Name.ShouldBe("Conference Room A");
			result.DeviceProfileId.ShouldBe("profile123");
			result.Setup.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetProfileAsync_WithRegenerateActivationCode_ReturnsProfile()
		{
			// Arrange
			var roomId = "room123";
			var regenerateActivationCode = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId))
				.WithQueryString("regenerate_activation_code", true.ToString())
				.Respond("application/json", ROOM_PROFILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetProfileAsync(roomId, regenerateActivationCode, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Basic.ShouldNotBeNull();
		}

		[Fact]
		public async Task DisplayEmergencyContentToAccountsAsync_WithContentAndAccountIds_Succeeds()
		{
			// Arrange
			var content = "Emergency: Building evacuation required";
			var accountIds = new[] { "account1", "account2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DisplayEmergencyContentToAccountsAsync(content, accountIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DisplayEmergencyContentToLocationsAsync_WithContentAndLocationIds_Succeeds()
		{
			// Arrange
			var content = "Emergency: Severe weather alert";
			var locationIds = new[] { "loc1", "loc2", "loc3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DisplayEmergencyContentToLocationsAsync(content, locationIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DisplayEmergencyContentToRoomsAsync_WithContentAndRoomIds_Succeeds()
		{
			// Arrange
			var content = "Emergency: Fire alarm activated";
			var roomIds = new[] { "room1", "room2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DisplayEmergencyContentToRoomsAsync(content, roomIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveEmergencyContentFromAccountsAsync_WithAccountIds_Succeeds()
		{
			// Arrange
			var accountIds = new[] { "account1", "account2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.RemoveEmergencyContentFromAccountsAsync(accountIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveEmergencyContentFromLocationsAsync_WithLocationIds_Succeeds()
		{
			// Arrange
			var locationIds = new[] { "loc1", "loc2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.RemoveEmergencyContentFromLocationsAsync(locationIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveEmergencyContentFromRoomsAsync_WithRoomIds_Succeeds()
		{
			// Arrange
			var roomIds = new[] { "room1", "room2", "room3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "events"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.RemoveEmergencyContentFromRoomsAsync(roomIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetAlertSettingsAsync_WithRoomId_ReturnsSettings()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "settings"))
				.WithQueryString("setting_type", "alert")
				.Respond("application/json", ALERT_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAlertSettingsAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.AlertSettings.ShouldNotBeNull();
			result.NotificationSettings.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSettingsAsync_WithRoomId_ReturnsSettings()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "settings"))
				.WithQueryString("setting_type", "meeting")
				.Respond("application/json", ROOM_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSettingsAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.SecuritySettings.ShouldNotBeNull();
			result.RoomSettings.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSignageSettingsAsync_WithRoomId_ReturnsSettings()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "settings"))
				.WithQueryString("setting_type", "signage")
				.Respond("application/json", SIGNAGE_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSignageSettingsAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetSchedulingDisplaySettingsAsync_WithRoomId_ReturnsSettings()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "settings"))
				.WithQueryString("setting_type", "scheduling_display")
				.Respond("application/json", SCHEDULING_DISPLAY_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSchedulingDisplaySettingsAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Additional Room Tests

		[Fact]
		public async Task GetSensorDataAsync_WithRoomId_ReturnsSensorData()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "sensor_data"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", SENSOR_DATA_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSensorDataAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task GetSensorDataAsync_WithAllParameters_ReturnsSensorData()
		{
			// Arrange
			var roomId = "room123";
			var deviceId = "device1";
			var sensorType = RoomSensorType.Temperature;
			var from = new DateTime(2023, 1, 1);
			var to = new DateTime(2023, 1, 31);
			var recordsPerPage = 50;
			var pageToken = "token456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "sensor_data"))
				.WithQueryString("device_id", deviceId)
				.WithQueryString("sensor_type", sensorType.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", SENSOR_DATA_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSensorDataAsync(roomId, deviceId, sensorType, from, to, recordsPerPage, pageToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetVirtualControllerUrlAsync_WithRoomId_ReturnsUrl()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "virtual_controller"))
				.WithQueryString("pre_authenticated_link", false.ToString())
				.Respond("application/json", VIRTUAL_CONTROLLER_URL_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetVirtualControllerUrlAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
		mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("https://zoom.us/j/room123/controller");
		}

		[Fact]
		public async Task GetVirtualControllerUrlAsync_WithPreAuthenticatedLink_ReturnsUrl()
		{
			// Arrange
			var roomId = "room123";
			var preAuthenticatedLink = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "virtual_controller"))
				.WithQueryString("pre_authenticated_link", true.ToString())
				.Respond("application/json", VIRTUAL_CONTROLLER_URL_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetVirtualControllerUrlAsync(roomId, preAuthenticatedLink, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetDeviceProfilesAsync_WithRoomId_ReturnsProfiles()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "device_profiles"))
				.Respond("application/json", ROOM_DEVICE_PROFILES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetDeviceProfilesAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Name.ShouldBe("Default Profile");
		}

		[Fact]
		public async Task GetSignageContentsAsync_WithResourceType_ReturnsContents()
		{
			// Arrange
			var resourceType = SignageResourceType.Content;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "digital_signage"))
				.WithQueryString("type", resourceType.ToEnumString())
				.WithQueryString("page_size", "30")
				.Respond("application/json", SIGNAGE_CONTENTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSignageContentsAsync(resourceType, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Name.ShouldBe("Welcome Screen");
		}

		[Fact]
		public async Task GetSignageContentsAsync_WithFolderId_ReturnsContents()
		{
			// Arrange
			var resourceType = SignageResourceType.Folder;
			var folderId = "folder123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "digital_signage"))
				.WithQueryString("type", resourceType.ToEnumString())
				.WithQueryString("folder_id", folderId)
				.WithQueryString("page_size", "30")
				.Respond("application/json", SIGNAGE_CONTENTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSignageContentsAsync(resourceType, folderId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Location Structure Tests

		[Fact]
		public async Task GetLocationStructureAsync_ReturnsStructure()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", "structure"))
				.Respond("application/json", LOCATION_STRUCTURE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationStructureAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(4);
		}

		[Fact]
		public async Task UpdateLocationStructureAsync_WithStructure_Succeeds()
		{
			// Arrange
			var structure = new[] { RoomLocationType.Country, RoomLocationType.State, RoomLocationType.City, RoomLocationType.Building };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", "locations", "structure"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.UpdateLocationStructureAsync(structure, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateLocationAsync_WithName_ReturnsLocation()
		{
			// Arrange
			var name = "New Building";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", "locations"))
				.Respond("application/json", LOCATION_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateLocationAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Name.ShouldBe("New Building");
		}

		[Fact]
		public async Task CreateLocationAsync_WithParentId_ReturnsLocation()
		{
			// Arrange
			var name = "Floor 2";
			var parentId = "building123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", "locations"))
				.Respond("application/json", LOCATION_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateLocationAsync(name, parentId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DeleteLocationAsync_WithLocationId_Succeeds()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", "locations", locationId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DeleteLocationAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task MoveLocationASync_WithLocationIdAndParentId_Succeeds()
		{
			// Arrange
			var locationId = "loc123";
			var parentId = "newparent456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", "locations", locationId, "location"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.MoveLocationASync(locationId, parentId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Location Settings Tests

		[Fact]
		public async Task GetLocationAlertSettingsAsync_WithLocationId_ReturnsSettings()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", locationId, "settings"))
				.WithQueryString("setting_type", "alert")
				.Respond("application/json", ALERT_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationAlertSettingsAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.AlertSettings.ShouldNotBeNull();
			result.NotificationSettings.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetLocationSettingsAsync_WithLocationId_ReturnsSettings()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", locationId, "settings"))
				.WithQueryString("setting_type", "meeting")
				.Respond("application/json", ROOM_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationSettingsAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.SecuritySettings.ShouldNotBeNull();
			result.RoomSettings.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetLocationSignageSettingsAsync_WithLocationId_ReturnsSettings()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", locationId, "settings"))
				.WithQueryString("setting_type", "signage")
				.Respond("application/json", SIGNAGE_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationSignageSettingsAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetLocationSchedulingDisplaySettingsAsync_WithLocationId_ReturnsSettings()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", locationId, "settings"))
				.WithQueryString("setting_type", "scheduling_display")
				.Respond("application/json", SCHEDULING_DISPLAY_SETTINGS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationSchedulingDisplaySettingsAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetLocationProfileAsync_WithLocationId_ReturnsProfile()
		{
			// Arrange
			var locationId = "loc123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations", locationId))
				.WithQueryString("setting_type", "meeting")
				.Respond("application/json", LOCATION_PROFILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationProfileAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Basic.ShouldNotBeNull();
			result.Basic.Name.ShouldBe("Building A");
			result.Setup.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllLocationsAsync_ReturnsLocations()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", LOCATIONS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllLocationsAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Name.ShouldBe("Building A");
		}

		#endregion

		#region Tags Tests

		[Fact]
		public async Task CreateTagAsync_WithNameAndDescription_ReturnsTagId()
		{
			// Arrange
			var name = "Conference Rooms";
			var description = "All conference room facilities";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", "tags"))
				.Respond("application/json", TAG_ID_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateTagAsync(name, description, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("tag123");
		}

		[Fact]
		public async Task UpdateTagAsync_WithNameAndDescription_Succeeds()
		{
			// Arrange
			var tagId = "tag123";
			var name = "Updated Tag Name";
			var description = "Updated description";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "tags", tagId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.UpdateTagAsync(tagId, name, description, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignTagsToRoom_WithRoomIdAndTagIds_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var tagIds = new[] { "tag1", "tag2", "tag3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", roomId, "tags"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.AssignTagsToRoom(roomId, tagIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignTagsToRoomsInLocation_WithLocationIdAndTagIds_Succeeds()
		{
			// Arrange
			var locationId = "loc123";
			var tagIds = new[] { "tag1", "tag2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "locations", locationId, "tags"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.AssignTagsToRoomsInLocation(locationId, tagIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UnAssignTagFromRoom_WithRoomIdAndTagId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var tagId = "tag1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", roomId, "tags"))
				.WithQueryString("tag_ids", tagId)
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.UnAssignTagFromRoom(roomId, tagId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteTagAsync_WithTagId_Succeeds()
		{
			// Arrange
			var tagId = "tag123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", "tags", tagId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DeleteTagAsync(tagId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Device Management Tests

		[Fact]
		public async Task GetAllDevicesAsync_WithRoomId_ReturnsDevices()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "devices"))
				.Respond("application/json", ROOM_DEVICES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllDevicesAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
		}

		[Fact]
		public async Task UpgradeAppVersionAsync_WithRoomIdAndDeviceId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var deviceId = "device1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", roomId, "devices", deviceId, "app_version"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.UpgradeAppVersionAsync(roomId, deviceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DowngradeAppVersionAsync_WithRoomIdAndDeviceId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var deviceId = "device1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", roomId, "devices", deviceId, "app_version"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DowngradeAppVersionAsync(roomId, deviceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CancelAppVersionChangeAsync_WithRoomIdAndDeviceId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var deviceId = "device1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("rooms", roomId, "devices", deviceId, "app_version"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.CancelAppVersionChangeAsync(roomId, deviceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteDeviceAsync_WithRoomIdAndDeviceId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var deviceId = "device1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", roomId, "devices", deviceId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DeleteDeviceAsync(roomId, deviceId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Additional Device Profile Tests

		[Fact]
		public async Task GetDeviceProfileAsync_WithRoomIdAndProfileId_ReturnsProfile()
		{
			// Arrange
			var roomId = "room123";
			var deviceProfileId = "profile1";

			var deviceProfileJson = @"{
				""id"": ""profile1"",
				""name"": ""Custom Profile"",
				""camera_id"": ""cam1"",
				""microphone_id"": ""mic1"",
				""speaker_id"": ""speaker1"",
				""audio_processing"": true,
				""auto_adjust_mic_level"": false,
				""echo_cancellation"": true,
				""noise_suppression"": ""moderate""
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "device_profiles", deviceProfileId))
				.Respond("application/json", deviceProfileJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetDeviceProfileAsync(roomId, deviceProfileId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("profile1");
			result.Name.ShouldBe("Custom Profile");
		}

		[Fact]
		public async Task DeleteDeviceProfileAsync_WithRoomIdAndProfileId_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var deviceProfileId = "profile1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("rooms", roomId, "device_profiles", deviceProfileId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.DeleteDeviceProfileAsync(roomId, deviceProfileId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateDeviceProfileAsync_WithMinimalParameters_Succeeds()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", roomId, "device_profiles"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.CreateDeviceProfileAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateDeviceProfileAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var roomId = "room123";
			var enableAudioProcessing = true;
			var autoAdjustMicrophoneLevel = false;
			var cameraId = "cam123";
			var enableEchoCancellation = true;
			var microphoneId = "mic123";
			var name = "My Device Profile";
			var noiseSuppressionType = RoomDeviceNoiseSuppressionType.Moderate;
			var speakerId = "speaker123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", roomId, "device_profiles"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.CreateDeviceProfileAsync(roomId, enableAudioProcessing, autoAdjustMicrophoneLevel, cameraId, enableEchoCancellation, microphoneId, name, noiseSuppressionType, speakerId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task GetDevicesInformationAsync_WithRoomId_Succeeds()
		{
			// Arrange
			var roomId = "room123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "device_profiles", "devices"))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.GetDevicesInformationAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Pagination Tests

		[Fact]
		public async Task GetAllAsync_WithPagingToken_ReturnsRooms()
		{
			// Arrange
			var pagingToken = "token456";
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", ROOMS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllAsync(recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllLocationsAsync_WithPagingToken_ReturnsLocations()
		{
			// Arrange
			var pagingToken = "token789";
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", LOCATIONS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllLocationsAsync(recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllTagsAsync_WithDefaultParameters_ReturnsTags()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "tags"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", TAGS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllTagsAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Name.ShouldBe("VIP Rooms");
		}

		[Fact]
		public async Task GetAllTagsAsync_WithPagingToken_ReturnsTags()
		{
			// Arrange
			var pagingToken = "token321";
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "tags"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", TAGS_LIST_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllTagsAsync(recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetSignageContentsAsync_WithPagingToken_ReturnsContents()
		{
			// Arrange
			var resourceType = SignageResourceType.Content;
			var pagingToken = "token654";
			var recordsPerPage = 50;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "digital_signage"))
				.WithQueryString("type", resourceType.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", SIGNAGE_CONTENTS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetSignageContentsAsync(resourceType, recordsPerPage: recordsPerPage, pagingToken: pagingToken, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		#endregion

		#region Location Profile Update Tests

		[Fact]
		public async Task UpdateLocationProfileAsync_WithAllParameters_Succeeds()
		{
			// Arrange
			var locationId = "loc123";
			var address = "789 Corporate Blvd";
			var description = "Main corporate office";
			var name = "Corporate Headquarters";
			var codeIsRequiredToExit = true;
			var passcode = "987654";
			var supportEmail = "support@example.com";
			var supportPhone = "+1234567890";
			var timezone = TimeZones.America_Los_Angeles;
			var applyBackgroundImageToAllDisplays = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("rooms", "locations", locationId))
				.Respond(System.Net.HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			await rooms.UpdateLocationProfileAsync(locationId, address, description, name, codeIsRequiredToExit, passcode, supportEmail, supportPhone, timezone, applyBackgroundImageToAllDisplays, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
