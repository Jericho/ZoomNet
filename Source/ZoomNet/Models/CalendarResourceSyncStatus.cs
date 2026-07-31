using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Type of calendar integration used to schedule the meeting.
	/// </summary>
	public enum CalendarResourceSyncStatus
	{
		/// <summary>Unknown.</summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>Synched.</summary>
		[EnumMember(Value = "success")]
		Success,

		/// <summary>Not-synced.</summary>
		[EnumMember(Value = "failed")]
		Failed,
	}
}
