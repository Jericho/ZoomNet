using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of attendee experience for an event.
	/// </summary>
	public enum EventAttendanceType
	{
		/// <summary>Virtual attendees only.</summary>
		[EnumMember(Value = "virtual")]
		Virtual,

		/// <summary>In-person attendees only.</summary>
		[EnumMember(Value = "in-person")]
		InPerson,

		/// <summary>oth in-person and virtual attendees.</summary>
		[EnumMember(Value = "hybrid")]
		Hybrid
	}
}
