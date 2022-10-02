namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate if a attendee is allowed to save webinar chats.
	/// </summary>
	public enum AttendeeChatSaveType
	{
		/// <summary> The attendee cannot save chat.</summary>
		Disallowed = 1,

		/// <summary>The attendee can only save host and panelist chats.</summary>
		HostAndPanelists = 2,

		/// <summary>Attendees can save all webinar chats.</summary>
		All = 3
	}
}
