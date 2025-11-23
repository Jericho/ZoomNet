using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Detail result of an event for a call log.
	/// </summary>
	public enum CallElementResult
	{
		/// <summary>Call answered.</summary>
		[EnumMember(Value = "answered")]
		Answered,

		/// <summary>Call accepted.</summary>
		[EnumMember(Value = "accepted")]
		Accepted,

		/// <summary>Parked call picked up.</summary>
		[EnumMember(Value = "picked_up")]
		PickedUp,

		/// <summary>Call connected.</summary>
		[EnumMember(Value = "connected")]
		Connected,

		/// <summary>Call succeeded.</summary>
		[EnumMember(Value = "succeeded")]
		Succeeded,

		/// <summary>Voicemail.</summary>
		[EnumMember(Value = "voicemail")]
		Voicemail,

		/// <summary>Call hang up.</summary>
		[EnumMember(Value = "hang_up")]
		HangUp,

		/// <summary>Call cancelled.</summary>
		[EnumMember(Value = "canceled")]
		Cancelled,

		/// <summary>Call failed.</summary>
		[EnumMember(Value = "call_failed")]
		CallFailed,

		/// <summary>Call unconnected.</summary>
		[EnumMember(Value = "unconnected")]
		Unconnected,

		/// <summary>Call rejected.</summary>
		[EnumMember(Value = "rejected")]
		Rejected,

		/// <summary>User busy.</summary>
		[EnumMember(Value = "busy")]
		Busy,

		/// <summary>Call ring timeout.</summary>
		[EnumMember(Value = "ring_timeout")]
		RingTimeout,

		/// <summary>Call overflowed.</summary>
		[EnumMember(Value = "overflowed")]
		Overflowed,

		/// <summary>No answer.</summary>
		[EnumMember(Value = "no_answer")]
		NoAnswer,

		/// <summary>Invalid key used.</summary>
		[EnumMember(Value = "invalid_key")]
		InvalidKey,

		/// <summary>Invalid operation.</summary>
		[EnumMember(Value = "invalid_operation")]
		InvalidOperation,

		/// <summary>Call abandoned.</summary>
		[EnumMember(Value = "abandoned")]
		Abandoned,

		/// <summary>System blocked.</summary>
		[EnumMember(Value = "system_blocked")]
		SystemBlocked,

		/// <summary>Service unavailable.</summary>
		[EnumMember(Value = "service_unavailable")]
		ServiceUnavailable,
	}
}
