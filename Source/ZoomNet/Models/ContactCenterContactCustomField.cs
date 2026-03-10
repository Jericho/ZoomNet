using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A contact's custom field.
	/// </summary>
	public class ContactCenterContactCustomField
	{
		/// <summary>Gets or sets the unique identifier of the customn field.</summary>
		[JsonPropertyName("custom_field_id")]
		public string FieldId { get; set; }

		/// <summary>Gets or sets the name of the custom field.</summary>
		[JsonPropertyName("custom_field_name")]
		public string FieldName { get; set; }

		/// <summary>Gets or sets the data type of the custom field.</summary>
		[JsonPropertyName("data_type")]
		public ContactCenterAddressBookCustomFieldDataType FieldDataType { get; set; }

		/// <summary>Gets or sets the contact's custom field value.</summary>
		[JsonPropertyName("custom_field_value")]
		public string Value { get; set; }

		/// <summary>Gets or sets a value indicating whether or not the value is outdated.</summary>
		[JsonPropertyName("outdated")]
		public bool IsOutdated { get; set; }
	}
}
