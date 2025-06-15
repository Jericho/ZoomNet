using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting for a single session event.
	/// </summary>
	public enum EventMeetingType
	{
		/// <summary>Meeting.</summary>
		[EnumMember(Value = "MEETING")]
		Meeting,

		/// <summary>Webinar.</summary>
		[EnumMember(Value = "WEBINAR")]
		Webinar
	}
}
