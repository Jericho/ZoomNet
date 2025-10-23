using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant joins a meeting breakout room.
	/// </summary>
	public class MeetingParticipantJoinedBreakoutRoomEvent : MeetingBreakoutRoomEvent
	{
		/// <summary>
		/// Gets or sets the information about meeting participant.
		/// </summary>
		public BreakoutRoomParticipantInfo Participant { get; set; }

		/// <summary>
		/// Gets or sets the time when the participant joined the breakout room.
		/// </summary>
		public DateTime JoinTime { get; set; }
	}
}
