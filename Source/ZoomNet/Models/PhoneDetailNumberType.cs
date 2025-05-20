using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specifies the type of phone number in the Zoom Phone API.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// <item><term>toll</term><description>Toll number.</description></item>
	/// <item><term>tollfree</term><description>Toll-free number.</description></item>
	/// </list>
	/// </remarks>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PhoneDetailNumberType
	{
		/// <summary>
		/// Toll number.
		/// </summary>
		[EnumMember(Value = "toll")]
		Toll,

		/// <summary>
		/// Toll-free number.
		/// </summary>
		[EnumMember(Value = "tollfree")]
		TollFree,
	}
}
