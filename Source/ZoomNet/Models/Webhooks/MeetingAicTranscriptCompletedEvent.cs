using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event triggers when the meeting transcript file is completed after a meeting or webinar ends.
	/// </summary>
	public class MeetingAicTranscriptCompletedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account ID of the user that hosted the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about meeting transcript.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAiCompanionMeetingTranscript MeetingTranscript { get; set; }
	}
}
