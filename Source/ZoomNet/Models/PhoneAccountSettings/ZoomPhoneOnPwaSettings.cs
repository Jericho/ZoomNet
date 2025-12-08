using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to use Zoom Phone on Zoom Progressive Web App.
	/// </summary>
	public class ZoomPhoneOnPwaSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether calling on Zoom PWA is allowed.
		/// </summary>
		[JsonPropertyName("allow_calling")]
		public bool? AllowCalling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether using SMS/MMS on Zoom PWA is allowed.
		/// </summary>
		[JsonPropertyName("allow_sms_mms")]
		public bool? AllowSmsMms { get; set; }
	}
}
