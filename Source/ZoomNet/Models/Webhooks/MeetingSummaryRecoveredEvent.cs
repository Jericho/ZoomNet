using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time one of app users or account users recovers a summary from the trash.
	/// </summary>
	public class MeetingSummaryRecoveredEvent : MeetingSummaryEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who recovered the meeting summary.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who recovered the meeting summary.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
