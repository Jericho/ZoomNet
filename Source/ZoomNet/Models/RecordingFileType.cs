using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of recording file.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RecordingFileType
	{
		/// <summary>Video.</summary>
		[EnumMember(Value = "mp4")]
		Video,

		/// <summary>Audio only.</summary>
		[EnumMember(Value = "m4a")]
		Audio,

		/// <summary>Timestamp file of the recording in JSON format.</summary>
		/// <remarks>To get a timeline file, the 'Add timestamp to the recording' setting must be enabled.</remarks>
		[EnumMember(Value = "timeline")]
		Timeline,

		/// <summary>Transcription of the recording in VTT format.</summary>
		[EnumMember(Value = "transcript")]
		Transcript,

		/// <summary>A text file containing in-meeting chat messages that were sent during the meeting.</summary>
		[EnumMember(Value = "chat")]
		Chat,

		/// <summary>File contains closed captions of the recording in VTT file format.</summary>
		[EnumMember(Value = "cc")]
		ClosedCaptioning
	}
}
