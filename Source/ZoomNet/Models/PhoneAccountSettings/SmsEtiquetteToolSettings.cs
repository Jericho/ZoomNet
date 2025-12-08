using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to define keywords and text patterns over SMS and prevents users from sharing unwanted messages.
	/// </summary>
	public class SmsEtiquetteToolSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets SMS etiquette policies.
		/// </summary>
		[JsonPropertyName("sms_etiquette_policy")]
		public SmsEtiquettePolicy[] SmsEtiquettePolicy { get; set; }
	}
}
