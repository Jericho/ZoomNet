using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Emergency call extension type.
	/// </summary>
	public enum EmergencyCallExtensionType
	{
		/// <summary>
		/// Zoom user.
		/// </summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>
		/// Interop.
		/// </summary>
		[EnumMember(Value = "interop")]
		Interop,

		/// <summary>
		/// Common area phone.
		/// </summary>
		[EnumMember(Value = "commonAreaPhone")]
		CommonAreaPhone,
	}
}
