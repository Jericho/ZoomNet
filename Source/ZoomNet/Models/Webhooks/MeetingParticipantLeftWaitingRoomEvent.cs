using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting host decides not to admit the participant to the meeting and removes the participant from the waiting room.
	/// </summary>
	public class MeetingParticipantLeftWaitingRoomEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant left the waiting room.
		/// </summary>
		public DateTime LeftOn { get; set; }

		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
