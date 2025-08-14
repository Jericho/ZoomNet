using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of event.
	/// </summary>
	public enum EventType
	{
		/// <summary>Multi session event.</summary>
		[EnumMember(Value = "CONFERENCE")]
		Conference,

		/// <summary>Single session event.</summary>
		[EnumMember(Value = "SIMPLE_EVENT")]
		Simple,

		/// <summary>Recurring sessions event.</summary>
		[EnumMember(Value = "RECURRING")]
		Reccuring
	}
}
