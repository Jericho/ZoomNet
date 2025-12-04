using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Direction of the call.
	/// </summary>
	public enum CallElementDirection
	{
		/// <summary>Inbound call.</summary>
		[EnumMember(Value = "inbound")]
		Inbound,

		/// <summary>Emergency call.</summary>
		[EnumMember(Value = "outbound")]
		Outbound,
	}
}
