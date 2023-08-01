using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.RoomSettings;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage rooms.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IRooms" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/rooms/">Zoom documentation</a> for more information.
	/// </remarks>
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

		/// <summary>
		/// Retrieve all rooms on your account.
		/// </summary>
		/// <param name="status">Use this parameter to filter the response by the status of room.</param>
		/// <param name="type">Use this parameter to filter the response by the type of room.</param>
		/// <param name="unassignedRooms">Use this query parameter with a value of `true` if you would like to see Zoom Rooms in your account that have not been assigned to anyone yet.</param>
		/// <param name="locationId">Parent location ID of the Zoom Room.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Room">rooms</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<Room>> GetAllAsync(RoomStatus? status = null, RoomType? type = null, bool unassignedRooms = false, string locationId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
			       .GetAsync($"rooms")
			       .WithArgument("status", status.HasValue ? JToken.Parse(JsonConvert.SerializeObject(status.Value)).ToString() : null)
			       .WithArgument("type", type.HasValue ? JToken.Parse(JsonConvert.SerializeObject(type.Value)).ToString() : null)
			       .WithArgument("unassigned_rooms", unassignedRooms)
			       .WithArgument("location_id", locationId)
			       .WithArgument("page_size", recordsPerPage)
			       .WithArgument("next_page_token", pagingToken)
			       .WithCancellationToken(cancellationToken)
			       .AsPaginatedResponseWithToken<Room>("rooms");
		}

		/// <summary>
		/// Adds a room to your account.
		/// </summary>
		/// <param name="name">The name of the new room.</param>
		/// <param name="type">The type of the new room.</param>
		/// <param name="locationId">Parent location ID of the new room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The added <see cref="Room">room</see>.</returns>
		public Task<Room> AddAsync(string name, RoomType type, string locationId = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject
			{
				{"name", name},
				{"type", JToken.Parse(JsonConvert.SerializeObject(type))}
			};
			data.AddPropertyIfValue("location_id", locationId);

			return _client
			       .PostAsync("rooms")
			       .WithJsonBody(data)
			       .WithCancellationToken(cancellationToken)
			       .AsObject<Room>();
		}

		/// <summary>
		/// Deletes a room from your account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public Task DeleteAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
			       .DeleteAsync($"rooms/{roomId}")
			       .WithCancellationToken(cancellationToken)
			       .AsMessage();
		}

		/// <summary>
		/// Gets detailed information on a specific Zoom Room.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="RoomProfile">room profile</see>.</returns>
		public Task<RoomProfile> GetProfileAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
			       .GetAsync($"rooms/{roomId}")
			       .WithCancellationToken(cancellationToken)
			       .AsObject<RoomProfile>("basic");
		}

		/// <summary>
		/// Update basic information on a specific Zoom Room in a Zoom account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="name">The name of the room.</param>
		/// <param name="supportEmail">The support email address of the room.</param>
		/// <param name="supportPhone">The support phone number of the room.</param>
		/// <param name="roomPasscode">The passcode for the room.</param>
		/// <param name="requiredCodeToExit">A value determining if the room requires a code to leave the Zoom application.</param>
		/// <param name="hideRoomInContacts">A value determining if the room should be hidden from the contacts list.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public Task UpdateProfileAsync(string roomId, string name = null, string supportEmail = null, string supportPhone = null, string roomPasscode = null, bool? requiredCodeToExit = null, bool? hideRoomInContacts = null, CancellationToken cancellationToken = default)
		{
			var data = new JObject();
			data.AddPropertyIfValue("basic/name", name);
			data.AddPropertyIfValue("basic/support_email", supportEmail);
			data.AddPropertyIfValue("basic/support_phone", supportPhone);
			data.AddPropertyIfValue("basic/room_passcode", roomPasscode);
			data.AddPropertyIfValue("basic/required_code_to_ext", requiredCodeToExit);
			data.AddPropertyIfValue("basic/hide_room_in_contacts", hideRoomInContacts);

			return _client
			       .PutAsync($"rooms/{roomId}")
			       .WithJsonBody(data)
			       .WithCancellationToken(cancellationToken)
			       .AsMessage();
		}

		/// <summary>
		/// List information about the devices that are being used for a specific Zoom Room in an account.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="Device">devices</see></returns>
		public Task<Device[]> GetDevicesAsync(string roomId, CancellationToken cancellationToken = default)
		{
			return _client
			       .GetAsync($"rooms/{roomId}/devices")
			       .WithCancellationToken(cancellationToken)
			       .AsObject<Device[]>("devices");
		}

		/// <summary>
		/// Updates the location of the Zoom Room.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="locationId">Parent location ID of the room.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public Task UpdateLocationAsync(string roomId, string locationId, CancellationToken cancellationToken = default)
		{
			var data = new JObject()
			{
				{"location_id", locationId}
			};

			return _client
			       .PutAsync($"rooms/{roomId}/location")
			       .WithJsonBody(data)
			       .WithCancellationToken(cancellationToken)
			       .AsMessage();
		}

		/// <summary>
		/// Get information on meeting or alert settings applied to a specific Zoom Room.
		/// </summary>
		/// <param name="roomId">The ID of the room.</param>
		/// <param name="settingsType">The type of settings to query. Defaults to Meeting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="IRoomSettings">room settings</see>.</returns>
		public Task<IRoomSettings> GetSettingsAsync(string roomId, SettingsType settingsType = SettingsType.Meeting, CancellationToken cancellationToken = default)
		{
			var request =
				_client
					.GetAsync($"rooms/{roomId}/settings")
					.WithArgument("status", JToken.Parse(JsonConvert.SerializeObject(settingsType)).ToString())
					.WithCancellationToken(cancellationToken);

			switch (settingsType)
			{
				case SettingsType.Meeting:
					return request.AsObject<RoomMeetingSettings>().AsTask<RoomMeetingSettings, IRoomSettings>();

				case SettingsType.Alert:
					return request.AsObject<RoomAlertSettings>().AsTask<RoomAlertSettings, IRoomSettings>();

				default:
					throw new ArgumentOutOfRangeException(nameof(settingsType), settingsType, null);
			}
		}
	}
}
