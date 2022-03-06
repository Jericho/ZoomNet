using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar is updated.
	/// </summary>
	public class WebinarUpdatedEvent : Event
	{
		/// <summary>
		/// Gets or sets the email address of the user who updated the webinar.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the operator who updated the webinar.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }

		/// <summary>
		/// Gets or sets the fields that have been modified.
		/// </summary>
		public (string FieldName, object OldValue, object NewValue)[] ModifiedFields { get; set; }
	}
}
