using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of the video.
	/// </summary>
	public enum HubVideoStatus
	{
		/// <summary>Video is in processing status and not available for viewing yet.</summary>
		[EnumMember(Value = "processing  ")]
		Processing,

		/// <summary>Processing complete and available for viewing.</summary>
		[EnumMember(Value = "done")]
		Ready,

		/// <summary>Processing status is unknown.</summary>
		[EnumMember(Value = "unknown")]
		Unknown,
	}
}
