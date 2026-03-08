using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact address book unit.
	/// </summary>
	public class ContactCenterAddressBookUnit
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("unit_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("unit_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the desciption.</summary>
		[JsonPropertyName("unit_description")]
		public string Description { get; set; }
	}
}
