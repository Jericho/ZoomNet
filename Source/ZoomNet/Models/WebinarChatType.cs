namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the groups a webinar participant can chat with.
	/// </summary>
	public enum WebinarChatType
	{
		/// <summary>Panelist can chat with Host and panelists only.</summary>
		HostAndPanelists = 1,

		/// <summary>The panelist can chat with everyone.</summary>
		Everyone = 2
	}
}
