using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the recording archive file's processing status.
	/// </summary>
	public enum RecordingArchiveFileStatus
	{
		/// <summary>
		/// The processing of the file is completed.
		/// </summary>
		[EnumMember(Value = "completed")]
		Completed,

		/// <summary>
		/// The file is being processed.
		/// </summary>
		[EnumMember(Value = "processing")]
		Processing,

		/// <summary>
		/// The processing of the file has failed.
		/// </summary>
		[EnumMember(Value = "failed")]
		Failed,
	}
}
