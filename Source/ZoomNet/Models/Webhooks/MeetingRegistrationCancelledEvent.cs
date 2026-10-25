using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a meeting registration has been cancelled.</summary>
	public record MeetingRegistrationCancelledEvent : MeetingRegistrationEvent
	{
		/// <summary>Gets or sets the email address of the user who cancelled the registration.</summary>
		[JsonPropertyName("operator")]
		public string Operator { get; init; }

		/// <summary>Gets or sets the user ID of the operator who cancelled the registration.</summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; init; }
	}
}
