using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Closed captioning settings.
	/// </summary>
	public class ClosedCaptioningSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow a live transcription service to transcribe meetings.</summary>
		[JsonPropertyName("auto_transcribing")]
		public bool AllowAutoTranscription { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to type closed captions or assign a participant or 3rd-party service to provide closed captioning.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to save closed captions or transcripts.</summary>
		[JsonPropertyName("save_caption")]
		public bool AllowParticipantsToSave { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the use of an API token to integrate with 3rd-party closed captioning services.</summary>
		[JsonPropertyName("third_party_captioning_service")]
		public bool AllowThirdPartyCaptioningService { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the viewing of full transcripts in the in-meeting side panel.</summary>
		[JsonPropertyName("view_full_transcript")]
		public bool AllowFullTranscriptViewing { get; set; }
	}
}
