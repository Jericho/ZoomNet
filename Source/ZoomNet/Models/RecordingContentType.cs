using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of content in a recording file.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RecordingContentType
	{
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

		/// <summary>timeline.</summary>
		[EnumMember(Value = "timeline")]
		Timeline,

		/// <summary>Active speaker.</summary>
		[EnumMember(Value = "active_speaker")]
		ActiveSpeaker
	}
}
