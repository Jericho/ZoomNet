using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of audio available to attendees.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RecordingSharingType
	{
		/// <summary>
		/// Publicly.
		/// </summary>
		[EnumMember(Value = "publicly")]
		Publicly,

		/// <summary>
		/// Internally.
		/// </summary>
		[EnumMember(Value = "internally")]
		Internally,

		/// <summary>
		/// None.
		/// </summary>
		[EnumMember(Value = "none")]
		None
	}
}
