namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the groups a given paticipant can chat with.
	/// </summary>
	public enum ParticipantChatType
	{
		/// <summary>The participant cannot use chat.</summary>
		Disallowed = 1,

		/// <summary>Participant can chat with Host and co-hosts only.</summary>
		HostAndCoHosts = 2,

		/// <summary>The participant can chat with other participants publicly.</summary>
		ParticipantsPublicly = 3,

		/// <summary>The participant can chat with other participants publicly and privately.</summary>
		ParticipantsPrivately = 4
	}
}
