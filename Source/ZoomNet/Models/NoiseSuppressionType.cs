using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Noise suppression type.
	/// </summary>
	public enum NoiseSuppressionType
	{
		/// <summary>
		/// Moderate noise suppression.
		/// </summary>
		[EnumMember(Value = "moderate")]
		Moderate,

		/// <summary>
		/// Aggressive noise suppression.
		/// </summary>
		[EnumMember(Value = "aggressive")]
		Aggressive,

		/// <summary>
		///  Noise suppression disabled..
		/// </summary>
		[EnumMember(Value = "none")]
		None
	}
}
