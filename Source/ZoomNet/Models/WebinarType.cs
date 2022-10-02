namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of webinar.
	/// </summary>
	public enum WebinarType
	{
		/// <summary>
		/// Regular webinar.
		/// </summary>
		Regular = 5,

		/// <summary>
		/// Recurring webinar with no fixed time.
		/// </summary>
		RecurringNoFixedTime = 6,

		/// <summary>
		/// Recurring webinar with fixed time.
		/// </summary>
		RecurringFixedTime = 9
	}
}
