using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a poll.
	/// </summary>
	public enum PollStatusForEventSession
	{
		/// <summary>Active.</summary>
		[EnumMember(Value = "active")]
		Active,

		/// <summary>Inactive.</summary>
		[EnumMember(Value = "inactive")]
		Inactive,
	}
}
