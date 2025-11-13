using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a registrant is denied from viewing a cloud recording.
	/// </summary>
	public class RecordingRegistrationDeniedEvent : RecordingRegistrationEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who denied the recording registration.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user who denied the recording registration.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
