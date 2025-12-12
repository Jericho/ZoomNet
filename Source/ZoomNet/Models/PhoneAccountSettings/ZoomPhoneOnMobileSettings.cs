using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to use Zoom Phone on mobile clients.
	/// </summary>
	public class ZoomPhoneOnMobileSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets value indicating whether calling and SMS or MMS functions on mobile are allowed.
		/// </summary>
		[JsonPropertyName("allow_calling_sms_mms")]
		public bool? AllowCallingSmsMms { get; set; }

		/// <summary>
		/// Gets or sets the clients that are allowed to make and receive calls.
		/// </summary>
		[JsonPropertyName("allow_calling_clients")]
		public MobileClientType[] AllowCallingClients { get; set; }

		/// <summary>
		/// Gets or sets the clients that are allowed to use SMS/MMS.
		/// </summary>
		[JsonPropertyName("allow_sms_mms_clients")]
		public MobileClientType[] AllowSmsMmsClients { get; set; }
	}
}
