using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of app request.
	/// </summary>
	public enum AppRequestType
	{
		/// <summary>
		/// Active.
		/// </summary>
		[EnumMember(Value = "active_requests")]
		Active,

		/// <summary>
		/// Past.
		/// </summary>
		[EnumMember(Value = "past_request")]
		Past
	}
}
