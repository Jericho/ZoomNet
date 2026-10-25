using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the waiting room settings for a meeting.</summary>
	public record MeetingWaitingRoomSettings
	{
		/// <summary>Gets or sets the mode of the waiting room.</summary>
		[JsonPropertyName("mode")]
		public MeetingWaitingRoomMode Mode { get; init; }

		/// <summary>Gets or sets who goes to the waiting room.</summary>
		[JsonPropertyName("who_goes_to_waiting_room")]
		public MeetingWaitingRoomParticipants WhoGoesToWaitingRoom { get; init; }
	}
}
