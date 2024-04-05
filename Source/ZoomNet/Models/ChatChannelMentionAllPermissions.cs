namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate who can can use @all.
	/// </summary>
	public enum ChatChannelMentionAllPermissions
	{
		/// <summary>
		/// Everyone can use @all.
		/// </summary>
		Everyone = 1,

		/// <summary>
		/// Only channel owners and administrators can use @all.
		/// </summary>
		Administrators = 2,

		/// <summary>
		/// Nobody can use @all.
		/// </summary>
		Nobody = 3
	}
}
