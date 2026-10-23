using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the platform used when creating the meeting.</summary>
	public enum MeetingCreationSource
	{
		/// <summary>Unknown or not specified.</summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>Other.</summary>
		[EnumMember(Value = "other")]
		Other,

		/// <summary>API.</summary>
		[EnumMember(Value = "open_api")]
		Api,

		/// <summary>Web Portal.</summary>
		[EnumMember(Value = "web_portal")]
		WebPortal,
	}
}
