using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room device.
	/// </summary>
	public class RoomDevice
	{
		/// <summary>
		/// Gets or sets the app version of Zoom Rooms.
		/// </summary>
		[JsonPropertyName("app_version")]
		public string AppVersion { get; set; }

		/// <summary>
		/// Gets or sets the operating system of the device..
		/// </summary>
		[JsonPropertyName("device_system")]
		public string OperatingSystem { get; set; }

		/// <summary>
		/// Gets or sets the type of the device.
		/// </summary>
		[JsonPropertyName("device_type")]
		public RoomDeviceType Type { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the Zoom Room.
		/// </summary>
		[JsonPropertyName("room_name")]
		public string RoomName { get; set; }

		/// <summary>
		/// Gets or sets the status of the device.
		/// </summary>
		[JsonPropertyName("status")]
		public RoomDeviceStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the device's MAC addresses.
		/// </summary>
		[JsonPropertyName("device_mac_addresses")]
		public string[] MacAddresses { get; set; }

		/// <summary>
		/// Gets or sets the device's name.
		/// </summary>
		[JsonPropertyName("device_hostname")]
		public string HostName { get; set; }

		/// <summary>
		/// Gets or sets the device's manufacturer.
		/// </summary>
		[JsonPropertyName("device_manufacturer")]
		public string Manufacturer { get; set; }

		/// <summary>
		/// Gets or sets the device's model.
		/// </summary>
		[JsonPropertyName("device_model")]
		public string Model { get; set; }

		/// <summary>
		/// Gets or sets the device's firmware.
		/// </summary>
		[JsonPropertyName("device_firmware")]
		public string Firmware { get; set; }

		/// <summary>
		/// Gets or sets the device's IP address.
		/// </summary>
		[JsonPropertyName("ip_address")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets the device's serial number.
		/// </summary>
		[JsonPropertyName("serial_number")]
		public string SerialNumber { get; set; }
	}
}
