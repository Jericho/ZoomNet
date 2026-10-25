using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Settings that control user's personal audio library.</summary>
	public record PersonalAudioLibrarySettings : SettingsGroupBase
	{
		/// <summary>Gets or sets a value indicating whether to allow music on hold customization.</summary>
		[JsonPropertyName("allow_music_on_hold_customization")]
		public bool? AllowMusicOnHoldCustomization { get; init; }

		/// <summary>Gets or sets a value indicating whether to allow voicemail and message greeting customization.</summary>
		[JsonPropertyName("allow_voicemail_and_message_greeting_customization")]
		public bool? AllowVoicemailAndMessageGreetingCustomization { get; init; }
	}
}
