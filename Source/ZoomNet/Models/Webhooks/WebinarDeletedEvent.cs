using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar is deleted.
	/// </summary>
	public class WebinarDeletedEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the webinar.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the webinar.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
