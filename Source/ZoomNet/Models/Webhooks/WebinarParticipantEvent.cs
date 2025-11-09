namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a webhook participant.
	/// </summary>
	public abstract class WebinarParticipantEvent : WebinarInfoEvent
	{
		/// <summary>
		/// Gets or sets the participant information.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
