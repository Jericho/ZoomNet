using Newtonsoft.Json;

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
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		[JsonProperty("email")]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonProperty("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonProperty("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the presence status.
		/// </summary>
		[JsonProperty("presence_status")]
		public PresenceStatus PresenceStatus { get; set; }

		/// <summary>
		/// Gets or sets the direct number.
		/// </summary>
		[JsonProperty("direct_number")]
		public string DirectNumber { get; set; }

		/// <summary>
		/// Gets or sets the extension.
		/// </summary>
		[JsonProperty("extension_number")]
		public string ExtensionNumber { get; set; }
	}
}
