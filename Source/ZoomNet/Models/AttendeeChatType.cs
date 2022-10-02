namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the groups a given attendee can chat with.
	/// </summary>
	public enum AttendeeChatType
	{
		/// <summary> The attendee cannot use chat.</summary>
		Disallowed = 1,

		/// <summary>Attendee can chat with Host and panelists only.</summary>
		HostAndPanelists = 2,

		/// <summary>The attendee can chat with everyone.</summary>
		Everyone = 3
	}
}
