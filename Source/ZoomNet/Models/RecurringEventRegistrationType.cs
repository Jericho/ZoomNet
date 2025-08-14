using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the recurring event registration type.
	/// </summary>
	/// <remarks>This is applicable only for recurring events.</remarks>
	public enum RecurringEventRegistrationType
	{
		/// <summary>One registration for all the sessions.</summary>
		[EnumMember(Value = "all_sessions")]
		AllSessions,

		/// <summary>Registration only for one session at a time.</summary>
		[EnumMember(Value = "single_session")]
		SingleSession,

		/// <summary>Registration allowed one or more sessions.</summary>
		[EnumMember(Value = "multiple_sessions")]
		MultipleSessions,
	}
}
