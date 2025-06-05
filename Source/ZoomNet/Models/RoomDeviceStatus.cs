using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate room device status.
	/// </summary>
	public enum RoomDeviceStatus
	{
		/// <summary>
		/// Online.
		/// </summary>
		[EnumMember(Value = "Online")]
		Online,

		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "Offline")]
		Offline
	}
}
