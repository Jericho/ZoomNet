using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Data center region.
	/// </summary>
	public enum DataCenterRegion
	{
		/// <summary>Europe.</summary>
		[EnumMember(Value = "EU")]
		Europe,

		/// <summary>Hong Kong.</summary>
		[EnumMember(Value = "HK")]
		HongKong,

		/// <summary>Australia.</summary>
		[EnumMember(Value = "AU")]
		Australia,

		/// <summary>India.</summary>
		[EnumMember(Value = "IN")]
		India,

		/// <summary>Latin America.</summary>
		[EnumMember(Value = "LA")]
		LatinAmerica,

		/// <summary>China.</summary>
		[EnumMember(Value = "CN")]
		China,

		/// <summary>United States of America.</summary>
		[EnumMember(Value = "US")]
		UnitedStatesOfAmerica,

		/// <summary>Canada.</summary>
		[EnumMember(Value = "CA")]
		Canada,

		/// <summary>Germany.</summary>
		[EnumMember(Value = "DE")]
		Germany,

		/// <summary>Netherlands.</summary>
		[EnumMember(Value = "NL")]
		Netherlands,

		/// <summary>Mexico.</summary>
		[EnumMember(Value = "MX")]
		Mexico,

		/// <summary>Singapore.</summary>
		[EnumMember(Value = "SG")]
		Singapore,

		/// <summary>Ireland.</summary>
		[EnumMember(Value = "IE")]
		Ireland,

		/// <summary>San Jose (SJ).</summary>
		[EnumMember(Value = "SJ")]
		SanJose1,

		/// <summary>San Jose (SC).</summary>
		[EnumMember(Value = "SC")]
		SanJose2,

		/// <summary>San Jose (SX).</summary>
		[EnumMember(Value = "SX")]
		SanJose3,

		/// <summary>San Jose (SJC).</summary>
		[EnumMember(Value = "SJC")]
		SanJose4,

		/// <summary>New York.</summary>
		[EnumMember(Value = "NY")]
		NewYork,

		/// <summary>Denver.</summary>
		[EnumMember(Value = "DV")]
		Denver,

		/// <summary>Virginia.</summary>
		[EnumMember(Value = "IAD")]
		Virginia,

		/// <summary>New Jersey.</summary>
		[EnumMember(Value = "NX")]
		NewJersey,

		/// <summary>Toronto (TR).</summary>
		[EnumMember(Value = "TR")]
		Toronto1,

		/// <summary>Toronto (YYZ).</summary>
		[EnumMember(Value = "YYZ")]
		Toronto2,

		/// <summary>Vancouver (VN).</summary>
		[EnumMember(Value = "VN")]
		Vancouver1,

		/// <summary>Vancouver (YVR).</summary>
		[EnumMember(Value = "YVR")]
		Vancouver2,

		/// <summary>Amsterdam (AM).</summary>
		[EnumMember(Value = "AM")]
		Amsterdam1,

		/// <summary>Amsterdam (AMS).</summary>
		[EnumMember(Value = "AMS")]
		Amsterdam2,

		/// <summary>Frankfurt (FR).</summary>
		[EnumMember(Value = "FR")]
		Frankfurt1,

		/// <summary>Frankfurt (FRA).</summary>
		[EnumMember(Value = "FRA")]
		Frankfurt2,

		/// <summary>Leipzig.</summary>
		[EnumMember(Value = "LEJ")]
		Leipzig,

		/// <summary>Zurich.</summary>
		[EnumMember(Value = "ZRH")]
		Zurich,

		/// <summary>Hong Kong (HK).</summary>
		[EnumMember(Value = "HK")]
		HongKong1,

		/// <summary>Hong Kong (HKG).</summary>
		[EnumMember(Value = "HKG")]
		HongKong2,

		/// <summary>Singapore (SG).</summary>
		[EnumMember(Value = "SG")]
		Singapore1,

		/// <summary>Singapore (SIN).</summary>
		[EnumMember(Value = "SIN")]
		Singapore2,

		/// <summary>Tokyo.</summary>
		[EnumMember(Value = "TY")]
		Tokyo,

		/// <summary>Narita.</summary>
		[EnumMember(Value = "NRT")]
		Narita,

		/// <summary>Osaka (OS).</summary>
		[EnumMember(Value = "OS")]
		Osaka1,

		/// <summary>Osaka (KIX).</summary>
		[EnumMember(Value = "KIX")]
		Osaka2,

		/// <summary>Sydney (SY).</summary>
		[EnumMember(Value = "SY")]
		Sydney1,

		/// <summary>Sydney (SYD).</summary>
		[EnumMember(Value = "SYD")]
		Sydney2,

		/// <summary>Melbourne (ME).</summary>
		[EnumMember(Value = "ME")]
		Melbourne1,

		/// <summary>Melbourne (MEL).</summary>
		[EnumMember(Value = "MEL")]
		Melbourne2,

		/// <summary>Mumbai (MB).</summary>
		[EnumMember(Value = "MB")]
		Mumbai1,

		/// <summary>Mumbai (BOM).</summary>
		[EnumMember(Value = "BOM")]
		Mumbai2,

		/// <summary>Hyderabad (HY).</summary>
		[EnumMember(Value = "HY")]
		Hyderabad1,

		/// <summary>Hyderabad (HYD).</summary>
		[EnumMember(Value = "HYD")]
		Hyderabad2,

		/// <summary>Tianjin.</summary>
		[EnumMember(Value = "TJ")]
		Tianjin,

		/// <summary>Sao Paulo.</summary>
		[EnumMember(Value = "SP")]
		SaoPaulo,

		/// <summary>Mexico (MX).</summary>
		[EnumMember(Value = "MX")]
		Mexico1,

		/// <summary>Mexico (QRO).</summary>
		[EnumMember(Value = "QRO")]
		Mexico2,

		/// <summary>Global Service Backup.</summary>
		[EnumMember(Value = "GSB")]
		GlobalServiceBackup,

		/// <summary>Cloud.</summary>
		[EnumMember(Value = "Cloud")]
		Cloud,

		/// <summary>Silicon Valley Gov.</summary>
		[EnumMember(Value = "SV")]
		SiliconValleyGov,

		/// <summary>New Jersey Gov.</summary>
		[EnumMember(Value = "NJ")]
		NewJerseyGov,

		/// <summary>Taiwan.</summary>
		/// <remarks>This is an undocumented value. See <a href="https://github.com/Jericho/ZoomNet/issues/267">this GitHub issue</a> for details.</remarks>
		[EnumMember(Value = "TW")]
		Taiwan,

		/// <summary>Switzerland.</summary>
		/// <remarks>This is an undocumented value. See <a href="https://github.com/Jericho/ZoomNet/issues/267">this GitHub issue</a> for details.</remarks>
		[EnumMember(Value = "CH")]
		Switzerland,
	}
}
