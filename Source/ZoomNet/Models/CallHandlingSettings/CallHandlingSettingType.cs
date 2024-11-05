using System.Runtime.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call handling settings type.
	/// </summary>
	public enum CallHandlingSettingType
	{
		/// <summary>
		/// Business hours.
		/// </summary>
		[EnumMember(Value = "business_hours")]
		BusinessHours,

		/// <summary>
		/// Closed hours.
		/// </summary>
		[EnumMember(Value = "closed_hours")]
		ClosedHours,

		/// <summary>
		/// Holidays hours.
		/// </summary>
		[EnumMember(Value = "holiday_hours")]
		HolidayHours
	}
}
