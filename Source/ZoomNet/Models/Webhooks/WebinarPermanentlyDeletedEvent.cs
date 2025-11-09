using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a scheduled webinar is permanently deleted.
	/// </summary>
	public class WebinarPermanentlyDeletedEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the email address of the user who deleted the webinar.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who deleted the webinar.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the operation.
		/// While permanently deleting a **recurring** webinar, users will have the option to delete either a single occurrence of the webinar or all occurrences.
		/// The value of this field can be one of the following:
		/// - `all`: All occurrences of the recurring webinar were permanently deleted.
		/// - `single`: Only one occurrence of the recurring webinar was deleted.
		/// </summary>
		[JsonPropertyName("operation")]
		public string Operation { get; set; }
	}
}
