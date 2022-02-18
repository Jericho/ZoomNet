using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Purchasing time frame.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PurchasingTimeFrame
	{
		/// <summary>Within a month</summary>
		[EnumMember(Value = "Within a month")]
		Within_a_month,

		/// <summary>1-3 months</summary>
		[EnumMember(Value = "1-3 months")]
		Between_1_and_3_months,

		/// <summary>4-6 months</summary>
		[EnumMember(Value = "4-6 months")]
		Between_4_and_6_months,

		/// <summary>More than 6 months</summary>
		[EnumMember(Value = "More than 6 months")]
		Nore_than_6_months,
	}
}
