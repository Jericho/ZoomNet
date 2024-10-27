using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of the call.
	/// </summary>
	public enum CallLogTimeType
	{
		/// <summary>
		/// Enables you to search call logs by start time.
		/// </summary>
		[EnumMember(Value = "startTime")]
		StartTime,

		/// <summary>
		/// Enables you to search call logs by end time.
		/// </summary>
		[EnumMember(Value = "endTime")]
		EndTime
	}
}
