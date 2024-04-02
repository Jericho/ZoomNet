using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Allowed SMS attachment types.
	/// </summary>
	public enum SmsAttachmentType
	{
		/// <summary>
		/// OTHER.
		/// </summary>
		[EnumMember(Value = "OTHER")]
		OTHER,

		/// <summary>
		/// PNG.
		/// </summary>
		[EnumMember(Value = "PNG")]
		PNG,

		/// <summary>
		/// GIF.
		/// </summary>
		[EnumMember(Value = "GIF")]
		GIF,

		/// <summary>
		/// JPG.
		/// </summary>
		[EnumMember(Value = "JPG")]
		JPG,

		/// <summary>
		/// AUDIO.
		/// </summary>
		[EnumMember(Value = "AUDIO")]
		AUDIO,

		/// <summary>
		/// VIDEO.
		/// </summary>
		[EnumMember(Value = "VIDEO")]
		VIDEO
	}
}
