using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate an agent status category.
	/// </summary>
	public enum ContactCenterAgentStatusCategory
	{
		/// <summary>System status.</summary>
		[EnumMember(Value = "agent_status")]
		SystemStatus,

		/// <summary>Reason for a 'Opt-Out' or 'Not Ready' status.</summary>
		[EnumMember(Value = "queue_opt_out_and_not_ready_status")]
		NotReadyReason,
	}
}
