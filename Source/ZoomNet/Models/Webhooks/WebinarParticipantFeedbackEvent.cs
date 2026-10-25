using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time an attendee completes an end-of-webinar feedback survey for a webinar.</summary>
	public record WebinarParticipantFeedbackEvent : Event
	{
		/// <summary>Gets or sets the account id of the user who commits the feedback.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets webinar information.</summary>
		[JsonPropertyName("object")]
		public WebinarBasicInfo Webinar { get; init; }

		/// <summary>Gets or sets the information about the webinar participant.</summary>
		public WebhookParticipant Participant { get; init; }

		/// <summary>Gets or sets the webinar participant feedback.</summary>
		public MeetingParticipantFeedback Feedback { get; init; }
	}
}
