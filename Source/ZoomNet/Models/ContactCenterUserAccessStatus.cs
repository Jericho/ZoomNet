using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the access status of a contact center user.
	/// </summary>
	public enum ContactCenterUserAccessStatus
	{
		/// <summary>Active.</summary>
		[EnumMember(Value = "active")]
		Active,

		/// <summary>Inactive.</summary>
		[EnumMember(Value = "inactive")]
		Inactive,
	}
}
