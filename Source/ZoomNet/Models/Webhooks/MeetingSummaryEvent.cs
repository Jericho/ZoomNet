using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting summary.
	/// </summary>
	public abstract class MeetingSummaryEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in which the event occurred.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets information about meeting summary.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookMeetingSummary MeetingSummary { get; set; }
	}
}
