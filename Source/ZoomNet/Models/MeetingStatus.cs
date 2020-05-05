using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a meeting.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MeetingStatus
	{
		/// <summary>
		/// Waiting.
		/// </summary>
		[EnumMember(Value = "waiting")]
		Waiting,

		/// <summary>
		/// Started.
		/// </summary>
		[EnumMember(Value = "started")]
		Started,

		/// <summary>
		/// Finished.
		/// </summary>
		[EnumMember(Value = "finished")]
		Finished
	}
}
