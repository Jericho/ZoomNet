using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting AI companion.
	/// </summary>
	public class MeetingAiCompanionEvent : Event
	{
		/// <summary>
		/// Gets or sets the account ID of the user that hosted the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting information.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAiCompanionMeetingInfo Meeting { get; set; }
	}
}
