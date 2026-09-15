using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the breakout room settings for a meeting.</summary>
	public class MeetingBreakoutRoomSettings
	{
		/// <summary>Gets or sets a value indicating whether the breakout room pre-assign option is enabled.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the rooms.</summary>
		[JsonPropertyName("rooms")]
		public PreAssignedBreakoutRoom[] Rooms { get; set; }
	}
}
