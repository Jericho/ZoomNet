using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The type of automatically recorded calls.
	/// </summary>
	public enum AutoRecordedCallType
	{
		/// <summary>
		/// Autorecord inbound calls only.
		/// </summary>
		[EnumMember(Value = "inbound")]
		Inbound,

		/// <summary>
		/// Autorecord outbound calls only.
		/// </summary>
		[EnumMember(Value = "outbound")]
		Outbound,

		/// <summary>
		/// Autorecord both inbound and outbound calls.
		/// </summary>
		[EnumMember(Value = "both")]
		Both,
	}
}
