using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a webinar is created.</summary>
	public record WebinarCreatedEvent : WebinarEvent
	{
		/// <summary>Gets or sets the operation (allowed values: all, single).</summary>
		[JsonPropertyName("operation")]
		public string Operation { get; init; }

		/// <summary>Gets or sets the email address of the user who created the webinar.</summary>
		[JsonPropertyName("operator")]
		public string Operator { get; init; }

		/// <summary>Gets or sets the user ID of the operator who created the webinar.</summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; init; }

		/// <summary>Gets or sets the platform through which the webinar was created.</summary>
		public CreationSource CreationSource { get; init; }
	}
}
