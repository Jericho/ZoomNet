namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant is waiting fpor the host to join a meeting.
	/// </summary>
	public class MeetingParticipantWaitingForHostEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
