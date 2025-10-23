namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user in a breakout room ends their content-sharing, such as their desktop or the classic whiteboard.
	/// </summary>
	public class MeetingBreakoutRoomSharingEndedEvent : MeetingBreakoutRoomEvent
	{
		/// <summary>
		/// Gets or sets the information about meeting participant.
		/// </summary>
		public BreakoutRoomParticipantBasicInfo Participant { get; set; }

		/// <summary>
		/// Gets or sets the information about the meeting's screenshare.
		/// </summary>
		public ScreenshareDetails SharingDetails { get; set; }
	}
}
