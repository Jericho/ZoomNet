namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the role granted to a speaker.
	/// </summary>
	public enum EventSpeakerRole
	{
		/// <summary>Alternative host.</summary>
		AlternativeHost = 0,

		/// <summary>Attendee.</summary>
		Attendee = 1,

		/// <summary>Panelist.</summary>
		Panelist = 2,
	}
}
