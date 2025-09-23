namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the opt in / opt out status of an agent.
	/// </summary>
	public enum ContactCenterQueueOptinStatus
	{
		/// <summary>Opted in by agent himself or supervisor.</summary>
		OptedIn = 0,

		/// <summary>Opted out by supervisor or admin.</summary>
		OptedOutBySupervisor = 1,

		/// <summary>Opted out by agent himself.</summary>
		OptedOutByAgent = 2,
	}
}
