using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Settings that allow to control voicemail and videomail email notifications.</summary>
	public record VoicemailNotificationByEmailSettings : SettingsGroupBase
	{
		/// <summary>Gets or sets a value indicating whether to forward the voicemail to email.</summary>
		[JsonPropertyName("forward_voicemail_to_email")]
		public bool? ForwardVoicemailToEmail { get; init; }

		/// <summary>Gets or sets a value indicating whether to include the voicemail file.</summary>
		[JsonPropertyName("include_voicemail_file")]
		public bool? IncludeVoicemailFile { get; init; }

		/// <summary>Gets or sets a value indicating whether to include the voicemail transcription.</summary>
		[JsonPropertyName("include_voicemail_transcription")]
		public bool? IncludeVoicemailTranscription { get; init; }
	}
}
