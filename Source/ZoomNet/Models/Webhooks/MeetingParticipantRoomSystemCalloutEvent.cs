using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting participant phone call out from a Zoom room.
	/// </summary>
	public abstract class MeetingParticipantRoomSystemCalloutEvent : Event
	{
		/// <summary>
		/// Gets or sets the account ID of the meeting host.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about the meeting.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingRoomJoinInvitation JoinInvitation { get; set; }

		/// <summary>
		/// Gets or sets information about the invited participant.
		/// </summary>
		public InvitedRoomParticipant Participant { get; set; }
	}
}
