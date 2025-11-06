using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting participant role.
	/// </summary>
	public enum ParticipantRole
	{
		/// <summary>
		/// Participant is meeting host.
		/// </summary>
		[EnumMember(Value = "host")]
		Host,

		/// <summary>
		/// Participant is meeting co-host.
		/// </summary>
		[EnumMember(Value = "co-host")]
		CoHost,

		/// <summary>
		/// Participant is meeting attendee.
		/// </summary>
		[EnumMember(Value = "attendee")]
		Attendee,
	}
}
