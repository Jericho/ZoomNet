using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate an agent status type.
	/// </summary>
	public enum ContactCenterAgentStatusType
	{
		/// <summary>Default.</summary>
		[EnumMember(Value = "default")]
		Default,

		/// <summary>Custom.</summary>
		[EnumMember(Value = "custom")]
		Custom,
	}
}
