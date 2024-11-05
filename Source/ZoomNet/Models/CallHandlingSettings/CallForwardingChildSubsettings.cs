using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// The call forwarding route settings. It's only required for the <see cref="CallForwardingSubSettings"/>.
	/// </summary>
	public class CallForwardingChildSubsettings
	{
		/// <summary>
		/// Gets or sets the external phone number's description.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		///  Gets or sets a value indicating whether to receive a call.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool? Enable { get; set; }

		/// <summary>
		/// Gets or sets the call forwarding's ID.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the external phone number in E.164 format format.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the external contact to where the call will be forwarded.
		/// </summary>
		[JsonPropertyName("external_contact")]
		public ExternalContact ExternalContact { get; set; }
	}
}
