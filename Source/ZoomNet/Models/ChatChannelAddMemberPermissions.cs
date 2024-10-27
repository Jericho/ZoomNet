namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate who can add new channel members.
	/// </summary>
	public enum ChatChannelAddMemberPermissions
	{
		/// <summary>
		/// All channel members can add new members.
		/// </summary>
		Everyone = 1,

		/// <summary>
		/// Only channel owners and administrators can add new members.
		/// </summary>
		Administrators = 2
	}
}
