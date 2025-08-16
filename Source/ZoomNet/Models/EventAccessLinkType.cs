using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of access link of the event.
	/// </summary>
	public enum EventAccessLinkType
	{
		/// <summary>Attendees will be required to authenticate with the email that was used at registration when joining.</summary>
		[EnumMember(Value = "registration")]
		Registration,

		/// <summary>The attendee group specified by the chosen authentication will join without registration.</summary>
		[EnumMember(Value = "group-join")]
		GroupJoin,
	}
}
