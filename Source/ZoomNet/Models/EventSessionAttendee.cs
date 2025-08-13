using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A session attendee.
	/// </summary>
	public class EventSessionAttendee
	{
		/// <summary>Gets or sets the email address of the attendee.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the event authentication method for the ticket.
		/// </summary>
		[JsonPropertyName("authentication_method")]
		public EventAuthenticationMethod AuthenticationMethod { get; set; }
	}
}
