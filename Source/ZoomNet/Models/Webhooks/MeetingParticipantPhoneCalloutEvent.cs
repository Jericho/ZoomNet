using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting participant phone call out.
	/// </summary>
	public abstract class MeetingParticipantPhoneCalloutEvent : Event
	{
		/// <summary>
		/// Gets or sets the meeting host's account ID.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting information.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingBasicInfo Meeting { get; set; }

		/// <summary>
		/// Gets or sets the invited phone participant information.
		/// </summary>
		public InvitedPhoneParticipant Participant { get; set; }
	}
}
