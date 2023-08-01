using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of room settings.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SettingsType
	{
		/// <summary>
		/// Meeting.
		/// </summary>
		[EnumMember(Value = "meeting")]
		Meeting,

		/// <summary>
		/// Alert.
		/// </summary>
		[EnumMember(Value = "alert")]
		Alert
	}
}
