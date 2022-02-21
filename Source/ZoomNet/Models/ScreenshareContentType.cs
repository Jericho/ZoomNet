using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type content share in a meeting/webinar.
	/// </summary>
	public enum ScreenshareContentType
	{
		/// <summary>
		/// Application.
		/// </summary>
		[EnumMember(Value = "application")]
		Application,

		/// <summary>
		/// Whiteboard.
		/// </summary>
		[EnumMember(Value = "whiteboard")]
		Whiteboard,

		/// <summary>
		/// Desktop.
		/// </summary>
		[EnumMember(Value = "desktop")]
		Desktop
	}
}
