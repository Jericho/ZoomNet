namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user in a breakout room begins sharing content, such as their desktop or the classic whiteboard.
	/// </summary>
	public class MeetingBreakoutRoomSharingStartedEvent : MeetingBreakoutRoomEvent
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
