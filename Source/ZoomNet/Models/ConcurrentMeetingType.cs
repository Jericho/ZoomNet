using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of concurrent meeting.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ConcurrentMeetingType
	{
		/// <summary>
		/// Basic.
		/// </summary>
		[EnumMember(Value = "Basic")]
		Basic,

		/// <summary>
		/// Plus.
		/// </summary>
		[EnumMember(Value = "Plus")]
		Plus,

		/// <summary>
		/// None.
		/// </summary>
		[EnumMember(Value = "None")]
		None
	}
}
