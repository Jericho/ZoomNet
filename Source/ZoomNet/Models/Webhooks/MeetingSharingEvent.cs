namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting sharing.
	/// </summary>
	public abstract class MeetingSharingEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the information about the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }

		/// <summary>
		/// Gets or sets the information about the screenshare.
		/// </summary>
		public ScreenshareDetails ScreenshareDetails { get; set; }
	}
}
