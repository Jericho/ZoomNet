namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of a deeplink.
	/// </summary>
	public enum DeeplinkType
	{
		/// <summary>Generate a deeplink that opens Zoom App in-meeting if the meeting is in progress, if not opens Zoom App in Apps Tab.</summary>
		Meeting = 0,

		/// <summary>Generate a deeplink that refreshes Chat App Webview.</summary>
		Chat = 1,
	}
}
