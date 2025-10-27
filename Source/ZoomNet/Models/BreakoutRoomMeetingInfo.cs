using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Breakout room and main meeting information as received in webhook events related to breakout rooms.
	/// </summary>
	public class BreakoutRoomMeetingInfo : MeetingBasicInfo
	{
		/// <summary>
		/// Gets or sets the breakout room's universally unique identifier (UUID).
		/// Each breakout room instance generates a breakout room UUID.
		/// </summary>
		[JsonPropertyName("breakout_room_uuid")]
		public string BreakoutRoomUuid { get; set; }

		/// <summary>
		/// Gets or sets the main scheduled meeting duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the main meeting's start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the main meeting's timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>
		/// Gets or sets the main meeting type.
		/// </summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }
	}
}
