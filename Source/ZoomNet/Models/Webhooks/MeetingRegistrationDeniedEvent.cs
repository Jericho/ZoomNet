using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting registration has been denied.
	/// </summary>
	public class MeetingRegistrationDeniedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who denied the registration.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who denied the registration.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the registrant information.
		/// </summary>
		public Registrant Registrant { get; set; }
	}
}
