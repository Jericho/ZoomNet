using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate where the audio recording is saved.
	/// </summary>
	public enum RecordingType
	{
		/// <summary>
		/// Record on local.
		/// </summary>
		[EnumMember(Value = "local")]
		OnLocal,

		/// <summary>
		/// Record on cloud.
		/// </summary>
		[EnumMember(Value = "cloud")]
		OnCloud,

		/// <summary>
		/// Do not record.
		/// </summary>
		[EnumMember(Value = "none")]
		Disabled
	}
}
