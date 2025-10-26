using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time one of app users or account users shares a meeting summary.
	/// </summary>
	public class MeetingSummarySharedEvent : MeetingSummaryEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who shared the meeting summary.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who shared the meeting summary.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets information about shared users.
		/// </summary>
		[JsonPropertyName("share_with_users")]
		public SharedUser[] ShareWithUsers { get; set; }
	}
}
