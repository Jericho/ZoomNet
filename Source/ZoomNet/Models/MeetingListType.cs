using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting to be listed.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MeetingListType
	{
		/// <summary>
		/// Scheduled.
		/// </summary>
		[EnumMember(Value = "scheduled")]
		Scheduled,

		/// <summary>
		/// Live.
		/// </summary>
		[EnumMember(Value = "live")]
		Live,

		/// <summary>
		/// Upcoming.
		/// </summary>
		[EnumMember(Value = "upcoming")]
		Upcoming,

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
