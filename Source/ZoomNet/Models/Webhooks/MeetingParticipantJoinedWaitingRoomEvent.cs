using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant joins a waiting room prior to being admitted to the meeting.
	/// </summary>
	public class MeetingParticipantJoinedWaitingRoomEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant joined the waiting room.
		/// </summary>
		public DateTime JoinedOn { get; set; }

		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
