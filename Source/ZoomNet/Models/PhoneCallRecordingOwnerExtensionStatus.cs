using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the phone call owner extension status.
	/// </summary>
	public enum PhoneCallRecordingOwnerExtensionStatus
	{
		/// <summary>
		/// Inactive.
		/// </summary>
		[EnumMember(Value = "inactive")]
		Inactive,

		/// <summary>
		/// Deleted.
		/// </summary>
		[EnumMember(Value = "deleted")]
		Deleted
	}
}
