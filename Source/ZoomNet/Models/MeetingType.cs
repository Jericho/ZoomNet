namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting.
	/// </summary>
	public enum MeetingType
	{
		/// <summary>
		/// Instant.
		/// </summary>
		Instant = 1,

		/// <summary>
		/// Scheduled.
		/// </summary>
		Scheduled = 2,

		/// <summary>
		/// Recurring meeting with no fixed time.
		/// </summary>
		RecurringNoFixedTime = 3,

		/// <summary>
		/// Meeting was started using a Personal Meeting ID.
		/// </summary>
		Personal = 4,

		/// <summary>
		/// Recurring meeting with fixed time.
		/// </summary>
		RecurringFixedTime = 8
	}
}
