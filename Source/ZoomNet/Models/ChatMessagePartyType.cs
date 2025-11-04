using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat message sender and receiver types.
	/// </summary>
	public enum ChatMessagePartyType
	{
		/// <summary>
		/// Applicable only for recipient type - message is sent to everyone.
		/// </summary>
		[EnumMember(Value = "everyone")]
		Everyone,

		/// <summary>
		/// The message is sent or received by the meeting host.
		/// </summary>
		[EnumMember(Value = "host")]
		Host,

		/// <summary>
		/// The message is sent or received by the meeting guest.
		/// </summary>
		[EnumMember(Value = "guest")]
		Guest,

		/// <summary>
		/// Applicable only for recipient type - message is sent to a group chat.
		/// </summary>
		[EnumMember(Value = "group")]
		Group,
	}
}
