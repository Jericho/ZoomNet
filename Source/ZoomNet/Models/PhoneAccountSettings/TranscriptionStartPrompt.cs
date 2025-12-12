using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings related to playing a prompt to call participants when the transcription has started.
	/// </summary>
	public class TranscriptionStartPrompt
	{
		/// <summary>
		/// Gets or sets a value indicating whether playing a prompt to call participants when the transcription starts is enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the audio prompt file id.
		/// </summary>
		/// <remarks>
		/// If the audio was removed from the user's audio library, it will be marked with a prefix, 'removed_vWby3OZaQlS1nAdmEAqgwA' for example.
		/// </remarks>
		[JsonPropertyName("audio_id")]
		public string AudioId { get; set; }

		/// <summary>
		/// Gets or sets the audio prompt file name.
		/// </summary>
		[JsonPropertyName("audio_name")]
		public string AudioName { get; set; }
	}
}
