using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time an attendee completes an end-of-meeting experience feedback survey for a meeting.</summary>
	public record MeetingParticipantFeedbackEvent : Event
	{
		/// <summary>Gets or sets the account id of the user who commits the feedback.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the information about the meeting.</summary>
		[JsonPropertyName("object")]
		public MeetingBasicInfo Meeting { get; init; }

		/// <summary>Gets or sets the information about the meeting participant.</summary>
		public WebhookParticipant Participant { get; init; }

		/// <summary>Gets or sets the meeting participant feedback.</summary>
		public MeetingParticipantFeedback Feedback { get; init; }
	}
}
