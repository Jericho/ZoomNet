using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to specify the type of virtual background.
	/// </summary>
	public enum VirtualBackgroundType
	{
		/// <summary>Image</summary>
		[EnumMember(Value = "image")]
		Image,

		/// <summary>Video</summary>
		[EnumMember(Value = "video")]
		Video
	}
}
