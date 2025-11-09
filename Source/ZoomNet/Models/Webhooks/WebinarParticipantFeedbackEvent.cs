using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time an attendee completes an end-of-webinar feedback survey for a webinar.
	/// </summary>
	public class WebinarParticipantFeedbackEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who commits the feedback.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets webinar information.
		/// </summary>
		[JsonPropertyName("object")]
		public WebinarBasicInfo Webinar { get; set; }

		/// <summary>
		/// Gets or sets the information about the webinar participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }

		/// <summary>
		/// Gets or sets the webinar participant feedback.
		/// </summary>
		public MeetingParticipantFeedback Feedback { get; set; }
	}
}
