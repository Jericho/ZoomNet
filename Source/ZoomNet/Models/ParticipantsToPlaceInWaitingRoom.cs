namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to specify what type of participants must be admitted to the Waiting room.
	/// </summary>
	public enum ParticipantsToPlaceInWaitingRoom
	{
		/// <summary>All attendees.</summary>
		Everyone = 0,

		/// <summary>Users who are not in your account.</summary>
		UsersNotInAccount = 1,

		/// <summary>Users who are not in your account and are not part of your allowed domains list.</summary>
		UsersNotInAccountAndNotWhitelisted = 2
	}
}
