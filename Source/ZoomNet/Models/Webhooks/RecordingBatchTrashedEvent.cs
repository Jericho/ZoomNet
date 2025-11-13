using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user temporarily deletes all of their cloud recordings.
	/// </summary>
	public class RecordingBatchTrashedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who batch deleted recordings to trash.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who batch deleted recordings to trash.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the type of batch operation.
		/// </summary>
		[JsonPropertyName("operation")]
		public RecordingsBatchTrashOperationType Operation { get; set; }

		/// <summary>
		/// Gets or sets meeting or webinar uuids which recordings were trashed.
		/// </summary>
		public string[] MeetingUuids { get; set; }
	}
}
