using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A session reservation for a registrant.
	/// </summary>
	public class EventSessionReservation
	{
		/// <summary>Gets or sets the registrant's first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>Gets or sets the registrant's last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>Gets or sets the email address of the registrant.</summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; set; }
	}
}
