namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a participant registers for a meeting.
	/// </summary>
	public class MeetingRegistrationCreatedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the registrant information.
		/// </summary>
		public Registrant Registrant { get; set; }
	}
}
