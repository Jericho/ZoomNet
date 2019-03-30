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
	}
}
