using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Device.
	/// </summary>
	public class Device
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the room name.
		/// </summary>
		[JsonProperty("room_name")]
		public string RoomName { get; set; }

		/// <summary>
		/// Gets or sets the device type.
		/// </summary>
		[JsonProperty("device_type")]
		public DeviceType DeviceType { get; set; }

		/// <summary>
		/// Gets or sets the version of the Zoom Room application.
		/// </summary>
		[JsonProperty(PropertyName = "app_version")]
		public string AppVersion { get; set; }

		/// <summary>
		/// Gets or sets the operating system of the device.
		/// </summary>
		[JsonProperty("device_system")]
		public string DeviceSystem { get; set; }

		/// <summary>
		/// Gets or sets the device status.
		/// </summary>
		[JsonProperty("status")]
		public DeviceStatus Status { get; set; }
	}
}
