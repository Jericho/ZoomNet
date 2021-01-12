namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of mention.
	/// </summary>
	public enum ChatMentionType
	{
		/// <summary>
		/// Mentions a specific contact.
		/// </summary>
		Contact = 1,

		/// <summary>
		/// Members everyone in the channel.
		/// </summary>
		All = 2
	}
}
