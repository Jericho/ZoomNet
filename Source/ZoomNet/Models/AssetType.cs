using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Asset's types.
	/// </summary>
	public enum AssetType
	{
		/// <summary>Audio asset.</summary>
		[EnumMember(Value = "audio")]
		Audio,

		/// <summary>Image asset.</summary>
		[EnumMember(Value = "image")]
		Image,

		/// <summary>Slides asset.</summary>
		[EnumMember(Value = "slides")]
		Slides,

		/// <summary>Text asset.</summary>
		[EnumMember(Value = "text")]
		Text,

		/// <summary>Video asset.</summary>
		[EnumMember(Value = "video")]
		Video,

		/// <summary>Saved reply asset.</summary>
		[EnumMember(Value = "saved_reply")]
		SavedReply,
	}
}
