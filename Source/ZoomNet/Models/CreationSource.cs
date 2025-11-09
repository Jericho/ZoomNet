using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The platform through which meeting or webinar was created.
	/// </summary>
	public enum CreationSource
	{
		/// <summary>
		/// Created through another platform.
		/// </summary>
		[EnumMember(Value = "other")]
		Other,

		/// <summary>
		/// Created through Open API.
		/// </summary>
		[EnumMember(Value = "open_api")]
		OpenApi,

		/// <summary>
		/// Created through web portal.
		/// </summary>
		[EnumMember(Value = "web_portal")]
		WebPortal,
	}
}
