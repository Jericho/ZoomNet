using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Whether to play the recording beep tone all participants in the call or only the recording user.
	/// </summary>
	public enum PlayBeepMember
	{
		/// <summary>
		/// Play the recording beep tone for all participants in the call.
		/// </summary>
		[EnumMember(Value = "allMembers")]
		AllMembers,

		/// <summary>
		/// Play the recording beep tone only for the recording user in the call.
		/// </summary>
		[EnumMember(Value = "recordingUser")]
		RecordingUser,
	}
}
