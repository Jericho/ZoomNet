namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting or webinar recording registration.
	/// </summary>
	public abstract class RecordingRegistrationEvent : RecordingMeetingOrWebinarInfoEvent
	{
		/// <summary>
		/// Gets or sets the registrant information.
		/// </summary>
		public Registrant Registrant { get; set; }
	}
}
