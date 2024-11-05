using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// External contact object. It's only required for <see cref="CallForwardingChildSubsettings"/>.
	/// </summary>
	public class ExternalContact
	{
		/// <summary>
		/// Gets or sets the external contact's ID.
		/// </summary>
		[JsonPropertyName("external_contact_id")]
		public string ExternalContactId { get; set; }
	}
}
