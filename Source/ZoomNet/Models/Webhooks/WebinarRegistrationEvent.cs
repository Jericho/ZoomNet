namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents an event related to webinar registration.</summary>
	public record WebinarRegistrationEvent : WebinarEvent
	{
		/// <summary>Gets or sets the registrant information.</summary>
		public Registrant Registrant { get; init; }

		/// <summary>Gets or sets the information about webinar registration source tracking.</summary>
		public TrackingSource TrackingSource { get; init; }
	}
}
