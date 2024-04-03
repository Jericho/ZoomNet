namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate who can post to a chat channel.
	/// </summary>
	public enum ChatChannelPostingPermissions
	{
		/// <summary>
		/// All chat channel members can post to the channel.
		/// </summary>
		Everyone = 1,

		/// <summary>
		/// Only channel owners and administrators can post to the channel.
		/// </summary>
		Administrators = 2,

		/// <summary>
		/// Only channel owners, administrators, and certain members can post to the channel.
		/// </summary>
		Restricted = 3
	}
}
