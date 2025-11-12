using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Type of recordings batch trash operation.
	/// </summary>
	public enum RecordingsBatchTrashOperationType
	{
		/// <summary>
		/// User recordings trashed.
		/// </summary>
		[EnumMember(Value = "trash_user_recordings")]
		TrashUserRecordings,

		/// <summary>
		/// Account recordings trashed.
		/// </summary>
		[EnumMember(Value = "trash_account_recordings")]
		TrashAccountRecordings,
	}
}
