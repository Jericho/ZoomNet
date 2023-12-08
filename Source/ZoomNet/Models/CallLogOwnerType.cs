using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the owner type.
	/// </summary>
	public enum CallLogOwnerType
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
		/// commonAreaPhone.
		/// </summary>
		[EnumMember(Value = "commonAreaPhone")]
		CommonAreaPhone,

		/// <summary>
		/// sharedLineGroup.
		/// </summary>
		[EnumMember(Value = "sharedLineGroup")]
		SharedLineGroup
	}
}
