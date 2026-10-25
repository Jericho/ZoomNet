using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents an event related to meeting invitation.</summary>
	public abstract record MeetingInvitationEvent : Event
	{
		/// <summary>Gets or sets the unique identifier of the account in which the event occurred.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets information about the meeting.</summary>
		[JsonPropertyName("object")]
		public MeetingBasicInfo Meeting { get; init; }

		/// <summary>Gets or sets information about the invited participant.</summary>
		public InvitedParticipant Participant { get; init; }
	}
}
