using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room.
	/// </summary>
	public class Room
	{
		/// <summary>
		/// Gets or sets the code used to complete the setup of the Zoom Room.
		/// </summary>
		[JsonPropertyName("activation_code")]
		public string ActivationCode { get; set; }

		/// <summary>
		/// Gets or sets the room id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the location of the room.
		/// </summary>
		[JsonPropertyName("location_id")]
		public string LocationId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Zoom Room ID. Use this ID for the Dashboard Zoom Room APIs.
		/// </summary>
		[JsonPropertyName("room_id")]
		public string RoomId { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		[JsonPropertyName("status")]
		public RoomStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		[JsonPropertyName("type")]
		public RoomType? Type { get; set; }

		/// <summary>
		/// Gets or sets the list of tag ID associated with the Zoom Room.
		/// </summary>
		[JsonPropertyName("tag_ids")]
		public string[] Tags { get; set; }

		/// <summary>
		/// Gets or sets the calendar ressource ID.
		/// </summary>
		[JsonPropertyName("calendar_resource_id")]
		public string CalendarResourceId { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user assigned to a Personal Zoom Room.
		/// Note: this field will only be present for Personal Zoom Rooms.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Personal Zoom Room will consume a Zoom Rooms license and have access to "Pro" features.
		/// Note: this field will only be present for Personal Zoom Rooms.
		/// </summary>
		[JsonPropertyName("pro_device")]
		public bool? IsProDevice { get; set; }
	}
}
