using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the source of callee/caller number.</summary>
	public enum CallLogNumberSource
	{
		/// <summary>Internal — ZP native.</summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>External — BYOC or Provider Exchange.</summary>
		[EnumMember(Value = "external")]
		External,

		/// <summary>BYOP — Premise peering. Not available when number_type = 1.</summary>
		[EnumMember(Value = "byop")]
		Byop
	}
}
