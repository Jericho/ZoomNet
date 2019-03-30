namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of approval for a meeting.
	/// </summary>
	public enum MeetingApprovalType
	{
		/// <summary>
		/// Automatically approve.
		/// </summary>
		Automatic = 0,

		/// <summary>
		/// Manually approve.
		/// </summary>
		Manual = 1,

		/// <summary>
		/// No registration required.
		/// </summary>
		None = 2
	}
}
