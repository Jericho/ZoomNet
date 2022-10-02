using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Data center region.
	/// </summary>
	public enum DataCenterRegion
	{
		/// <summary>Europe</summary>
		[EnumMember(Value = "EU")]
		Europe,

		/// <summary>Hong Kong</summary>
		[EnumMember(Value = "HK")]
		HongKong,

		/// <summary>Australia</summary>
		[EnumMember(Value = "AU")]
		Australia,

		/// <summary>India</summary>
		[EnumMember(Value = "IN")]
		India,

		/// <summary>Latin America</summary>
		[EnumMember(Value = "LA")]
		LatinAmerica,

		/// <summary>Tokyo</summary>
		[EnumMember(Value = "TY")]
		Tokyo,

		/// <summary>China</summary>
		[EnumMember(Value = "CN")]
		China,

		/// <summary>United States of America</summary>
		[EnumMember(Value = "US")]
		UnitedStatesOfAmerica,

		/// <summary>Canada</summary>
		[EnumMember(Value = "CA")]
		Canada,

		/// <summary>Canada</summary>
		[EnumMember(Value = "DE")]
		Germany,

		/// <summary>Canada</summary>
		[EnumMember(Value = "NL")]
		Netherlands,

		/// <summary>Mexico</summary>
		[EnumMember(Value = "MX")]
		Mexico,

		/// <summary>Singapore</summary>
		[EnumMember(Value = "SG")]
		Singapore,

		/// <summary>Ireland</summary>
		[EnumMember(Value = "IE")]
		Ireland,
	}
}
