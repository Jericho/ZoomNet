using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the device status.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DeviceStatus
	{
		/// <summary>
		/// Offline.
		/// </summary>
		[EnumMember(Value = "Offline")]
		Offline,

		/// <summary>
		/// Online.
		/// </summary>
		[EnumMember(Value = "Online")]
		Online
	}
}
