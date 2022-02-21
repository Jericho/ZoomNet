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
		/// <remarks>Default value</remarks>
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
		/// Offline.
		/// </summary>
		[EnumMember(Value = "In_Calendar_Event")]
		InEvent,

		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "Presenting")]
		Presenting,

		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "In_A_Zoom_Meeting")]
		InMeeting,

		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "On_A_Call")]
		OnCall,
	}
}
