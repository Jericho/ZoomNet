using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Emergency call routing source.
	/// </summary>
	public enum EmergencyCallSource
	{
		/// <summary>
		/// Zoom phone.
		/// </summary>
		[EnumMember(Value = "ZOOM")]
		Zoom,

		/// <summary>
		/// Bring your own carrier.
		/// </summary>
		[EnumMember(Value = "BYOC Carrier")]
		Byoc,

		/// <summary>
		/// Mobile phone.
		/// </summary>
		[EnumMember(Value = "Mobile Carrier")]
		Mobile,
	}
}
