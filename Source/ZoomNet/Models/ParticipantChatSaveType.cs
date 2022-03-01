namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate if a participant is allowed to save meeting chats.
	/// </summary>
	public enum ParticipantChatSaveType
	{
		/// <summary> The participant cannot save chat.</summary>
		Disallowed = 1,

		/// <summary>Participants can only save host and co-host meeting chats.</summary>
		HostAndCoHosts = 2,

		/// <summary>Participants can save all meeting chats.</summary>
		All = 3
	}
}
