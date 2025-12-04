using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about device location.
	/// </summary>
	public class DeviceLocation
	{
		/// <summary>
		/// Gets or sets the basic service set identifier of the device.
		/// </summary>
		[JsonPropertyName("bssid")]
		public string[] BssId { get; set; }

		/// <summary>
		/// Gets or sets the GPS location of the device.
		/// </summary>
		[JsonPropertyName("gps")]
		public string[] Gps { get; set; }

		/// <summary>
		/// Gets or sets the IP address of the device.
		/// </summary>
		[JsonPropertyName("ip")]
		public string[] IpAddress { get; set; }
	}
}
