namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to webinar sharing.
	/// </summary>
	public abstract class WebinarSharingEvent : WebinarParticipantEvent
	{
		/// <summary>
		/// Gets or sets the information about the screenshare.
		/// </summary>
		public ScreenshareDetails ScreenshareDetails { get; set; }
	}
}
