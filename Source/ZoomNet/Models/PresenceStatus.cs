using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the presence status of a user.
	/// </summary>
	public enum PresenceStatus
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>Default value.</remarks>
		Unknown,

		/// <summary>
		/// Do not disturb.
		/// </summary>
		[EnumMember(Value = "Do_Not_Disturb")]
		DoNotDisturb,

		/// <summary>
		/// Away.
		/// </summary>
		[EnumMember(Value = "Away")]
		Away,

		/// <summary>
		/// Available.
		/// </summary>
		[EnumMember(Value = "Available")]
		Available,

		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "Offline")]
		Offline,

		/// <summary>
		/// In calendar event.
		/// </summary>
		[EnumMember(Value = "In_A_Calendar_Event")]
		InEvent,

		/// <summary>
		/// Presenting.
		/// </summary>
		[EnumMember(Value = "Presenting")]
		Presenting,

		/// <summary>
		/// In a Zoom meeting.
		/// </summary>
		[EnumMember(Value = "In_A_Meeting")]
		InMeeting,

		/// <summary>
		/// On a call.
		/// </summary>
		[EnumMember(Value = "In_A_Call")]
		OnCall,

		/// <summary>
		/// Out of office.
		/// </summary>
		[EnumMember(Value = "Out_of_Office")]
		OutOfOffice,

		/// <summary>
		/// Busy.
		/// </summary>
		[EnumMember(Value = "Busy")]
		Busy
	}
}
