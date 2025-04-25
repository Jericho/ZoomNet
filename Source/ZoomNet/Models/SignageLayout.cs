using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the layout of the digital signage.
	/// </summary>
	public enum SignageLayout
	{
		/// <summary>Standard Center.</summary>
		[EnumMember(Value = "standard")]
		Standard,

		/// <summary>Video + Content.</summary>
		[EnumMember(Value = "video_content")]
		VideoAndContent
	}
}
