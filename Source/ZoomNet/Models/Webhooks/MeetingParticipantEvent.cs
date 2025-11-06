namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting participant.
	/// </summary>
	public abstract class MeetingParticipantEvent : MeetingInfoEvent
	{
		/// <summary>
		/// Gets or sets the participant information.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
