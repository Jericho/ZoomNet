namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of channel.
	/// </summary>
	public enum ChatChannelType
	{
		/// <summary>
		/// Members must be invited to join.
		/// </summary>
		Private = 1,

		/// <summary>
		/// Members of this channel should be invited amd the members should be from the same organization.
		/// </summary>
		PrivateWithUsersInSameAccount = 2,

		/// <summary>
		/// Anyone can search for this channel and join this channel.
		/// </summary>
		Public = 3,

		/// <summary>
		/// This is an instant channel which can be created by adding members to a new chat.
		/// </summary>
		Instant = 4
	}
}
