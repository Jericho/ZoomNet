using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user converts a meeting into a webinar.
	/// </summary>
	public class MeetingConvertedToWebinarEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who converted the meeting into a webinar.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user who converted the meeting into a webinar.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who converted the meeting into a webinar.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets information about the webinar converted from a meeting.
		/// </summary>
		[JsonPropertyName("object")]
		public WebinarSummary Webinar { get; set; }
	}
}
