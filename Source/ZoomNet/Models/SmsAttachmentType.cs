using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Allowed SMS attachment types.
	/// </summary>
	public enum SmsAttachmentType
	{
		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "OTHER")]
		Other,

		/// <summary>
		/// Png.
		/// </summary>
		[EnumMember(Value = "PNG")]
		Png,

		/// <summary>
		/// Gif.
		/// </summary>
		[EnumMember(Value = "GIF")]
		Gif,

		/// <summary>
		/// Jpg.
		/// </summary>
		[EnumMember(Value = "JPG/JPEG")]
		Jpg,

		/// <summary>
		/// Audio.
		/// </summary>
		[EnumMember(Value = "AUDIO")]
		Audio,

		/// <summary>
		/// Video.
		/// </summary>
		[EnumMember(Value = "VIDEO")]
		Video
	}
}
