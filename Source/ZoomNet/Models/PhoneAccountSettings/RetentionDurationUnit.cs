using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Unit of the retention duration value.
	/// </summary>
	public enum RetentionDurationUnit
	{
		/// <summary>
		/// Retention duration is specified in years.
		/// </summary>
		[EnumMember(Value = "year")]
		Year,

		/// <summary>
		/// Retention duration is specified in months.
		/// </summary>
		[EnumMember(Value = "month")]
		Month,

		/// <summary>
		/// Retention duration is specified in days.
		/// </summary>
		[EnumMember(Value = "day")]
		Day,
	}
}
