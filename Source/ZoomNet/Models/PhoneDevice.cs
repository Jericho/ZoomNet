using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone device information.
	/// </summary>
	public class PhoneDevice
	{
		/// <summary>
		/// Gets or sets device id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets device manufacturer and model name.
		/// </summary>
		[JsonPropertyName("device_type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets device name.
		/// </summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets MAC address or serial number.
		/// </summary>
		[JsonPropertyName("mac_address")]
		public string MacAddress { get; set; }

		/// <summary>
		/// Gets or sets the provisioning information of a device.
		/// </summary>
		[JsonPropertyName("provision")]
		public DeviceProvisioning Provision { get; set; }

		/// <summary>
		/// Gets or sets the site to assign the phone user to.
		/// </summary>
		[JsonPropertyName("site")]
		public Site Site { get; set; }
	}
}
