using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An account call log item.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.CallLog" />
	public class AccountCallLog : CallLog
	{
		/// <summary>
		/// Gets or sets the device's private IP address if the account has the show_device_ip_for_call_log parameter set to enabled.
		/// </summary>
		/// <value>Indicates the device's private IP address.</value>
		[JsonPropertyName("device_private_ip")]
		public string DevicePrivateIp { get; set; }

		/// <summary>
		/// Gets or sets the device's public IP address if the account has the show_device_ip_for_call_log parameter set to enabled.
		/// </summary>
		/// <value>Indicates the device's public IP address.</value>
		[JsonPropertyName("device_public_ip")]
		public string DevicePublicIp { get; set; }

		/// <summary>
		/// Gets or sets the "owner" information.
		/// </summary>
		/// <value>Call "owner" information.</value>
		[JsonPropertyName("owner")]
		public CallLogOwnerInfo Owner { get; set; }
	}
}
