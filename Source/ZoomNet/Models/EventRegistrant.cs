using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Registrant to an event.
	/// </summary>
	public class EventRegistrant
	{
		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		[JsonPropertyName("registration_status")]
		public EventRegistrantStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonPropertyName("tickets")]
		public EventTicketSummary[] Tickets { get; set; }
	}
}
