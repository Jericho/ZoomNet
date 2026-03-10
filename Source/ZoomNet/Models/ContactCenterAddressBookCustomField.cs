using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An address book custom field.
	/// </summary>
	public class ContactCenterAddressBookCustomField
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("custom_field_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("custom_field_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the data type.</summary>
		[JsonPropertyName("data_type")]
		public ContactCenterAddressBookCustomFieldDataType DataType { get; set; }

		/// <summary>Gets or sets the address book IDs that should be associated with the custom field.</summary>
		[JsonPropertyName("address_book_ids")]
		public string[] AddressBookIds { get; set; }

		/// <summary>Gets or sets the description.</summary>
		[JsonPropertyName("custom_field_description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the default value.</summary>
		[JsonPropertyName("default_value")]
		public string DefaultValue { get; set; }

		/// <summary>Gets or sets the list of supported values for the picklist. This is only valid when data_type is pick_list.</summary>
		[JsonPropertyName("pick_list_values")]
		public string[] PickListValues { get; set; }

		/// <summary>Gets or sets a value indicating whether or not to show the custom fields on the inbound engagement notification in the Zoom client.</summary>
		[JsonPropertyName("show_in_inbound_notification")]
		public bool ShowInInboundNotification { get; set; }

		/// <summary>Gets or sets a value indicating whether or not to show the custom fields on the profile tab in the Zoom client.</summary>
		[JsonPropertyName("show_in_profile_tab")]
		public bool ShowInProfileTab { get; set; }

		/// <summary>Gets or sets a value indicating whether or not to show the custom fields in calls transferred to Zoom Phone.</summary>
		[JsonPropertyName("show_in_transferred_calls")]
		public bool ShowInTransferredCalls { get; set; }

		/// <summary>Gets or sets a value indicating whether or not to show the custom fields in calls transferred to Zoom Phone.</summary>
		[JsonPropertyName("use_as_external_url_parameter")]
		public bool UseAsExternalUrlParameter { get; set; }

		/// <summary>Gets or sets a value indicating whether or not to use the custom fields in the consumer routing profile.</summary>
		[JsonPropertyName("use_as_routing_profile_parameter")]
		public bool UseAsRoutingProfileParameter { get; set; }
	}
}
