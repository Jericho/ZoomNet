using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the recording file's processing status.
	/// </summary>
	public enum RecordingStatus
	{
		/// <summary>
		/// The status of the file is unspecified.
		/// </summary>
		[EnumMember(Value = "")]
		Unspecified,

		/// <summary>
		/// The processing of the file is complete.
		/// </summary>
		[EnumMember(Value = "completed")]
		Completed,

		/// <summary>
		/// The file is processing.
		/// </summary>
		[EnumMember(Value = "processing")]
		Processing,
	}
}
