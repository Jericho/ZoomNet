using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Ticket assigned to an event registrant.
	/// </summary>
	public class EventRegistrantTicket
	{
		/// <summary>Gets or sets the id of the registrant ticket.</summary>
		[JsonPropertyName("ticket_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		[JsonPropertyName("role")]
		public EventTicketRole Role { get; set; }

		/// <summary>
		/// Gets or sets the event authentication method for the ticket.
		/// </summary>
		[JsonPropertyName("authentication_method")]
		public EventAuthenticationMethod AuthenticationMethod { get; set; }

		/// <summary>Gets or sets the QR code Url.</summary>
		[JsonPropertyName("qr_code_url")]
		public string QrCodeUrl { get; set; }
	}
}
