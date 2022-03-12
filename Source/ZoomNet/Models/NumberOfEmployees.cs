using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// NumberOfEmployees.
	/// </summary>
	public enum NumberOfEmployees
	{
		/// <summary>Unknown</summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>1-20</summary>
		[EnumMember(Value = "1-20")]
		Between_0001_and_0020,

		/// <summary>21-50</summary>
		[EnumMember(Value = "21-50")]
		Between_0021_and_0050,

		/// <summary>21-50</summary>
		[EnumMember(Value = "51-100")]
		Between_0051_and_0100,

		/// <summary>101-500</summary>
		[EnumMember(Value = "101-500")]
		Between_0101_and_0500,

		/// <summary>501-1,000</summary>
		[EnumMember(Value = "501-1,000")] // There's a typo in the documentation: it says 500-1,000 but the API rejects that value. The actual value is 501-1,000
		Between_0501_and_1000,

		/// <summary>21-50</summary>
		[EnumMember(Value = "1,001-5,000")]
		Between_1001_and_5000,

		/// <summary>5,001-10,000</summary>
		[EnumMember(Value = "5,001-10,000")]
		Between_5001_and_10000,

		/// <summary>5,001-10,000</summary>
		[EnumMember(Value = "More than 10,000")]
		More_than_10000,
	}
}
