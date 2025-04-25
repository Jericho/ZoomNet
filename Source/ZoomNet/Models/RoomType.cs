using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate room type.
	/// </summary>
	public enum RoomType
	{
		/// <summary>
		/// Kiosk.
		/// </summary>
		[EnumMember(Value = "Kiosk")]
		Kiosk,

		/// <summary>
		/// Zoom room.
		/// </summary>
		[EnumMember(Value = "ZoomRoom")]
		Room,

		/// <summary>
		/// Whiteboard.
		/// </summary>
		[EnumMember(Value = "StandaloneWhiteboard")]
		Whiteboard,

		/// <summary>
		/// Scheduling display only.
		/// </summary>
		[EnumMember(Value = "SchedulingDisplayOnly")]
		SchedulingDisplayOnly,

		/// <summary>
		/// Digital signage only.
		/// </summary>
		[EnumMember(Value = "DigitalSignageOnly")]
		DigitalSignageOnly,

		/// <summary>
		/// Personal Zoom Room.
		/// </summary>
		[EnumMember(Value = "PersonalZoomRoom")]
		PersonalRoom,
	}
}
