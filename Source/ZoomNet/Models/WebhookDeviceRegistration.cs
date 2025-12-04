using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Device information as provided in <see cref="Webhooks.PhoneDeviceRegistrationEvent"/>.
	/// </summary>
	public class WebhookDeviceRegistration
	{
		/// <summary>
		/// Gets or sets the device id.
		/// </summary>
		[JsonPropertyName("device_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the device name.
		/// </summary>
		[JsonPropertyName("device_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets MAC address or serial number.
		/// </summary>
		[JsonPropertyName("mac_address")]
		public string MacAddress { get; set; }
	}
}
