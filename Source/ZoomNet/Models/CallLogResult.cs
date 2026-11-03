using System.Runtime.Serialization;
using ZoomNet.Utilities;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the result of the call.</summary>
	public enum CallLogResult
	{
		// The following enum values are based on the documented possible values for:
		// - phone.caller_call_log_completed (currently undocumented, See: https://devforum.zoom.us/t/undocumented-possible-values-in-the-result-json-node-for-the-phone-caller-call-log-completed-webhook/147587)
		// - phone.callee_call_log_completed
		// - phone.caller_call_element_completed,
		// - phone.callee_call_element_completed,
		// - phone.caller_call_history_completed (deprecated)
		// - phone.callee_call_history_completed (deprecated)

		/// <summary>Answered.</summary>
		[EnumMember(Value = "answered")]
		Answered,

		/// <summary>Accepted.</summary>
		[EnumMember(Value = "accepted")]
		Accepted,

		/// <summary>Picked Up.</summary>
		[EnumMember(Value = "picked_up")]
		PickedUp,

		/// <summary>Connected.</summary>
		[EnumMember(Value = "connected")]
		Connected,

		/// <summary>Succeeded.</summary>
		[EnumMember(Value = "succeeded")]
		Succeeded,

		/// <summary>Voicemail.</summary>
		[EnumMember(Value = "voicemail")]
		Voicemail,

		/// <summary>Hang Up.</summary>
		[EnumMember(Value = "hang_up")]
		HangUp,

		/// <summary>Canceled.</summary>
		[MultipleValuesEnumMember(DefaultValue = "canceled", OtherValues = new[] { "Call Cancel", "Call Cancelled" })]
		Canceled,

		/// <summary>Call Failed.</summary>
		[MultipleValuesEnumMember(DefaultValue = "call_failed", OtherValues = new[] { "Call failed" })]
		CallFailed,

		/// <summary>Unconnected.</summary>
		[EnumMember(Value = "unconnected")]
		Unconnected,

		/// <summary>Ring Timeout.</summary>
		[EnumMember(Value = "ring_timeout")]
		RingTimeout,

		/// <summary>Overflowed.</summary>
		[EnumMember(Value = "overflowed")]
		Overflowed,

		/// <summary>No Answer.</summary>
		[EnumMember(Value = "no_answer")]
		NoAnswer,

		/// <summary>Invalid Key.</summary>
		[EnumMember(Value = "invalid_key")]
		InvalidKey,

		/// <summary>Invalid Operation.</summary>
		[EnumMember(Value = "invalid_operation")]
		InvalidOperation,

		/// <summary>Abandoned.</summary>
		[EnumMember(Value = "abandoned")]
		Abandoned,

		/// <summary>System Blocked.</summary>
		[EnumMember(Value = "system_blocked")]
		SystemBlocked,

		/// <summary>Service Unavailable.</summary>
		[EnumMember(Value = "service_unavailable")]
		ServiceUnavailable,

		/// <summary>Answered by another member.</summary>
		[MultipleValuesEnumMember(DefaultValue = "Answered by another Member", OtherValues = new[] { "Answered by Other Member" })] // "Other Member" until November 2026, "another Member" since November 2026
		AnsweredByOtherMember,

		/// <summary>Busy.</summary>
		[EnumMember(Value = "Busy")]
		Busy,

		/// <summary>Park.</summary>
		[EnumMember(Value = "Park")]
		Park,

		/// <summary>Rejected.</summary>
		[EnumMember(Value = "Rejected")]
		Rejected,

		// The following enum values were present prior to November 2026. I have left them here for backward compatibility.

		/// <summary>Missed.</summary>
		[EnumMember(Value = "Missed")]
		Missed,

		/// <summary>Call connected.</summary>
		[EnumMember(Value = "Call connected")]
		CallConnected,

		/// <summary>Blocked.</summary>
		[EnumMember(Value = "Blocked")]
		Blocked,

		/// <summary>Wrong Number.</summary>
		[EnumMember(Value = "Wrong Number")]
		WrongNumber,

		/// <summary>International Disabled.</summary>
		[EnumMember(Value = "International Disabled")]
		InternationalDisabled,

		/// <summary>Internal Error.</summary>
		[EnumMember(Value = "Internal Error")]
		InternalError,

		/// <summary>Restricted Number.</summary>
		[EnumMember(Value = "Restricted Number")]
		RestrictedNumber,

		/// <summary>Message.</summary>
		[EnumMember(Value = "Message")]
		Message,

		/// <summary>Call Cancelled.</summary>
		[EnumMember(Value = "Call Cancelled")]
		CallCancelled,

		/// <summary>Parked.</summary>
		[EnumMember(Value = "Parked")]
		Parked,

		/// <summary>Blocked by non-GAL.</summary>
		[EnumMember(Value = "Blocked by non-GAL")]
		BlockedByNonGAL,

		/// <summary>Blocked by info-Barriers.</summary>
		[EnumMember(Value = "Blocked by info-Barriers")]
		BlockedByInfoBarriers,

		/// <summary>Recording Failure.</summary>
		[EnumMember(Value = "Recording Failure")]
		RecordingFailure,

		/// <summary>Recorded.</summary>
		[EnumMember(Value = "Recorded")]
		Recorded,

		/// <summary>Auto Recorded.</summary>
		[EnumMember(Value = "Auto Recorded")]
		AutoRecorded
	}
}
