using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A sub status of the 'Not Ready' or 'Opt-out' agent status.
	/// </summary>
	public class ContactCenterAgentNotReadyReason : ContactCenterSystemStatus
	{
		/// <summary>Gets or sets a value indicating whether the agent can set the status as a sub-status for Not Ready or Queue Opt Out.</summary>
		/// <remarks>Only applicable to a status with the queue_opt_out_and_not_ready_status category.</remarks>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the list of queues that are using the 'Not Ready' or opt out of queue status.</summary>
		[JsonPropertyName("status_assigned_queues")]
		public (string Id, string Name)[] AssignedQueues { get; set; }
	}
}
