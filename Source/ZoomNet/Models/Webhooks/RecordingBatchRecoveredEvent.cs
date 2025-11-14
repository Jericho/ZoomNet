using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user recovers one or more cloud recordings from the trash.
	/// </summary>
	public class RecordingBatchRecoveredEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who recovered the recording.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who recovered the recording.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the information about meeting or webinar recordings.
		/// </summary>
		public RecordingsBatch[] Meetings { get; set; }
	}
}
