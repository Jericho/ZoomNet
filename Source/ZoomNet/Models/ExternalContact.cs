using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom phone external contact model.
	/// </summary>
	public class ExternalContact
	{
		/// <summary>
		/// Gets or sets the Zoom-generated external contact Id.
		/// Don't set it on external contact creation, it will be generated automatically.
		/// </summary>
		[JsonPropertyName("external_contact_id")]
		public string ExternalContactId { get; set; }

		/// <summary>
		/// Gets or sets the external contact's username or extension display name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
