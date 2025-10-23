using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Breakout room and main meeting information as received in webhook events related to breakout rooms.
	/// </summary>
	public class BreakoutRoomMeetingInfo
	{
		/// <summary>
		/// Gets or sets the breakout room's universally unique identifier (UUID).
		/// Each breakout room instance generates a breakout room UUID.
		/// </summary>
		[JsonPropertyName("breakout_room_uuid")]
		public string BreakoutRoomUuid { get; set; }

		/// <summary>
		/// Gets or sets the main meeting id, also known as the meeting number.
		/// </summary>
		[JsonPropertyName("id")]
		/*
			This allows us to overcome the fact that "id" is sometimes a string and sometimes a number
			See: https://devforum.zoom.us/t/the-data-type-of-meetingid-is-inconsistent-in-webhook-documentation/70090
			Also, see: https://github.com/Jericho/ZoomNet/issues/228
		*/
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the main meeting's unique id.
		/// </summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the main scheduled meeting duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the main meeting.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

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
		/// Gets or sets the topic of the main meeting.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the main meeting type.
		/// </summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }
	}
}
