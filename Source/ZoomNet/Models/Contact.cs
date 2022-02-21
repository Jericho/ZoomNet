using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Contact.
	/// </summary>
	public class Contact
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the presence status.
		/// </summary>
		[JsonPropertyName("presence_status")]
		public PresenceStatus PresenceStatus { get; set; }

		/// <summary>
		/// Gets or sets the direct number.
		/// </summary>
		[JsonPropertyName("direct_number")]
		public string DirectNumber { get; set; }

		/// <summary>
		/// Gets or sets the extension.
		/// </summary>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; set; }
	}
}
