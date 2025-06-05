using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room location settings type.
	/// </summary>
	public enum RoomLocationSettingsType
	{
		/// <summary>
		/// Alert settings of the Zoom Rooms located in a specific location.
		/// </summary>
		[EnumMember(Value = "alert")]
		Alert,

		/// <summary>
		/// Meeting settings of the Zoom Rooms located in a specific location.
		/// </summary>
		[EnumMember(Value = "meeting")]
		Meeting,

		/// <summary>
		/// Digital Signage settings of the Zoom Rooms located in a specific location.
		/// </summary>
		[EnumMember(Value = "signage")]
		Signage,

		/// <summary>
		/// Scheduling Display settings of the Zoom Rooms located in a specific location.
		/// </summary>
		[EnumMember(Value = "scheduling_display")]
		SchedulingDisplay
	}
}
