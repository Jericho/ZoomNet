using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting metrics are being returned for.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DashboardMeetingType
	{
		/// <summary>
		/// Live.
		/// </summary>
		[EnumMember(Value = "live")]
		Live,

		/// <summary>
		/// Past
		/// </summary>
		[EnumMember(Value = "past")]
		Past,

		/// <summary>
		/// PastOne
		/// </summary>
		[EnumMember(Value = "pastOne")]
		PastOne
	}
}
