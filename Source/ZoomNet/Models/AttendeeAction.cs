using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Attendee actions performed by host for an event.</summary>
	public record AttendeeAction
	{
		/// <summary>Gets or sets the action name.</summary>
		[JsonPropertyName("action")]
		public string Action { get; init; }

		/// <summary>Gets or sets the email address of the checked-in attendee.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the The source of the action. Any information that can be used for tracking the source of this action.</summary>
		[JsonPropertyName("source")]
		public string Source { get; init; }
	}
}
