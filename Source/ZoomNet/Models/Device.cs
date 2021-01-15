using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Device.
	/// </summary>
	public class Device
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the device name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the device protocol.
		/// </summary>
		[JsonProperty("protocol")]
		public Protocol Protocol { get; set; }

		/// <summary>
		/// Gets or sets the IP address of the device.
		/// </summary>
		[JsonProperty("ip")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets the encryption.
		/// </summary>
		[JsonProperty("encryption")]
		public Encryption Encryption { get; set; }
	}
}
