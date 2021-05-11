using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant who has already joined the meeting is sent back to the waiting room during the meeting.
	/// </summary>
	public class MeetingParticipantSentToWaitingRoomEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant was sent to the waiting room.
		/// </summary>
		public DateTime SentToWaitingRoomOn { get; set; }

		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
