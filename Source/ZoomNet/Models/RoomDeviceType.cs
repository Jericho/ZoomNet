using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate room device type.
	/// </summary>
	public enum RoomDeviceType
	{
		/// <summary>
		/// Zoom Rooms Computer.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer")]
		Computer,

		/// <summary>
		/// Controller.
		/// </summary>
		[EnumMember(Value = "Controller")]
		Controller,

		/// <summary>
		/// Scheduling Display.
		/// </summary>
		[EnumMember(Value = "Scheduling Display")]
		SchedulingDisplay,

		/// <summary>
		/// Control system.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Control System")]
		ControlSystem,

		/// <summary>
		/// Whiteboard.
		/// </summary>
		[EnumMember(Value = "Companion Whiteboard")]
		Whiteboard
	}
}
