namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting for an event session.
	/// </summary>
	public enum EventSessionType
	{
		/// <summary>Meeting.</summary>
		Meeting = 0,

		/// <summary>Webinar.</summary>
		Webinar = 2,

		/// <summary>No webinar or meeting.</summary>
		NoWebinarOrMeeting = 4
	}
}
