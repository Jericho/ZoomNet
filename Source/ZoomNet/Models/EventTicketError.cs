using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details about an error that occured when creating a ticket for a registrant.
	/// </summary>
	public class EventTicketError
	{
		/// <summary>Gets or sets the email address used for the registration.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the error code for the failure reason.</summary>
		[JsonPropertyName("error_code")]
		public string ErrorCode { get; set; }

		/// <summary>Gets or sets the error message.</summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}
