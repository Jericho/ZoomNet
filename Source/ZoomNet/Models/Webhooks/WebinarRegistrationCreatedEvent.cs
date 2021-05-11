namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a participant registers for a webinar.
	/// </summary>
	public class WebinarRegistrationCreatedEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the registrant information.
		/// </summary>
		public Registrant Registrant { get; set; }
	}
}
