using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a user permanently deletes a cloud recording.</summary>
	public record RecordingDeletedEvent : RecordingFilesEvent
	{
		/// <summary>Gets or sets the email address of the user who deleted the recording.</summary>
		[JsonPropertyName("operator")]
		public string Operator { get; init; }

		/// <summary>Gets or sets the id of the user who deleted the recording.</summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; init; }
	}
}
