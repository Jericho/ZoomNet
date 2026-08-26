using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the direction of the call.</summary>
	public enum CallLogDirection
	{
		/// <summary>Inbound.</summary>
		[EnumMember(Value = "inbound")]
		Inbound,

		/// <summary>Outbound.</summary>
		[EnumMember(Value = "outbound")]
		Outbound
	}
}
