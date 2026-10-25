using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a meeting is created.</summary>
	public record MeetingCreatedEvent : MeetingEvent
	{
		/// <summary>Gets or sets the operation (allowed values: all, single).</summary>
		[JsonPropertyName("operation")]
		public string Operation { get; init; }

		/// <summary>Gets or sets the email address of the user who created the meeting.</summary>
		[JsonPropertyName("operator")]
		public string Operator { get; init; }

		/// <summary>Gets or sets the user ID of the operator who created the meeting.</summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; init; }
	}
}
