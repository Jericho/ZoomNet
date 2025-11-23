using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Type of the call used in call element.
	/// </summary>
	public enum CallElementCallType
	{
		/// <summary>General call.</summary>
		[EnumMember(Value = "general")]
		General,

		/// <summary>Emergency call.</summary>
		[EnumMember(Value = "emergency")]
		Emergency,
	}
}
