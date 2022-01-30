using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a participant.
	/// </summary>
	public enum ParticipantStatus
	{
		/// <summary>
		/// In a meeting.
		/// </summary>
		[EnumMember(Value = "in_meeting")]
		InMeeting,

		/// <summary>
		/// In a waiting room.
		/// </summary>
		[EnumMember(Value = "in_waiting_room")]
		InWaitingRoom,
	}
}
