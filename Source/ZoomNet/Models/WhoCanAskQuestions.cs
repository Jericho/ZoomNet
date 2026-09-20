namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate who can ask questions.</summary>
	public enum WhoCanAskQuestions
	{
		/// <summary>All participants and invitees.</summary>
		AllParticipantsAndInvitees = 1,

		/// <summary>All participants only from when they join.</summary>
		AllParticipantsOnlyFromWhenTheyJoin = 2,

		/// <summary>Only meeting host.</summary>
		OnlyMeetingHost = 3,

		/// <summary>Participants and invitees in our organization.</summary>
		ParticipantsAndInviteesInOurOrganization = 4,

		/// <summary>Participants in our organization only from when they join.</summary>
		ParticipantsInOurOrganizationOnlyFromWhenTheyJoin = 5
	}
}
