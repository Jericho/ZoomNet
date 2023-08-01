using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the layout mode.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Layout
	{
		/// <summary>
		/// Standard.
		/// </summary>
		[EnumMember(Value = "standard")]
		Standard,

		/// <summary>
		/// VideoContent.
		/// </summary>
		[EnumMember(Value = "video_content")]
		VideoContent
	}
}
