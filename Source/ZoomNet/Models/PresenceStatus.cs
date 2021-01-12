using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the presence status of a user.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PresenceStatus
	{
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
	}
}
