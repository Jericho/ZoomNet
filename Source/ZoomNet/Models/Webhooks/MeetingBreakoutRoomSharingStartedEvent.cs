namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a user in a breakout room begins sharing content, such as their desktop or the classic whiteboard.</summary>
	public record MeetingBreakoutRoomSharingStartedEvent : MeetingBreakoutRoomEvent
	{
		/// <summary>Gets or sets the information about meeting participant.</summary>
		public BreakoutRoomParticipantBasicInfo Participant { get; init; }

		/// <summary>Gets or sets the information about the meeting's screenshare.</summary>
		public ScreenshareDetails SharingDetails { get; init; }
	}
}
