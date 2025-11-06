using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant joins a waiting room prior to being admitted to the meeting.
	/// </summary>
	public class MeetingParticipantJoinedWaitingRoomEvent : MeetingParticipantEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant joined the waiting room.
		/// </summary>
		public DateTime JoinedOn { get; set; }
	}
}
