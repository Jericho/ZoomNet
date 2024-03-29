using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of content in a recording file.
	/// </summary>
	public enum RecordingContentType
	{
		/// <summary>Not specified.</summary>
		[EnumMember(Value = "")]
		NotSpecified,

		/// <summary>Shared screen with speaker view closed captioned.</summary>
		[EnumMember(Value = "shared_screen_with_speaker_view(CC)")]
		SharedScreenWithSpeakerViewClosedCaptioned,

		/// <summary>Shared screen with speaker view.</summary>
		[EnumMember(Value = "shared_screen_with_speaker_view")]
		SharedScreenWithSpeakerView,

		/// <summary>Shared screen with gallery view.</summary>
		[EnumMember(Value = "shared_screen_with_gallery_view")]
		SharedScreenWithGalleryView,

		/// <summary>Speaker view.</summary>
		[EnumMember(Value = "speaker_view")]
		SpeakerView,

		/// <summary>Gallery view.</summary>
		[EnumMember(Value = "gallery_view")]
		GalleryView,

		/// <summary>Shared screen.</summary>
		[EnumMember(Value = "shared_screen")]
		SharedScreen,

		/// <summary>Audio only.</summary>
		[EnumMember(Value = "audio_only")]
		AudioOnly,

		/// <summary>Audio transcript.</summary>
		[EnumMember(Value = "audio_transcript")]
		AudioTranscript,

		/// <summary>Chat file.</summary>
		[EnumMember(Value = "chat_file")]
		ChatFile,

		/// <summary>Active speaker.</summary>
		[EnumMember(Value = "active_speaker")]
		ActiveSpeaker,

		/// <summary>Poll.</summary>
		[EnumMember(Value = "poll")]
		Poll,

		/// <summary>timeline.</summary>
		[EnumMember(Value = "timeline")]
		Timeline,

		/// <summary>closed_caption.</summary>
		[EnumMember(Value = "closed_caption")]
		ClosedCaption,

		/// <summary>Audio interpretation.</summary>
		[EnumMember(Value = "audio_interpretation")]
		AudioInterpretation,

		/// <summary>Summary.</summary>
		[EnumMember(Value = "summary")]
		Summary,

		/// <summary>Summary next steps.</summary>
		[EnumMember(Value = "summary_next_steps")]
		SummaryNextSteps,

		/// <summary>Summary smart chapters.</summary>
		[EnumMember(Value = "summary_smart_chapters")]
		SummarySmartChapters,
	}
}
