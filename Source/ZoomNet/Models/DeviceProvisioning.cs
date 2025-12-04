using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Provisioning information of a device.
	/// </summary>
	public class DeviceProvisioning
	{
		/// <summary>
		/// Gets or sets the provisioning type.
		/// </summary>
		[JsonPropertyName("type")]
		public DeviceProvisioningType Type { get; set; }

		/// <summary>
		/// Gets or sets the provisioning url.
		/// </summary>
		/// <remarks>
		/// Applicable only for <see cref="DeviceProvisioningType.Assisted"/> provisioning type.
		/// </remarks>
		[JsonPropertyName("url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the SIP account details registered during the device provisioning process.
		/// </summary>
		/// <remarks>
		/// Applicable only for <see cref="DeviceProvisioningType.Manual"/> provisioning type.
		/// </remarks>
		[JsonPropertyName("sip_accounts")]
		public SipAccount[] SipAccounts { get; set; }
	}
}
