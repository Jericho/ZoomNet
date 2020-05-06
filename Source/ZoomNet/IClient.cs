using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Gets the resource which allows you to manage meetings.
		/// </summary>
		/// <value>
		/// The meetings resource.
		/// </value>
		IMeetings Meetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings that occured in the past.
		/// </summary>
		/// <value>
		/// The past meetings resource.
		/// </value>
		IPastMeetings PastMeetings { get; }
	}
}
