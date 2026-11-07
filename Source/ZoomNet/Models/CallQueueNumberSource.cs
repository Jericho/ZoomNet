using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the source of callee/caller number.</summary>
	public enum CallQueueNumberSource
	{
		/// <summary>Internal.</summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>External.</summary>
		[EnumMember(Value = "external")]
		External,
	}
}
