using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of recording file.
	/// </summary>
	public enum RecordingFileType
	{
		/// <summary>Not specified.</summary>
		[EnumMember(Value = "")]
		NotSpecified,

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

		/// <summary>File containing closed captions of the recording in VTT file format.</summary>
		[EnumMember(Value = "cc")]
		ClosedCaptioning,

		/// <summary>File containing polling data in csv format.</summary>
		[EnumMember(Value = "csv")]
		PollingData,

		/// <summary>Summary file of the recording in JSON file format.</summary>
		[EnumMember(Value = "summary")]
		Summary,

		/// <summary>
		/// A JSON file encoded in base64 format containing chat messages.
		/// The file also includes waiting room chats, deleted messages, meeting emojis and non-verbal feedback.
		/// </summary>
		[EnumMember(Value = "chat_message")]
		ChatMessage,

		/// <summary>
		/// A JSON file containing records of members entering and leaving the subgroup.
		/// </summary>
		[EnumMember(Value = "sub_group_member_log")]
		SubGroupMemberLog,

		/// <summary>
		/// A JSON file containing internal user archive AI companion content.
		/// </summary>
		[EnumMember(Value = "aic_conversation")]
		AicConversation,
	}
}
