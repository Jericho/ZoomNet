using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the room status.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
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
		/// UnderConstruction.
		/// </summary>
		[EnumMember(Value = "UnderConstruction")]
		UnderConstruction
	}
}
