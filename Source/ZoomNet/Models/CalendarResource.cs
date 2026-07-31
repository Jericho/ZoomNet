using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Calendar resource record.
	/// </summary>
	public class CalendarResource
	{
		/// <summary>
		/// Gets or sets the id of the calendar resource.
		/// </summary>
		[JsonPropertyName("calendar_resource_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the email of the calendar resource.
		/// </summary>
		[JsonPropertyName("calendar_resource_email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the name of the calendar resource.
		/// </summary>
		[JsonPropertyName("calendar_resource_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the id of the room assigned to the calendar resource.
		/// </summary>
		[JsonPropertyName("assigned_room_id")]
		public string AssignedRoomId { get; set; }

		/// <summary>
		/// Gets or sets the sync status of the calendar resource.
		/// </summary>
		[JsonPropertyName("sync_status")]
		public CalendarResourceSyncStatus SyncStatus { get; set; }
	}
}
