using System.Runtime.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call handling subsetting type.
	/// </summary>
	public enum CallHandlingSubsettingsType
	{
		/// <summary>
		/// Call forwarding hours.
		/// </summary>
		[EnumMember(Value = "call_forwarding")]
		CallForwarding,

		/// <summary>
		/// Holiday.
		/// </summary>
		[EnumMember(Value = "holiday")]
		Holiday,

		/// <summary>
		/// Custom hours.
		/// </summary>
		[EnumMember(Value = "custom_hours")]
		CustomHours,

		/// <summary>
		/// Call handling.
		/// </summary>
		[EnumMember(Value = "call_handling")]
		CallHandling
	}
}
