using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Telephony user settings.
	/// </summary>
	public class TelephonyUserSettings
	{
		/// <summary>Gets or sets the information about the third party audio service.</summary>
		[JsonPropertyName("audio_conference_info")]
		public string ThirdPartyAudioInfo { get; set; }

		/// <summary>Gets or sets a value indicating whether to show the international numbers link on the invitation email.</summary>
		[JsonPropertyName("show_international_numbers_link")]
		public bool ShowInternationalNumbersInInvitation { get; set; }

		/// <summary>Gets or sets where most of the participants call into or call from during a meeting.</summary>
		[JsonPropertyName("telephony_regions")]
		public TelephonyRegionsSettings TelephonyRegions { get; set; }

		/// <summary>Gets or sets a value indicating whether third party audio is permitted.</summary>
		[JsonPropertyName("third_party_audio")]
		public bool AllowThirdPartyAudio { get; set; }
	}
}
