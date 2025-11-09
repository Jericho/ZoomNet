using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar registration has been approved.
	/// </summary>
	public class WebinarRegistrationApprovedEvent : WebinarRegistrationEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who approved the registration.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who approved the registration.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
