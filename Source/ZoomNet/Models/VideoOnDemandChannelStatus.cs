using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a VOD channel.
	/// </summary>
	public enum VideoOnDemandChannelStatus
	{
		/// <summary>Published.</summary>
		[EnumMember(Value = "PUBLISHED ")]
		Published,

		/// <summary>Draft.</summary>
		[EnumMember(Value = "DRAFT ")]
		Draft
	}
}
