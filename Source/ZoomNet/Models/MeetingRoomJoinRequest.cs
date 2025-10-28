using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information from invitation to join a meeting through phone (call out) from a Zoom room.
	/// </summary>
	public class MeetingRoomJoinRequest : MeetingBasicInfo
	{
		/// <summary>
		/// Gets or sets the user name of the event's trigger.
		/// </summary>
		[JsonPropertyName("inviter_name")]
		public string InviterName { get; set; }

		/// <summary>
		/// Gets or sets the request unique identifier (UUID).
		/// </summary>
		[JsonPropertyName("message_id")]
		public string MessageId { get; set; }
	}
}
