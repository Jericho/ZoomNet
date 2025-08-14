using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the channel type.
	/// </summary>
	public enum VideoOnDemandChannelType
	{
		/// <summary>The channel content hosted on the Zoom Events platform.</summary>
		[EnumMember(Value = "VIDEO_LIST_HUB")]
		VideoListHub,

		/// <summary>The embedded video channel.</summary>
		/// <remarks>The video channel (with multiple videos) can be embedded on a third-party website.</remarks>
		[EnumMember(Value = "MULTI_VIDEO_EMBEDDED")]
		MultiVideoEmbedded,

		/// <summary>The single video embedded channel.</summary>
		/// <remarks>The single video channel can be embedded on a third-party website.</remarks>
		[EnumMember(Value = "SINGLE_VIDEO_EMBEDDED")]
		SingleVideoEmbedded
	}
}
