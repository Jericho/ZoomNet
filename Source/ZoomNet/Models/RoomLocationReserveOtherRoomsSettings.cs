using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location reserve other rooms settings.
	/// </summary>
	public class RoomLocationReserveOtherRoomsSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether a user is allowed to perform instant room reservations of other rooms on the device.
		/// </summary>
		[JsonPropertyName("allow_to_reserve_other_room_types")]
		public bool? AllowReserveOtherRooms { get; set; }

		/// <summary>
		/// Gets or sets the type of rooms the Zoom Room Scheduling Display will allow a user to find to reserve in the same parent Zoom Rooms Location Hierarchy Location as this device, and higher Location levels if so configured.
		/// </summary>
		[JsonPropertyName("reserve_other_room_location_types")]
		public RoomLocationType? LocationType { get; set; }
	}
}
