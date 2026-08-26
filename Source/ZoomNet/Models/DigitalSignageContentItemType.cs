using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the type of a content item in the Zoom Rooms Digital Signage content library.</summary>
	public enum DigitalSignageContentItemType
	{
		/// <summary>Image.</summary>
		[EnumMember(Value = "image")]
		Image,

		/// <summary>Video.</summary>
		[EnumMember(Value = "video")]
		Video,

		/// <summary>URL.</summary>
		[EnumMember(Value = "url")]
		Url,
	}
}
