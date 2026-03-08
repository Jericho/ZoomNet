using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact address book.
	/// </summary>
	public class ContactCenterAddressBook
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("address_book_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("address_book_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the desciption.</summary>
		[JsonPropertyName("address_book_description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the unit Id.</summary>
		[JsonPropertyName("unit_id")]
		public string UnitId { get; set; }

		/// <summary>Gets or sets the unit name.</summary>
		[JsonPropertyName("unit_name")]
		public string UnitName { get; set; }
	}
}
