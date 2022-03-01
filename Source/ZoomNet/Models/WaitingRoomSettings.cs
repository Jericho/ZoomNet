using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Waiting room settings.
	/// </summary>
	public class WaitingRoomSettings
	{
		/// <summary>
		/// Gets or sets the type of participants to be admitted to tge waiting room.
		/// </summary>
		[JsonProperty("participants_to_place_in_waiting_room", NullValueHandling = NullValueHandling.Ignore)]
		public ParticipantsToPlaceInWaitingRoom ParticipantsToPlaceInWaitingRoom { get; set; }

		/// <summary>
		/// Gets or sets the users who can admit participants from the Waiting Room.
		/// </summary>
		[JsonProperty("users_who_can_admit_participants_from_waiting_room", NullValueHandling = NullValueHandling.Ignore)]
		public UsersWhoCanAdmitParticipantsFromWaitingRoom UsersWhoCanAdmitParticipantsFromWaitingRoom { get; set; }

		/// <summary>
		/// Gets or sets the comma-separated list of domains that can bypass the Waiting Room.
		/// </summary>
		[JsonProperty("whitelisted_domains_for_waiting_room", NullValueHandling = NullValueHandling.Ignore)]
		public string WhitelistedDomainsForWaitingRoom { get; set; }
	}
}
