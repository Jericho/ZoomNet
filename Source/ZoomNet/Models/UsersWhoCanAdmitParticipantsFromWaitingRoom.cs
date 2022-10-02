namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to specify what type of users can admit participants from the Waiting Room.
	/// </summary>
	public enum UsersWhoCanAdmitParticipantsFromWaitingRoom
	{
		/// <summary>Host and co-hosts only.</summary>
		HostAndCoHosts = 0,

		/// <summary>Host, co-hosts, and anyone who bypassed the Waiting Room if the host and co-hosts are not present.</summary>
		HostAndCoHostsAndWaitingRoomBypass = 1
	}
}
