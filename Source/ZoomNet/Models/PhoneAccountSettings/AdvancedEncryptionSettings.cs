using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings related to advanced encryption:
	/// - whether to allow voicemail to be encrypted with keys which are not accessible to Zoom servers. These voicemails can be decrypted only by the intended user recipient.
	/// </summary>
	public class AdvancedEncryptionSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to disable incoming unencrypted voicemail.
		/// </summary>
		[JsonPropertyName("disable_incoming_unencrypted_voicemail")]
		public bool? DisableIncomingUnencryptedVoicemail { get; set; }
	}
}
