using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting registrsation has been approved.
	/// </summary>
	public class MeetingRegistrationApprovedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who approved the registration.
		/// </summary>
		[JsonProperty(PropertyName = "operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who approved the registration.
		/// </summary>
		[JsonProperty(PropertyName = "operator_id")]
		public string OperatorId { get; set; }
	}
}
