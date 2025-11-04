using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time an invitation to join a meeting through phone (call out) from a Zoom room fails.
	/// </summary>
	public class MeetingParticipantRoomSystemCalloutFailedEvent : MeetingParticipantRoomSystemCalloutEvent
	{
		/// <summary>
		/// Gets or sets the reason type for failure.
		/// </summary>
		[JsonPropertyName("reason_type")]
		public MeetingRoomCalloutFailureReason ReasonType { get; set; }
	}
}
