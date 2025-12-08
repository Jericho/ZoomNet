using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to use Zoom Phone on desktop clients.
	/// </summary>
	public class ZoomPhoneOnDesktopSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets the clients that are allowed to make and receive calls.
		/// </summary>
		[JsonPropertyName("allow_calling_clients")]
		public DesktopClientType[] AllowCallingClients { get; set; }

		/// <summary>
		/// Gets or sets the clients that are allowed to use SMS/MMS.
		/// </summary>
		[JsonPropertyName("allow_sms_mms_clients")]
		public DesktopClientType[] AllowSmsMmsClients { get; set; }
	}
}
