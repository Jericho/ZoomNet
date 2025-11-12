using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a registrant is approved to view a cloud recording.
	/// </summary>
	public class RecordingRegistrationApprovedEvent : RecordingRegistrationEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user that approved registration for the meeting or webinar recording.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets id of the user that approved registration for the meeting or webinar recording.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
