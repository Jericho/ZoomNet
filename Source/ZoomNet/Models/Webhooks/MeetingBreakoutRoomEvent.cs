using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a breakout room.
	/// </summary>
	public abstract class MeetingBreakoutRoomEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in which the event occurred.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the breakout room meeting object.
		/// </summary>
		[JsonPropertyName("object")]
		public BreakoutRoomMeetingInfo Meeting { get; set; }
	}
}
