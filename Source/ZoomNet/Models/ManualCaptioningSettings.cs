using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Manual captioning settings.
	/// </summary>
	public class ManualCaptioningSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow the host to manually caption or let the host assign a participant to provide manual captioning.</summary>
		[JsonPropertyName("allow_to_type")]
		public bool AllowManual { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable Zoom's live transcription feature.</summary>
		[JsonPropertyName("auto_generated_captions")]
		public bool EnableLiveTranscription { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable the viewing of full transcripts in the in-meeting side panel.</summary>
		[JsonPropertyName("full_transcript")]
		public bool EnableFullTranscriptViewing { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable manual closed captioning.</summary>
		[JsonPropertyName("manual_captions")]
		public bool EnableManualClosedCaptioning { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to save closed captions or transcripts.</summary>
		/// <remarks>If the <see cref="EnableFullTranscriptViewing"/> property is set to false, participants cannot save captions.</remarks>
		[JsonPropertyName("save_captions")]
		public bool AllowSave { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the use of an API token to integrate with 3rd-party closed captioning services.</summary>
		[JsonPropertyName("third_party_captioning_service")]
		public bool AllowThirdPartyCaptioningService { get; set; }
	}
}
