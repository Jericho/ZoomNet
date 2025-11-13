using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user permanently deletes one or more cloud recordings.
	/// </summary>
	public class RecordingBatchDeletedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the recording.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who deleted the recording.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the information about meeting or webinar recordings.
		/// </summary>
		public RecordingsBatch[] Meetings { get; set; }
	}
}
