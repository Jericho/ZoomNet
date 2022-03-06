using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a user.
	/// </summary>
	public enum UserStatus
	{
		/// <summary>
		/// Active.
		/// </summary>
		[EnumMember(Value = "active")]
		Active,

		/// <summary>
		/// Inactive.
		/// </summary>
		[EnumMember(Value = "inactive")]
		Inactive,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending
	}
}
