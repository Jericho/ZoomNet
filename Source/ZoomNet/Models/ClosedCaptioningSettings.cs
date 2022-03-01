using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Closed captioning settings.
	/// </summary>
	public class ClosedCaptioningSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow a live transcription service to transcribe meetings.</summary>
		[JsonProperty(PropertyName = "auto_transcribing")]
		public bool AllowAutoTranscription { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the host to type closed captions or assign a participant or 3rd-party service to provide closed captioning.</summary>
		[JsonProperty(PropertyName = "enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to save closed captions or transcripts.</summary>
		[JsonProperty(PropertyName = "save_caption")]
		public bool AllowParticipantsToSave { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the use of an API token to integrate with 3rd-party closed captioning services.</summary>
		[JsonProperty(PropertyName = "third_party_captioning_service")]
		public bool AllowThirdPartyCaptioningService { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the viewing of full transcripts in the in-meeting side panel.</summary>
		[JsonProperty(PropertyName = "view_full_transcript")]
		public bool AllowFullTranscriptViewing { get; set; }
	}
}
