using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user recovers a previously-deleted webinar.
	/// </summary>
	public class WebinarRecoveredEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who recovered the meeting.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who recovered the meeting.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the operation (allowed values: all, single).
		/// </summary>
		[JsonPropertyName("operation")]
		public string Operation { get; set; }
	}
}
