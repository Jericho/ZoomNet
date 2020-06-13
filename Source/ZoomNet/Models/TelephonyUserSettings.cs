using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Telephony user settings.
	/// </summary>
	public class TelephonyUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether third party audio is permitted.
		/// </summary>
		[JsonProperty(PropertyName = "third_party_audio")]
		public bool AllowThirdPartyAudio { get; set; }

		/// <summary>
		/// Gets or sets the information about the third party audio service.
		/// </summary>
		[JsonProperty(PropertyName = "audio_conference_info")]
		public string ThirdPartyAudioInfo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the international numbers link on the invitation email.
		/// </summary>
		[JsonProperty(PropertyName = "show_international_numbers_link")]
		public bool ShowInternationalNumbersInInvitation { get; set; }
	}
}
