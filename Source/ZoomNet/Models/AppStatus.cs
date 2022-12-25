using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of an app.
	/// </summary>
	public enum AppStatus
	{
		/// <summary>
		/// The app has been published.
		/// </summary>
		[EnumMember(Value = "PUBLISHED")]
		Published,

		/// <summary>
		/// The app is not published.
		/// </summary>
		[EnumMember(Value = "UNPUBLISHED")]
		Unpublished,
	}
}
