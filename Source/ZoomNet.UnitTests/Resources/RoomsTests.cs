using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class RoomsTests
	{
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
				.Respond("application/json", EndpointsResource.rooms_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Name.ShouldBe("My Personal Meeting Room");
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
				.Respond("application/json", EndpointsResource.rooms_GET);

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
				.Respond("application/json", EndpointsResource.rooms_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateAsync(name, type, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Name.ShouldBe("My Personal Meeting Room");
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
				.Respond("application/json", EndpointsResource.rooms_GET);

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
				.Respond("application/json", EndpointsResource.rooms__roomId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetProfileAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Basic.ShouldNotBeNull();
			result.Basic.Name.ShouldBe("My Personal Meeting Room");
			result.DeviceProfileId.ShouldBe("J352JVkNRpyAgUaurxmrsh");
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
				.Respond("application/json", EndpointsResource.rooms__roomId__GET);

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
				.Respond("application/json", EndpointsResource.rooms__id__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms__id__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms__id__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms__id__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms__roomId__sensor_data_GET);

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
			var from = new DateOnly(2023, 1, 1);
			var to = new DateOnly(2023, 1, 31);
			var recordsPerPage = 50;
			var pageToken = "token456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "sensor_data"))
				.WithQueryString("device_id", deviceId)
				.WithQueryString("sensor_type", sensorType.ToEnumString())
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pageToken)
				.Respond("application/json", EndpointsResource.rooms__roomId__sensor_data_GET);

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
				.Respond("application/json", EndpointsResource.rooms__roomId__virtual_controller_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetVirtualControllerUrlAsync(roomId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("https://zoom.us/launch/webzrc?nodeId=SnoDt6rcTqi7HIuFZsib9A");
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
				.Respond("application/json", EndpointsResource.rooms__roomId__virtual_controller_GET);

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
				.Respond("application/json", EndpointsResource.rooms__roomId__device_profiles_GET);

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
			result[0].Name.ShouldBe("ZR1 Device");
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
				.Respond("application/json", EndpointsResource.rooms_digital_signage_GET);

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
			result.Records[0].Name.ShouldBe("content name");
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
				.Respond("application/json", EndpointsResource.rooms_digital_signage_GET);

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
				.Respond("application/json", EndpointsResource.rooms_locations_structure_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationStructureAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
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
				.Respond("application/json", EndpointsResource.rooms_locations_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateLocationAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Name.ShouldBe("location name");
		}

		[Fact]
		public async Task CreateLocationAsync_WithParentId_ReturnsLocation()
		{
			// Arrange
			var name = "Floor 2";
			var parentId = "building123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("rooms", "locations"))
				.Respond("application/json", EndpointsResource.rooms_locations_POST);

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
				.Respond("application/json", EndpointsResource.rooms_locations__locationId__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms_locations__locationId__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms_locations__locationId__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms_locations__locationId__settings_GET);

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
				.Respond("application/json", EndpointsResource.rooms_locations__locationId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetLocationProfileAsync(locationId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.Basic.ShouldNotBeNull();
			result.Basic.Name.ShouldBe("State");
			result.Setup.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllLocationsAsync_ReturnsLocations()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", "locations"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", EndpointsResource.rooms_locations_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllLocationsAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Name.ShouldBe("BuildingA");
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
				.Respond("application/json", EndpointsResource.rooms_tags_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.CreateTagAsync(name, description, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.ShouldBe("5bfc6df7a11445ef81513b2c3b4c8d5d");
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
				.Respond("application/json", EndpointsResource.rooms__roomId__devices_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetAllDevicesAsync(roomId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
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

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("rooms", roomId, "device_profiles", deviceProfileId))
				.Respond("application/json", EndpointsResource.rooms__roomId__device_profiles__deviceProfileId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var rooms = new Rooms(client);

			// Act
			var result = await rooms.GetDeviceProfileAsync(roomId, deviceProfileId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("Gz_fNcaSPByng-3vsqv_iQ");
			result.Name.ShouldBe("ZR1 Device");
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
