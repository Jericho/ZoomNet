using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the waiting room settings for a meeting.</summary>
	public class MeetingWaitingRoomSettings
	{
		/// <summary>Gets or sets the mode of the waiting room.</summary>
		[JsonPropertyName("mode")]
		public MeetingWaitingRoomMode Mode { get; set; }

		/// <summary>Gets or sets who goes to the waiting room.</summary>
		[JsonPropertyName("who_goes_to_waiting_room")]
		public MeetingWaitingRoomParticipants WhoGoesToWaitingRoom { get; set; }
	}
}
