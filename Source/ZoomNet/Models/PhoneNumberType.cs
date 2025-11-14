using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specifies the type of phone number in the Zoom Phone API.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// <item><term>toll</term><description>Toll number.</description></item>
	/// <item><term>tollfree</term><description>Toll-free number.</description></item>
	/// <item><term>media_link</term><description>Media link.</description></item>
	/// </list>
	/// </remarks>
	public enum PhoneNumberType
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

		/// <summary>
		/// Media link.
		/// </summary>
		[EnumMember(Value = "media_link")]
		MediaLink,
	}
}
