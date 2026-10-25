using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Voicemail transcript information.</summary>
	public record VoicemailTranscript
	{
		/// <summary>Gets or sets the content of the voicemail transcript.</summary>
		[JsonPropertyName("content")]
		public string Content { get; init; }

		/// <summary>Gets or sets the company that provides the transcription engine technology.</summary>
		[JsonPropertyName("engine")]
		public string Engine { get; init; }

		/// <summary>Gets or sets the status of the voicemail transcript.</summary>
		[JsonPropertyName("status")]
		public VoicemailTranscriptStatus Status { get; init; }
	}
}
