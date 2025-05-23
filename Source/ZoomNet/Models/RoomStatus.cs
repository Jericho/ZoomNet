using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a room.
	/// </summary>
	public enum RoomStatus
	{
		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "Offline")]
		Offline,

		/// <summary>
		/// Available.
		/// </summary>
		[EnumMember(Value = "Available")]
		Available,

		/// <summary>
		/// InMeeting.
		/// </summary>
		[EnumMember(Value = "InMeeting")]
		InMeeting,

		/// <summary>
		/// Under construction.
		/// </summary>
		[EnumMember(Value = "UnderConstruction")]
		UnderConstruction
	}
}
