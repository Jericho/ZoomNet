using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of a room.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RoomType
	{
		/// <summary>
		/// ZoomRoom.
		/// </summary>
		[EnumMember(Value = "ZoomRoom")]
		ZoomRoom,

		/// <summary>
		/// SchedulingDisplayOnly.
		/// </summary>
		[EnumMember(Value = "SchedulingDisplayOnly")]
		SchedulingDisplayOnly,

		/// <summary>
		/// DigitalSignageOnly.
		/// </summary>
		[EnumMember(Value = "DigitalSignageOnly")]
		DigitalSignageOnly
	}
}
