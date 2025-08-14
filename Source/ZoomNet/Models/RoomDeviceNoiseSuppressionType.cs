using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate a room device's noise cancellation setting.
	/// </summary>
	public enum RoomDeviceNoiseSuppressionType
	{
		/// <summary>
		/// Moderate.
		/// </summary>
		[EnumMember(Value = "moderate")]
		Moderate,

		/// <summary>
		/// Aggressive.
		/// </summary>
		[EnumMember(Value = "aggressive")]
		Aggressive,

		/// <summary>
		/// none.
		/// </summary>
		[EnumMember(Value = "none")]
		None
	}
}
