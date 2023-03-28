using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the direction of the call.
	/// </summary>
	public enum CallLogDirection
	{
		/// <summary>
		/// inbound.
		/// </summary>
		[EnumMember(Value = "inbound")]
		Inbound,

		/// <summary>
		/// outbound.
		/// </summary>
		[EnumMember(Value = "outbound")]
		Outbound
	}
}
