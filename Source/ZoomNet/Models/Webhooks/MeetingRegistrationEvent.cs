namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting registration.
	/// </summary>
	public abstract class MeetingRegistrationEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the registrant information.
		/// </summary>
		public Registrant Registrant { get; set; }
	}
}
