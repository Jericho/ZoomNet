using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow users to turn on live transcriptions for a call.
	/// </summary>
	public class CallLiveTranscriptionSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets settings related to playing a prompt to call participants when the transcription has started.
		/// </summary>
		[JsonPropertyName("transcription_start_prompt")]
		public TranscriptionStartPrompt TranscriptionStartPrompt { get; set; }
	}
}
