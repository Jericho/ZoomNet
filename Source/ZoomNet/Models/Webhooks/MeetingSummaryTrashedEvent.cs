using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time one of app users or account users temporarily deletes a meeting summary.
	/// </summary>
	public class MeetingSummaryTrashedEvent : MeetingSummaryEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the meeting summary.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the meeting summary.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
