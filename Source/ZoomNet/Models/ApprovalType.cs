namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of approval for a meeting/webinar.
	/// </summary>
	public enum ApprovalType
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
