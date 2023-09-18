using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the phone call owner types.
	/// </summary>
	public enum PhoneCallRecordingOwnerType
	{
		/// <summary>
		/// user.
		/// </summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>
		/// callQueue.
		/// </summary>
		[EnumMember(Value = "call queue")]
		CallQueue
	}
}
