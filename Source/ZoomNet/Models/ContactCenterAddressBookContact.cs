using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An address book contact.
	/// </summary>
	public class ContactCenterAddressBookContact
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("contact_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the display name.</summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>Gets or sets the account number.</summary>
		[JsonPropertyName("account_number")]
		public string AccountNumber { get; set; }

		/// <summary>Gets or sets the address book unique identifier.</summary>
		[JsonPropertyName("address_book_id")]
		public string AddressBookId { get; set; }

		/// <summary>Gets or sets the name of the address book.</summary>
		[JsonPropertyName("address_book_name")]
		public string AddressBookName { get; set; }

		/// <summary>Gets or sets the name of the company.</summary>
		[JsonPropertyName("company")]
		public string Company { get; set; }

		/// <summary>Gets or sets the consumer IDs associated with the contact.</summary>
		[JsonPropertyName("consumer_ids")]
		public string[] ConsumerIds { get; set; }

		/// <summary>Gets or sets the contact's email addresses.</summary>
		[JsonPropertyName("emails")]
		public string[] EmailAddresses { get; set; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>Gets or sets the location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>Gets or sets the role.</summary>
		[JsonPropertyName("role")]
		public string Role { get; set; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/*
		/// <summary>Gets or sets the variables.</summary>
		[JsonPropertyName("variables")]
		public string Variables { get; set; }
		*/
	}
}
