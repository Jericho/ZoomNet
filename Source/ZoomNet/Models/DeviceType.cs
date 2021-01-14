using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of a device.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DeviceType
	{
		/// <summary>
		/// ZoomRoomsComputer.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer")]
		ZoomRoomsComputer,

		/// <summary>
		/// Controller.
		/// </summary>
		[EnumMember(Value = "Controller")]
		Controller,

		/// <summary>
		/// SchedulingDisplay.
		/// </summary>
		[EnumMember(Value = "Scheduling Display")]
		SchedulingDisplay,

		/// <summary>
		/// ZoomRoomsControlSystem.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Control System")]
		ZoomRoomsControlSystem,

		/// <summary>
		/// CompanionWhiteboard.
		/// </summary>
		[EnumMember(Value = "Companion Whiteboard")]
		CompanionWhiteboard
	}
}
