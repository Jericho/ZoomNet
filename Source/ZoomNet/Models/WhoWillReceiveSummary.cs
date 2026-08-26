namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate who will receive the summary.</summary>
	public enum WhoWillReceiveSummary
	{
		/// <summary>Only meeting host.</summary>
		OnlyMeetingHost = 1,

		/// <summary>Only meeting host, co-hosts, and alternative hosts.</summary>
		OnlyMeetingHostCoHostsAndAlternativeHosts = 2,

		/// <summary>Only meeting host and meeting invitees in our organization.</summary>
		OnlyMeetingHostAndMeetingInviteesInOurOrganization = 3,

		/// <summary>All meeting invitees including those outside of our organization.</summary>
		AllMeetingInvitees = 4
	}
}
