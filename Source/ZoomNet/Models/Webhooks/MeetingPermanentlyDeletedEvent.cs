using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting is permanently deleted.
	/// </summary>
	public class MeetingPermanentlyDeletedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the meeting.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the meeting.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the operation.
		/// While permanently deleting a **recurring** meeting, users will have the option to recover either a single occurrence of the meeting or all occurrences of the meeting.
		/// The value of this field can be one of the following:
		/// - `all`: All occurrences of the recurring meeting were permanently deleted.
		/// - `single`: Only one occurrence of the recurring meeting was deleted.
		/// </summary>
		[JsonPropertyName("operation")]
		public string Operation { get; set; }
	}
}
