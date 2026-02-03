using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom phone external contact details model.
	/// </summary>
	public class ExternalContactDetails : ExternalContact
	{
		/// <summary>
		/// Gets or sets the external contact's description.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the external contact's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the external contact's extension number.
		/// </summary>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; set; }

		/// <summary>
		/// Gets or sets the customer-configured external contact ID.
		/// If it is not set id will be generated automatically.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the external contact's phone numbers.
		/// </summary>
		[JsonPropertyName("phone_numbers")]
		public List<string> PhoneNumbers { get; set; }

		/// <summary>
		/// Gets or sets the external contact's SIP group, to define the call routing path. This is for customers that use SIP trunking.
		/// </summary>
		[JsonPropertyName("routing_path")]
		public string RoutingPath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow the automatic call recording.
		/// </summary>
		[JsonPropertyName("auto_call_recorded")]
		public bool AutoCallRecorded { get; set; }

		/// <summary>Gets or sets the profile picture download url.</summary>
		[JsonPropertyName("profile_picture_download_url")]
		public string ProfilePictureUrl { get; set; }

		/// <summary>Gets or sets a value indicating whether to treat this contact as an internal extension.</summary>
		[JsonPropertyName("enable_internal_extension")]
		public bool TreatAsInternalExtension { get; set; }
	}
}
