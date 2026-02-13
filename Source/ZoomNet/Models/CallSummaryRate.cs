using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the call summary rate.
	/// </summary>
	public enum CallSummaryRate
	{
		/// <summary>Thumb up.</summary>
		[EnumMember(Value = "thumb_up")]
		ThumbUp,

		/// <summary>Thumb down.</summary>
		[EnumMember(Value = "thumb_down")]
		ThumbDown,
	}
}
