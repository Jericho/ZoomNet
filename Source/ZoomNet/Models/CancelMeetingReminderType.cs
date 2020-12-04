using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type Cancel Meeting Reminder.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CancelMeetingReminderType
	{
		/// <summary>
		/// True.
		/// </summary>
		[EnumMember(Value = "true")]
		True,

		/// <summary>
		/// False.
		/// </summary>
		[EnumMember(Value = "false")]
		False
	}
}
