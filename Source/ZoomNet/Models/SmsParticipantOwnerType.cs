using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS history owner type.
	/// </summary>
	public enum SmsParticipantOwnerType
	{
		/// <summary>
		/// user.
		/// </summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>
		/// callQueue.
		/// </summary>
		[EnumMember(Value = "callQueue")]
		CallQueue,

		/// <summary>
		/// autoReceptionist.
		/// </summary>
		[EnumMember(Value = "autoReceptionist")]
		AutoReceptionist,

		/// <summary>
		/// sharedLineGroup.
		/// </summary>
		[EnumMember(Value = "sharedLineGroup")]
		SharedLineGroup
	}
}
