using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a meeting participant leaves a meeting breakout room.</summary>
	public record MeetingParticipantLeftBreakoutRoomEvent : MeetingBreakoutRoomEvent
	{
		/// <summary>Gets or sets the information about meeting participant.</summary>
		public BreakoutRoomParticipantInfo Participant { get; init; }

		/// <summary>Gets or sets the time when the participant left the breakout room.</summary>
		public DateTime LeaveTime { get; init; }

		/// <summary>Gets or sets the reason why the participant left the breakout room.</summary>
		public string LeaveReason { get; init; }
	}
}
