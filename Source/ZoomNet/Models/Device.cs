using System.Text.Json.Serialization;

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
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the device name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the device protocol.
		/// </summary>
		[JsonPropertyName("protocol")]
		public Protocol Protocol { get; set; }

		/// <summary>
		/// Gets or sets the IP address of the device.
		/// </summary>
		[JsonPropertyName("ip")]
		public string IpAddress { get; set; }

		/// <summary>
		/// Gets or sets the encryption.
		/// </summary>
		[JsonPropertyName("encryption")]
		public Encryption Encryption { get; set; }
	}
}
