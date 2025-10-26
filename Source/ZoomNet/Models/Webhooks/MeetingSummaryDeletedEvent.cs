using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time one of app users or account users permanently deletes a meeting summary.
	/// </summary>
	public class MeetingSummaryDeletedEvent : MeetingSummaryEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who permanently deleted the meeting summary.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who permanently deleted the meeting summary.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
