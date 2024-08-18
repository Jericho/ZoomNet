using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of an app request.
	/// </summary>
	public enum AppRequestStatus
	{
		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending,

		/// <summary>
		/// Approved.
		/// </summary>
		[EnumMember(Value = "approved")]
		Approved,

		/// <summary>
		/// Rejected.
		/// </summary>
		[EnumMember(Value = "rejected")]
		Rejected,
	}
}
