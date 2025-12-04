using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Device provisioning type.
	/// </summary>
	public enum DeviceProvisioningType
	{
		/// <summary>
		/// Zero touch provisioning.
		/// </summary>
		[EnumMember(Value = "ztp")]
		ZeroTouch,

		/// <summary>
		/// Assisted provisioning.
		/// </summary>
		[EnumMember(Value = "assisted")]
		Assisted,

		/// <summary>
		/// Manual provisioning.
		/// </summary>
		[EnumMember(Value = "manual")]
		Manual,
	}
}
