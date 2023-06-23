using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate whether a meeting (or webinar) is recorded and if so, where the file is saved.
	/// </summary>
	public enum AutoRecordingType
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
