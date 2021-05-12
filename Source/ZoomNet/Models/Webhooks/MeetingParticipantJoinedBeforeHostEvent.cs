namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant joins a meeting before the host.
	/// </summary>
	public class MeetingParticipantJoinedBeforeHostEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
