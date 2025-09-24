using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate how a given user is assigned to a queue.
	/// </summary>
	public enum ContactCenterQueueAssignmentType
	{
		/// <summary>Agent.</summary>
		[EnumMember(Value = "agent")]
		Agent,

		/// <summary>Supervisor.</summary>
		[EnumMember(Value = "supervisor")]
		Supervisor,

		/// <summary>Any.</summary>
		[EnumMember(Value = "any")]
		Any,

		/// <summary>Both.</summary>
		[EnumMember(Value = "both")]
		Both,
	}
}
