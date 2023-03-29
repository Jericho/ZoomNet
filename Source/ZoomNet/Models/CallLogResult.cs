using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the result of the call.
	/// </summary>
	public enum CallLogResult
	{
		/// <summary>
		/// Missed.
		/// </summary>
		[EnumMember(Value = "Missed")]
		Missed,

		/// <summary>
		/// Voicemail.
		/// </summary>
		[EnumMember(Value = "Voicemail")]
		Voicemail,

		/// <summary>
		/// Call connected.
		/// </summary>
		[EnumMember(Value = "Call connected")]
		CallConnected,

		/// <summary>
		/// Rejected.
		/// </summary>
		[EnumMember(Value = "Rejected")]
		Rejected,

		/// <summary>
		/// Blocked.
		/// </summary>
		[EnumMember(Value = "Blocked")]
		Blocked,

		/// <summary>
		/// Busy.
		/// </summary>
		[EnumMember(Value = "Busy")]
		Busy,

		/// <summary>
		/// Wrong Number.
		/// </summary>
		[EnumMember(Value = "Wrong Number")]
		WrongNumber,

		/// <summary>
		/// No Answer.
		/// </summary>
		[EnumMember(Value = "No Answer")]
		NoAnswer,

		/// <summary>
		/// International Disabled.
		/// </summary>
		[EnumMember(Value = "International Disabled")]
		InternationalDisabled,

		/// <summary>
		/// Internal Error.
		/// </summary>
		[EnumMember(Value = "Internal Error")]
		InternalError,

		/// <summary>
		/// Call failed.
		/// </summary>
		[EnumMember(Value = "Call failed")]
		CallFailed,

		/// <summary>
		/// Restricted Number.
		/// </summary>
		[EnumMember(Value = "Restricted Number")]
		RestrictedNumber,

		/// <summary>
		/// Call Cancel.
		/// </summary>
		[EnumMember(Value = "Call Cancel")]
		CallCancel,

		/// <summary>
		/// Message.
		/// </summary>
		[EnumMember(Value = "Message")]
		Message,

		/// <summary>
		/// Answered by Other Member.
		/// </summary>
		[EnumMember(Value = "Answered by Other Member")]
		AnsweredByOtherMember,

		/// <summary>
		/// Call Cancelled.
		/// </summary>
		[EnumMember(Value = "Call Cancelled")]
		CallCancelled,

		/// <summary>
		/// Park.
		/// </summary>
		[EnumMember(Value = "Park")]
		Park,

		/// <summary>
		/// Parked.
		/// </summary>
		[EnumMember(Value = "Parked")]
		Parked,

		/// <summary>
		/// Blocked by non-GAL.
		/// </summary>
		[EnumMember(Value = "Blocked by non-GAL")]
		BlockedByNonGAL,

		/// <summary>
		/// Blocked by info-Barriers.
		/// </summary>
		[EnumMember(Value = "Blocked by info-Barriers")]
		BlockedByInfoBarriers,

		/// <summary>
		/// Recording Failure.
		/// </summary>
		[EnumMember(Value = "Recording Failure")]
		RecordingFailure,

		/// <summary>
		/// Recorded.
		/// </summary>
		[EnumMember(Value = "Recorded")]
		Recorded,

		/// <summary>
		/// Auto Recorded.
		/// </summary>
		[EnumMember(Value = "Auto Recorded")]
		AutoRecorded
	}
}
