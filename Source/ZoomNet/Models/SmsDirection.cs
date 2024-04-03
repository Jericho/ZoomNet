using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Direction of SMS.
	/// </summary>
	public enum SmsDirection
	{
		/// <summary>
		/// Inbound SMS.
		/// </summary>
		[EnumMember(Value = "in")]
		Inbound,

		/// <summary>
		/// Outbound SMS.
		/// </summary>
		[EnumMember(Value = "out")]
		Outbound
	}
}
