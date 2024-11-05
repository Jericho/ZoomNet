using System.Runtime.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// The ring mode types.
	/// </summary>
	public enum RingModeType
	{
		/// <summary>
		/// Simultaneous mode.
		/// </summary>
		[EnumMember(Value = "simultaneous")]
		Simultaneous,

		/// <summary>
		/// Sequential mode.
		/// </summary>
		[EnumMember(Value = "sequential")]
		Sequential,

		/// <summary>
		/// Rotating mode.
		/// </summary>
		[EnumMember(Value = "rotating")]
		Rotating,

		/// <summary>
		/// Longest idle mode.
		/// </summary>
		[EnumMember(Value = "longest_idle")]
		LongestIdle,
	}
}
