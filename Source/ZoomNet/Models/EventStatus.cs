using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of an event.
	/// </summary>
	public enum EventStatus
	{
		/// <summary>Published.</summary>
		[EnumMember(Value = "PUBLISHED")]
		Published,

		/// <summary>Draft.</summary>
		[EnumMember(Value = "DRAFT")]
		Draft,

		/// <summary>Cancelled.</summary>
		[EnumMember(Value = "CANCELLED")]
		Cancelled,
	}
}
