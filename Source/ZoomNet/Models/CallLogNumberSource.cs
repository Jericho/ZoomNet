using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the source of callee/caller number.
	/// </summary>
	public enum CallLogNumberSource
	{
		/// <summary>
		/// internal — ZP native.
		/// </summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>
		/// external — BYOC or Provider Exchange.
		/// </summary>
		[EnumMember(Value = "external")]
		External,

		/// <summary>
		/// byop — Premise peering. Not available when number_type = 1.
		/// </summary>
		[EnumMember(Value = "byop")]
		Byop
	}
}
