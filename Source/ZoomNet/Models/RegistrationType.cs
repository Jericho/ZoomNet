namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of registration for a meeting/webinar.
	/// </summary>
	public enum RegistrationType
	{
		/// <summary>
		/// Attendees register once and can attend any of the occurrences.
		/// </summary>
		RegisterOnceAttendAll = 1,

		/// <summary>
		/// Attendees need to register for each occurrence to attend.
		/// </summary>
		RegisterForEachOccurrence = 2,

		/// <summary>
		/// Attendees register once and can choose one or more occurrence to attend.
		/// </summary>
		RegisterOnceAttendOnce = 3
	}
}
